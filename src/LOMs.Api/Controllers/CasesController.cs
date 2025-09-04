using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Cases.Commands.CreateCase;
using LOMs.Application.Features.Cases.Queries.GetCaseByIdQuery;
using LOMs.Application.Features.People.Clients.Commands.CreateClient;
using LOMs.Contract.Commons.Enums;
using LOMs.Contract.Requests.Cases;
using LOMs.Domain.Cases.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LOMs.Api.Controllers;

[Route("api/cases")]
public class CasesController(ICommandMediator command, IQueryMediator query) : ApiController
{
    [HttpGet("{caseId:guid}", Name = "GetCaseById")]
    public async Task<IActionResult> GetById(Guid caseId, CancellationToken ct)
    {
        var result = await query.QueryAsync(new GetCaseByIdQuery(caseId), ct);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCase([FromForm] CreateCaseRequest request, CancellationToken ct)
    {
        // ✅ تحقق من صحة القيم المدخلة
        if (!Enum.IsDefined(typeof(LOMs.Domain.Cases.Enums.CourtType), request.CourtType))
            return Problem("نوع المحكمة غير صالح.");

        if (!Enum.IsDefined(typeof(LOMs.Domain.Cases.Enums.PartyRole), request.PartyRole))
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

        List<CreateContractWithCaseCommand> contracts = new List<CreateContractWithCaseCommand>();
        if (request.HasContracts)
        {
            if (request.Contracts is null)
                return Problem("يجب ادخال عقد او اكثر عند وجود عقود");

            if (!request.Contracts.Any(x => !Enum.IsDefined(typeof(LOMs.Domain.Cases.Enums.ContractType), (LOMs.Domain.Cases.Enums.ContractType)x.ContractType)))
            {
                return Problem("الرجاء اختيار نوع عقد متوفر.");
            }

            // Use a foreach loop to handle asynchronous file operations
            foreach (var contractRequest in request.Contracts)
            {
                // Read file content into a byte array
                if (contractRequest.ContractFile == null || contractRequest.ContractFile.Length == 0)
                {
                    return Problem("ملف العقد مطلوب.");
                }

                // Map to the command with the file content and name
                var contractCommand = new CreateContractWithCaseCommand(
                    (LOMs.Domain.Cases.Enums.ContractType)contractRequest.ContractType,
                    contractRequest.IssueDate,
                    contractRequest.ExpiryDate,
                    contractRequest.TotalAmount,
                    contractRequest.InitialPayment,
                    contractRequest.ContractFile.FileName,
                    contractRequest.IsAssigned
                );
                contracts.Add(contractCommand);
            }
        }
        List<CreatePOAWithCaseCommand> poas = new List<CreatePOAWithCaseCommand>();

        if (request.HasPOAs)
        {
            if (request.POAs is null || !request.POAs.Any())
                return Problem("يجب إدخال وكالة واحدة على الأقل عند تحديد وجود وكالات.");

            foreach (var poa in request.POAs)
            {
                if (string.IsNullOrWhiteSpace(poa.POANumber))
                    return Problem("رقم الوكالة مطلوب.");

                if (string.IsNullOrWhiteSpace(poa.IssuingAuthority))
                    return Problem("جهة إصدار الوكالة مطلوبة.");

                if (poa.IssueDate > DateOnly.FromDateTime(DateTime.UtcNow))
                    return Problem("تاريخ إصدار الوكالة لا يمكن أن يكون في المستقبل.");

                if (poa.AttachmentFile is null || poa.AttachmentFile.Length == 0)
                    return Problem("ملف الوكالة مطلوب ويجب أن يكون صالحًا.");
            }

            poas = request.POAs.Select(poa =>
                new CreatePOAWithCaseCommand(
                    poa.POANumber.Trim(),
                    poa.IssueDate,
                    poa.IssuingAuthority.Trim(),
                    poa.AttachmentFile.FileName
                )).ToList();
        }


        var commandRequest = new CreateCaseCommand(
                Clients: clientModels,
                CaseNumber: request.CaseNumber,
                CaseNotes: request.CaseSubject,
                CourtType: (LOMs.Domain.Cases.Enums.CourtType)request.CourtType,
                PartyRole: (LOMs.Domain.Cases.Enums.PartyRole)request.PartyRole,
                ClientRequests: request.ClientRequestDetails,
                EstimatedReviewDate: request.EstimatedReviewDate,
                LawyerOpinion: request.LawyerOpinion,
                IsDraft: request.IsDraft,
                HasContracts: request.HasContracts,
                Contracts: contracts,
                HasPOAs : request.HasPOAs,
                POAs: poas,
                AssignedOfficerId: request.AssignedOfficer
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


