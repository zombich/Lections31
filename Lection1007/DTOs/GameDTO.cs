using Lection1007.Models;
using System.Linq.Expressions;

public class GameDTO
{
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal Tax { get; set; }
    public string? Category { get; set; }


}

public static class GameExtension
{
    public static GameDTO? ToDto(this Game game)
    {
        if (game is null)
            return null;
        return new GameDTO()
        {
            Title = game.Name,
            Price = game.Price,
            Tax = game.Price * 0.2m,
            Category = game.Category.Name ?? ""
        };
    }

    public static IEnumerable<GameDTO> ToDtos(this IEnumerable<Game> games)
        => games.Select(g => g.ToDto());

}

public static class GameExpression
{
    public static Expression<Func<Game, GameDTO>> ToDto
        => game => new GameDTO
        {
            Title = game.Name,
            Price = game.Price,
            Tax = game.Price * 0.2m,
            Category = game.Category.Name ?? ""
        };

}