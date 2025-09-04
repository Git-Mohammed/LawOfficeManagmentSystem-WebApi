using LOMs.Application.Features.Cases.Dtos;
using LOMs.Application.Features.People.Clients.Dtos;

namespace LOMs.Application.Features.ClientFiles.Dtos;

public class ClientFileDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string FileNumber { get; set; } = null!;

}
