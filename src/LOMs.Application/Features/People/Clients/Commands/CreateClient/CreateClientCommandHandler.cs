using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People;
using LOMs.Domain.People.Clients;
using LOMs.Domain.People.Phones;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.People.Clients.Commands.CreateClient
{
    public sealed class CreateClientCommandHandler(IMapper mapper, ILogger<CreateClientCommandHandler> logger, IAppDbContext context)
        : ICommandHandler<CreateClientCommand, Result<ClientDto>>
    {
        private readonly ILogger<CreateClientCommandHandler> _logger = logger;
        private readonly IAppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<Result<ClientDto>> HandleAsync(CreateClientCommand message, CancellationToken ct = default)
        {
            var phoneNumber = message.Person.Phone.Number?.Trim();
            var code = message.Person.Phone.Code?.Trim();

            if (string.IsNullOrWhiteSpace(phoneNumber))
                return PersonErrors.PhoneNumberRequired;
            if (string.IsNullOrWhiteSpace(code))
                return PersonErrors.PhoneCodeRequired;

            var exists = await _context.Phones.AnyAsync(x => (x.Number == phoneNumber) && (x.Code == code), ct);
            if (exists)
                return PersonErrors.PhoneNumberAlreadyExists;

           var phoneResult = Phone.Create(Guid.NewGuid(), phoneNumber, code);

            var personResult = Person.Create(
                Guid.NewGuid(),
                message.Person.FullName,
                message.Person.NationalId,
                message.Person.CountryCode,
                message.Person.BirthDate,
                message.Person.Address,
                new List<Phone> { phoneResult.Value });

            if (personResult.IsError)
                return personResult.Errors;

            var clientId = Guid.NewGuid();
            var clientResult = Client.Create(clientId, personResult.Value);

            if (clientResult.IsError)
                return clientResult.Errors;

            _context.Clients.Add(clientResult.Value);
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Client created successfully. Id: {ClientId}", clientId);

            return _mapper.Map<Client, ClientDto>(clientResult.Value);
        }

    }
}
