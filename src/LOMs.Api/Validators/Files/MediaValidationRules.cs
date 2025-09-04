﻿namespace LOMs.Api.Validators.Files;

public static class MediaValidationRules
{
    public const long MaxImageSize = 5 * 1024 * 1024;
    public const long MaxVideoSize = 50 * 1024 * 1024;
    public const long MaxAudioSize = 10 * 1024 * 1024;

    public static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
    public static readonly string[] AllowedVideoExtensions = { ".mp4", ".mov", ".webm", ".mkv", ".avi" };
    public static readonly string[] AllowedAudioExtensions = { ".mp3", ".wav", ".ogg", ".aac" };
}
