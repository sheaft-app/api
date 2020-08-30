using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Powells.CouponCode;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using Sheaft.Options;
using Sheaft.Services.Interop;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Services
{
    public class IdentifierService : IIdentifierService
    {
        private readonly CloudTableClient _tableClient;
        private readonly StorageOptions _storageOptions;
        private readonly SponsoringOptions _sponsoringOptions;

        public IdentifierService(IOptionsSnapshot<StorageOptions> storageOptions, IOptionsSnapshot<SponsoringOptions> sponsoringOptions)
        {
            _storageOptions = storageOptions.Value;
            _sponsoringOptions = sponsoringOptions.Value;

            var cloudStorageAccount = CloudStorageAccount.Parse(_storageOptions.ConnectionString);
            _tableClient = cloudStorageAccount.CreateCloudTableClient();
        }

        public string RemoveDiacritics(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var formD = value.Normalize(NormalizationForm.FormD);
            var chars = new char[formD.Length];
            var count = 0;

            foreach (var c in formD.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
            {
                chars[count++] = c;
            }

            var noDiacriticsFormD = new string(chars, 0, count);
            return noDiacriticsFormD.Normalize(NormalizationForm.FormC);
        }

        public string NormalizeIdentifier(string name, string pattern = "[^[a-zA-Z0-9]]*")
        {
            var sourceInFormD = name.Normalize(NormalizationForm.FormD);
            var strBuilder = new StringBuilder();
            foreach (var c in from c in sourceInFormD
                              let uc = CharUnicodeInfo.GetUnicodeCategory(c)
                              where uc != UnicodeCategory.NonSpacingMark
                              select c)
                strBuilder.Append(c);

            var result = new Regex(pattern).Replace(strBuilder.ToString(), "");
            return result.Normalize(NormalizationForm.FormC).ToLowerInvariant();
        }

        public async Task<CommandResult<string>> GetNextOrderReferenceAsync(Guid serialNumber, CancellationToken token)
        {
            var uuid = await GetNextUuidAsync(_storageOptions.Tables.OrdersReferences, serialNumber, token);
            if (!uuid.Success)
                return new CommandResult<string>(uuid.Exception);

            var identifier = GenerateEanIdentifier(uuid.Result, 13);
            return new CommandResult<string>(identifier);
        }

        public async Task<CommandResult<string>> GetNextProductReferenceAsync(Guid serialNumber, CancellationToken token)
        {
            var uuid = await GetNextUuidAsync(_storageOptions.Tables.ProductsReferences, serialNumber, token);
            if (!uuid.Success)
                return new CommandResult<string>(uuid.Exception);

            var identifier = GenerateEanIdentifier(uuid.Result, 13);
            return new CommandResult<string>(identifier);
        }

        public async Task<CommandResult<string>> GetNextSponsoringCode(CancellationToken token)
        {
            try
            {
                var table = _tableClient.GetTableReference(_storageOptions.Tables.SponsoringCodes);
                await table.CreateIfNotExistsAsync(token);

                var opts = new Powells.CouponCode.Options { PartLength = _sponsoringOptions.CodeLength, Parts = _sponsoringOptions.CodeParts };
                var ccb = new CouponCodeBuilder();
                var badWords = ccb.BadWordsList;
                var code = "";

                var concurrentUpdateError = false;

                do
                {
                    try
                    {
                        code = ccb.Generate(opts);

                        var tableResults = await table.ExecuteAsync(TableOperation.Retrieve<SponsoringTableEntity>("code", code), token);
                        if ((SponsoringTableEntity)tableResults?.Result != null)
                        {
                            concurrentUpdateError = true;
                        }
                        else
                        {
                            await table.ExecuteAsync(TableOperation.Insert(new SponsoringTableEntity
                            {
                                PartitionKey = "code",
                                RowKey = code,
                            }), token);
                        }

                        concurrentUpdateError = false;
                    }
                    catch (StorageException e)
                    {
                        if (e.RequestInformation.HttpStatusCode == 412 || e.RequestInformation.HttpStatusCode == 409)
                            concurrentUpdateError = true;
                        else
                            throw;
                    }
                } while (concurrentUpdateError);

                return new CommandResult<string>(code);
            }
            catch (Exception e)
            {
                return new CommandResult<string>(e, MessageKind.Identifier_SponsorCode_Error);
            }
        }
        private static string GenerateEanIdentifier(long value, int length)
        {
            if (value.ToString().Length > 12)
                throw new ArgumentException("Invalid EAN length", nameof(value));

            if (length > 12)
            {
                var str = value.ToString("000000000000");
                var checksum = CalculateChecksum(str);

                return value.ToString("000000000000") + checksum;
            }

            return value.ToString("000000000000");
        }

        private static int CalculateChecksum(string code)
        {
            if (code == null || code.Length != 12)
                throw new ArgumentException("Code length should be 12, i.e. excluding the checksum digit", nameof(code));

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int v;
                if (!int.TryParse(code[i].ToString(), out v))
                    throw new ArgumentException("Invalid character encountered in specified code.", nameof(code));
                sum += (i % 2 == 0 ? v : v * 3);
            }
            int check = 10 - (sum % 10);
            return check % 10;
        }

        private async Task<CommandResult<long>> GetNextUuidAsync(string container, Guid companyIdentifier, CancellationToken token)
        {
            try
            {
                var table = _tableClient.GetTableReference(container);
                await table.CreateIfNotExistsAsync(token);

                var key = "identifier";
                long id = 0;

                var concurrentUpdateError = false;

                do
                {
                    try
                    {
                        var tableResults = await table.ExecuteAsync(TableOperation.Retrieve<IdentifierTableEntity>(companyIdentifier.ToString("N"), key), token);
                        var results = (IdentifierTableEntity)tableResults.Result;
                        if (results != null)
                        {
                            id = results.Id;
                            results.Id++;
                            await table.ExecuteAsync(TableOperation.Replace(results), token);
                        }
                        else
                        {
                            id = 1;
                            await table.ExecuteAsync(TableOperation.Insert(new IdentifierTableEntity
                            {
                                PartitionKey = companyIdentifier.ToString("N"),
                                RowKey = key,
                                Id = id
                            }), token);
                        }

                        concurrentUpdateError = false;
                    }
                    catch (StorageException e)
                    {
                        if (e.RequestInformation.HttpStatusCode == 412 || e.RequestInformation.HttpStatusCode == 409)
                            concurrentUpdateError = true;
                        else
                            throw;
                    }
                } while (concurrentUpdateError);

                return new CommandResult<long>(id);
            }
            catch (Exception e)
            {
                return new CommandResult<long>(e, MessageKind.Identifier_Uuid_Error);
            }
        }

        private class IdentifierTableEntity : TableEntity
        {
            public long Id { get; set; }
        }

        private class SponsoringTableEntity : TableEntity
        {
        }
    }
}
