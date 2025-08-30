using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.People.Clients.Commands.CreateClient;

    public sealed record CreateClientCommand(CreatePersonCommand Person) : ICommand<Result<ClientDto>>;


