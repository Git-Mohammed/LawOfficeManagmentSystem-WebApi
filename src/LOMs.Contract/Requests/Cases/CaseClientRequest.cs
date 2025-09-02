using LOMs.Contract.Requests.Clients;
using System.Text.Json.Serialization;

namespace LOMs.Contract.Requests.Cases
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
    [JsonDerivedType(typeof(ExistingClientRequest), "existing")]
    [JsonDerivedType(typeof(NewClientRequest), "new")]
    public abstract class CaseClientRequest
    {
        // Base class remains intentionally empty for polymorphic dispatch
    }

    /// <summary>
    /// Represents a client already registered in the system.
    /// </summary>
    public class ExistingClientRequest : CaseClientRequest
    {
        public Guid ClientId { get; set; }
    }

    /// <summary>
    /// Represents a new client to be created as part of the case.
    /// </summary>
    public class NewClientRequest : CaseClientRequest
    {
        public CreateClientRequest Client { get; set; } = new();
    }
}
