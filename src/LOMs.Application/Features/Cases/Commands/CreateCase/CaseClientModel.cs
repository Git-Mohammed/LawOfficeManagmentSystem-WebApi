using LOMs.Application.Features.People.Clients.Commands.CreateClient;



namespace LOMs.Application.Features.Cases.Commands.CreateCase
    {
        /// <summary>
        /// Base model for representing a client in a case — either existing or new.
        /// </summary>
        public abstract record CaseClientModel;

        /// <summary>
        /// Represents a reference to an existing client.
        /// </summary>
        public record ExistingCaseClientModel : CaseClientModel
        {
            public Guid ExistingClientId { get; init; }
        }

        /// <summary>
        /// Represents a new client to be created as part of the case.
        /// </summary>
        public record NewCaseClientModel : CaseClientModel
        {
            public CreateClientCommand Client { get; init; }

            public NewCaseClientModel(CreateClientCommand client)
            {
                Client = client;
            }
        }
}

