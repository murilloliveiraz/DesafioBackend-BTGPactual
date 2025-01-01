namespace API.DTOs
{
    public record PaginationResponse(int page, int pageSize, int totalElements, int totalPages)
    {
    }
}
