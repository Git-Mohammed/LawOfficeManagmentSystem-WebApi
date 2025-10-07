using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Judgments.Dtos;
using LOMs.Domain.Cases.Judgments;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Judgments.Queries;

public class GetJudgmentByIdQueryHandler(IAppDbContext context, ILogger<GetJudgmentByIdQueryHandler> logger, IMapper mapper) : IQueryHandler<GetJudgmentByIdQuery, Result<JudgmentDto>>
{
    private readonly IAppDbContext _context = context;
    private readonly ILogger<GetJudgmentByIdQueryHandler> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<JudgmentDto>> HandleAsync(GetJudgmentByIdQuery query, CancellationToken cancellationToken = new CancellationToken())
    {
        var existingJudgment = await _context.Judgments.AsNoTracking().FirstOrDefaultAsync(j => j.Id == query.JudgmentId && j.CaseId == query.CaseId, cancellationToken);
        if (existingJudgment is null)
        {
            _logger.LogWarning("Judgment with Id {JudgmentId} not found.", query.JudgmentId);
            return JudgmentErrors.NotFound;
        }
        
        return _mapper.Map<Judgment,JudgmentDto>(existingJudgment);
    }
}