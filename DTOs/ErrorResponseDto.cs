namespace JobApplicationTracker.WebApi.DTOs
{
    public record ErrorResponseDto(
        string Message,
        string? Details,
        int StatusCode,
        DateTime Timestamp,
        Dictionary<string, string[]>? Errors
    );
}
