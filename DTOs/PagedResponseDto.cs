namespace JobApplicationTracker.WebApi.DTOs
{
    /// <summary>
    /// Generic paginated response wrapper
    /// </summary>
    /// <typeparam name="T">The type of items in the data collection</typeparam>
    /// <param name="Data">The paginated collection of items</param>
    /// <param name="PageNumber">Current page number (1-based)</param>
    /// <param name="PageSize">Number of items per page</param>
    /// <param name="TotalCount">Total number of items across all pages</param>
    /// <param name="TotalPages">Total number of pages</param>
    public record PagedResponseDto<T>(
        IEnumerable<T> Data,
        int PageNumber,
        int PageSize,
        int TotalCount,
        int TotalPages
    );
}
