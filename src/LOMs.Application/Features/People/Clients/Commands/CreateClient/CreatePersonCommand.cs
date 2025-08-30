using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People;
using System.Windows.Input;

namespace LOMs.Application.Features.People.Clients.Commands.CreateClient;

public sealed record CreatePersonCommand(string FullName,
    string NationalId, DateOnly BirthDate, string PhoneNumber, string Address) : ICommand<Result<PersonDto>>;



