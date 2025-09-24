using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Application.Features.People.Commands;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People;
using System.Windows.Input;

namespace LOMs.Application.Features.People.Clients.Commands.CreateClient;

public sealed record CreatePersonCommand(string FullName,
    string NationalId,string CountryCode,DateOnly BirthDate, string Address, CreatePhoneCommand Phone) : ICommand<Result<PersonDto>>;



