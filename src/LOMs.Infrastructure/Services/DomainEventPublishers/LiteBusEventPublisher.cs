using LiteBus.Events.Abstractions;
using LOMs.Application.Common.Interfaces;

namespace LOMs.Infrastructure.Services.DomainEventPublishers;

public sealed class LiteBusEventPublisher(IEventMediator mediator) : IDomainEventPublisher
{
    private readonly IEventMediator _mediator = mediator;

    public async Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await _mediator.PublishAsync(domainEvent, cancellationToken: cancellationToken);
    }
}