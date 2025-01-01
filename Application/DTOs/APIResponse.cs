namespace API.DTOs
{
    public record APIResponse<T>(List<T> data, PaginationResponse pagination);
}
