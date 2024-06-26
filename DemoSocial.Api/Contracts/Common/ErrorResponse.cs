﻿namespace DemoSocial.Api.Contracts.Common;

public record ErrorResponse
{
    public int StatusCode { get; set; }
    public string StatusPhrase { get; set; }
    public List<string> Errors { get; set; } = [];
    public DateTime Timestamp { get; set; }
}
