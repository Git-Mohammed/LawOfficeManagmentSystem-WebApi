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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LOMs.Api.Controllers;

[Route("api/cases")]
public class CasesController(ICommandMediator command, IQueryMediator query) : ApiController
{
    [HttpGet("{Id:guid}", Name = "GetCaseById")]
    public async Task<IActionResult> GetById(Guid Id, CancellationToken ct)
    {
        var result = await query.QueryAsync(new GetCaseByIdQuery(Id), ct);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCase([FromForm] CreateCaseRequest request, CancellationToken ct)
    {
        // ✅ تحقق من صحة القيم المدخلة
        if (!Enum.IsDefined(typeof(LOMs.Contract.Commons.Enums.CourtType), request.CourtType))
            return Problem("نوع المحكمة غير صالح.");

        if (!Enum.IsDefined(typeof(LOMs.Contract.Commons.Enums.PartyRole), request.PartyRole))
            return Problem("دور العميل في القضية غير صالح.");

        var clientModels = new List<CaseClientModel>();

        try
        {
            List<CaseClientRequest> clients = new List<CaseClientRequest>();

            if (!string.IsNullOrEmpty(request.ClientsJson))
            {


                clients = JsonSerializer.Deserialize<List<CaseClientRequest>>(request.ClientsJson);
            }

            else
            {
                return Problem("يجب ان يكون هناك عملاء");

            }

            if(clients is null ||  clients.Count == 0)
                return Problem("يجب ان يكون هناك عملاء");

            clientModels = clients.Select<CaseClientRequest, CaseClientModel>(c =>
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
                                newClient.Client.Person.CountryCode,
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

        var contracts = new List<CreateContractWithCaseCommand>();

        // The controller receives the flattened DTO
        if (request.HasContracts)
        {
            // 1. Basic validation
            if (request.ContractsData is null || request.ContractsData.Count == 0)
            {
                return Problem("يجب ادخال عقد او اكثر عند وجود عقود");
            }

            if (request.ContractFiles is null || request.ContractFiles.Count != request.ContractsData.Count)
            {
                return Problem("يجب أن يتطابق عدد مرفقات العقود مع عدد بيانات العقود.");
            }

            // 2. Map and process each file and its corresponding data
            for (int i = 0; i < request.ContractsData.Count; i++)
            {
                var contractData = request.ContractsData[i];
                var contractFile = request.ContractFiles[i];

                // 3. Perform specific validation for each item
                if (!Enum.IsDefined(typeof(LOMs.Contract.Commons.Enums.ContractType), (LOMs.Contract.Commons.Enums.ContractType)contractData.ContractType))
                {
                    return Problem("الرجاء اختيار نوع عقد متوفر.");
                }

                if (contractFile.Length == 0)
                {
                    return Problem("ملف العقد مطلوب.");
                }

                // 4. Create the command with the file stream and filename
                var contractCommand = new CreateContractWithCaseCommand(
                    (LOMs.Domain.Cases.Enums.ContractType)contractData.ContractType,
                    contractData.IssueDate,
                    contractData.ExpiryDate,
                    contractData.TotalAmount,
                    contractData.InitialPayment,
                    contractFile.OpenReadStream(),
                    contractFile.FileName,
                    contractData.IsAssigned
                );
                contracts.Add(contractCommand);
            }
            // The rest of your controller logic continues here
        }
        List<CreatePOAWithCaseCommand> poas = new List<CreatePOAWithCaseCommand>();

        if (request.HasPOAs)
        {
            // 1. Validate that the data and files lists exist and have the same count.
            if (request.POAsData is null || request.POAFiles is null || request.POAFiles.Count != request.POAsData.Count)
            {
                return Problem("يجب أن يتطابق عدد مرفقات الوكالات مع عدد بيانات الوكالات.");
            }

            // 2. Use a for loop to correctly match each file with its data.
            for (int i = 0; i < request.POAsData.Count; i++)
            {
                var poaData = request.POAsData[i];
                var poaFile = request.POAFiles[i];

                // 3. Perform specific validation for each item.
                if (string.IsNullOrWhiteSpace(poaData.POANumber))
                    return Problem("رقم الوكالة مطلوب.");

                if (string.IsNullOrWhiteSpace(poaData.IssuingAuthority))
                    return Problem("جهة إصدار الوكالة مطلوبة.");

                if (poaData.IssueDate > DateOnly.FromDateTime(DateTime.UtcNow))
                    return Problem("تاريخ إصدار الوكالة لا يمكن أن يكون في المستقبل.");

                if (poaFile is null || poaFile.Length == 0)
                    return Problem("ملف الوكالة مطلوب ويجب أن يكون صالحًا.");

                // 4. Create the command with the file stream and filename.
                var poaCommand = new CreatePOAWithCaseCommand(
                    poaData.POANumber.Trim(),
                    poaData.IssueDate,
                    poaData.IssuingAuthority.Trim(),
                    poaFile.OpenReadStream(),
                    poaFile.FileName);

                poas.Add(poaCommand);
            }
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
                AssignedOfficerId: request.AssignedEmployeeId
        );

        var result = await command.SendAsync(commandRequest, ct);

        return result.Match(
            success => CreatedAtRoute(
                routeName: "GetCaseById",
                routeValues: new { Id = success.Id },
                value: new { Id = success.Id }),
            error => Problem(error)
        );
    }

}


