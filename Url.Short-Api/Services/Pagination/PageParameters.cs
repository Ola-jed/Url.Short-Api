namespace Url.Short_Api.Services.Pagination;

public record PageParameters(int PerPage,int Page)
{
    public int Offset => (Page - 1) * PerPage;
}