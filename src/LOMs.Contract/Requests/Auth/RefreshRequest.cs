﻿namespace LOMs.Contract.Requests.Auth;

public class RefreshRequest
{
    public string RefreshToken { get; set; } = null!;
}