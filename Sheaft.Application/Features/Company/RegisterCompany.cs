using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Mediator;
using Sheaft.Application.Persistence;
using Sheaft.Domain.Common;
using Sheaft.Domain.Events.Company;

namespace Sheaft.Application.Features.Company
{
    public static class RegisterCompany
    {
        public class Command : BaseCommand<Guid>
        {
            public string Name { get; }
            public bool OpenForNewContracts { get; }
            public OwnerDto Owner { get; }
            public LegalsDto Legals { get; }
            public AddressDto ShippingAddress { get; }
            public AddressDto BillingAddress { get; }

            public Command(RequestUser user, string name, OwnerDto owner, LegalsDto legals, AddressDto shippingAddress, AddressDto billingAddress = null, bool openForNewContracts = true)
                : base(user)
            {
                Name = name;
                Owner = owner;
                Legals = legals;
                ShippingAddress = shippingAddress;
                BillingAddress = billingAddress;
                OpenForNewContracts = openForNewContracts;
            }
        }

        public class OwnerDto
        {
            public OwnerDto(RequestUser user)
            {
                UserId = user.Id;
                Firstname = user.Firstname;
                Lastname = user.Lastname;
                Email = user.Email;
                Phone = user.Phone;
                Picture = user.Picture;
            }
            
            public Guid UserId { get; }
            public string Firstname { get; }
            public string Lastname { get; }
            public string Email { get; }
            public string Phone { get; }
            public string Picture { get; }
        }

        public class LegalsDto
        {
            public LegalsDto(string name, string siret, string vatNumber, AddressDto address)
            {
                Name = name;
                Siret = siret;
                VatNumber = vatNumber;
                Address = address;
            }
            
            public string Name { get; }
            public string Siret { get; }
            public string VatNumber { get; }
            public AddressDto Address { get; }
        }

        public class AddressDto
        {
            public AddressDto(string streetAddress, string postalCode, string city, string country)
            {
                StreetAddress = streetAddress;
                PostalCode = postalCode;
                City = city;
                Country = country;
            }
            
            public string StreetAddress { get; }
            public string PostalCode { get; }
            public string City { get; }
            public string Country { get; }
        }

        public class CommandHandler : ICommandHandler<Command, Guid>
        {
            private readonly IRepository<Domain.Company> _repository;
            private readonly IUnitOfWork _unitOfWork;

            public CommandHandler(IRepository<Domain.Company> repository,
                IUnitOfWork unitOfWork)
            {
                _repository = repository;
                _unitOfWork = unitOfWork;
            }
            
            public async Task<Result<Guid>> Handle(Command request, CancellationToken token)
            {
                try
                {
                    await _repository.Add(new Domain.Company(request.Name, request.), token);
                    await _unitOfWork.SaveChanges(token);
                }
                catch (Exception e)
                {
                    return Result<Guid>.Failure("Une erreur est survenue pendant la création de la société.", e);
                }
            }
        }

        public class EventHandler : IEventHandler<CompanyRegistered>
        {
            public Task Handle(DomainEventNotification<CompanyRegistered> notification, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}