using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sheaft.Application.Features.Company;
using Sheaft.Domain.Common;
using Sheaft.Domain.Security;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Application.Tests.Company
{
    [TestClass]
    public class RegisterCompanyTests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var appDbContext = AppDbContextFactory.GetUnitTestDbContext();
            var requestUser = new RequestUser(Guid.NewGuid(), "test", "test@test.com", "firstname",
                "lastname", "phone", "picture", new List<string> {RoleDefinition.Supplier});

            var command = new RegisterCompany.Command(requestUser, "commercial_name",
                new RegisterCompany.OwnerDto(requestUser),
                new RegisterCompany.LegalsDto("legal_name", "1121212121212145", "vatNumber",
                    new RegisterCompany.AddressDto("1 street", "74000", "test", "france")),
                new RegisterCompany.AddressDto("2 street", "73000", "shipping_city", "france"),
                new RegisterCompany.AddressDto("1 street", "76000", "billing_city", "france"));

            var result = await new RegisterCompany.CommandHandler(new CompanyRepository(appDbContext),
                new UnitOfWork(new MediatR.Mediator(), appDbContext)).Handle(command, CancellationToken.None);
        }
    }
}