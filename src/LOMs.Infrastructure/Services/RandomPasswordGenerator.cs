using LOMs.Application.Common.Interfaces;

namespace LOMs.Infrastructure.Services;

public class RandomPasswordGenerator : IPasswordGenerator
{
    public string Generate()
    {
        return Guid.NewGuid().ToString("N")[..8]; // 8 chars random
    }
}