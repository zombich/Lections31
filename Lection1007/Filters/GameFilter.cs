namespace Lection1007.Filters
{
    public class GameFilter
    {
        public string? Name { get; set; }
        public string? Category { get; set; } = "arcada";
        public decimal? Price { get; set; }
    }
}

public class Paginator
{
    public int PageSize { get; set; } = 5;
    public int Page { get; set; } = 1;


}
