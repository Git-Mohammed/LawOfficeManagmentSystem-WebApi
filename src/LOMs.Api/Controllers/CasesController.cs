using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Cases.Commands.CreateCase;
using LOMs.Application.Features.People.Clients.Commands.CreateClient;
using LOMs.Contract.Requests.Cases;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Cases.Enums.CourtTypes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Api.Controllers;

[Route("api/cases")]
public class CasesController(ICommandMediator command, IQueryMediator query) : ApiController
{
    //[HttpGet("{caseId:guid}", Name = "GetCaseById")]
    //public async Task<IActionResult> GetById(Guid caseId, CancellationToken ct)
    //{
    //    var result = await query.QueryAsync(new GetCaseByIdQuery(caseId), ct);

    //    return result.Match(
    //        response => Ok(response),
    //        Problem);
    //}
    [HttpPost]
    public async Task<IActionResult> CreateCase([FromBody] CreateCaseRequest request, CancellationToken ct)
    {
        // ✅ تحقق من صحة القيم المدخلة
        if (!Enum.IsDefined(typeof(CourtType), request.CourtType))
            return Problem("نوع المحكمة غير صالح.");

        if (!Enum.IsDefined(typeof(PartyRole), request.Role))
            return Problem("دور العميل في القضية غير صالح.");

        var clientModels = new List<CaseClientModel>();

        try
        {
            clientModels = request.Clients.Select<CaseClientRequest, CaseClientModel>(c =>
            {
                return c switch
                {
                    ExistingClientRequest existing => new ExistingCaseClientModel
                    {
                        ExistingClientId = existing.ClientId
                    },
                    NewClientRequest newClient => new NewCaseClientModel(
                        new CreateClientCommand(
                            new CreatePersonCommand(
                                newClient.Client.Person.FullName,
                                newClient.Client.Person.NationalId,
                                newClient.Client.Person.BirthDate,
                                newClient.Client.Person.PhoneNumber,
                                newClient.Client.Person.Address
                            )
                        )
                    ),
                    _ => throw new ValidationException("كل عنصر في قائمة العملاء يجب أن يحتوي إما على معرف موجود أو بيانات عميل جديدة.")
                };
            }).ToList();
        }
        catch (ValidationException ex)
        {
            return Problem(ex.Message);
        }

        var commandRequest = new CreateCaseCommand(
            Clients: clientModels,
            CaseNumber: request.Number,
            CaseNotes: request.Subject,
            CourtType: (CourtType)request.CourtType,
            Role: (PartyRole)request.Role,
            ClientRequests: request.ClientRequests,
            EstimatedReviewDate: request.EstimatedReviewDate,
            LawyerOpinion: request.LawyerOpinion,
            IsDraft: request.IsDraft,
            AssignedOfficer: request.AssignedOfficer
        );

        var result = await command.SendAsync(commandRequest, ct);

        return result.Match(
            success => CreatedAtRoute(
                routeName: "GetCaseById",
                routeValues: new { caseId = success.Id },
                value: new { caseId = success.Id }),
            error => Problem(error)
        );
    }

}


