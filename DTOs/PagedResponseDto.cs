namespace JobApplicationTracker.WebApi.DTOs
{
    public record PagedResponseDto<T>(
        IEnumerable<T> Data,
        int PageNumber,
        int PageSize,
        int TotalCount,
        int TotalPages
    );
}
