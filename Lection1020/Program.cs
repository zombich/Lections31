using Lection1020.Contexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("выполнение sql-запросов средствами ORM");

using var context = new GamesDbContext();

//await FromSql(context);
//await SqlQuery(context);

decimal addingPrice = 0.5m;
int changedRows = context.Database.ExecuteSql($"update game set price -={addingPrice}");
Console.WriteLine(changedRows);

var games = context.Games.Where(g => EF.Functions.Like(g.Name, "[a-d]%"));


static async Task FromSql(GamesDbContext context)
{
    var games = context.Games.FromSql($"select * from game");
    Console.WriteLine(games.ToQueryString());
    await games.ForEachAsync(game => Console.WriteLine($"{game.Name} - {game.Price} - {game.CategoryId}"));

    int price = 1000;
    games = context.Games.FromSql($"select * from game where price < {price}");
    Console.WriteLine(games.ToQueryString());
    await games.ForEachAsync(game => Console.WriteLine($"{game.Name} - {game.Price} - {game.CategoryId}"));

    string columnName = "price";
    games = context.Games.FromSqlRaw($"select * from game order by {columnName}");
    Console.WriteLine(games.ToQueryString());
    await games.ForEachAsync(game => Console.WriteLine($"{game.Name} - {game.Price} - {game.CategoryId}"));


    string title = "SimCity";
    games = context.Games.FromSqlRaw($"select * from game where name = '{title}'");
    Console.WriteLine(games.ToQueryString());
    await games.ForEachAsync(game => Console.WriteLine($"{game.Name} - {game.Price} - {game.CategoryId}"));

    title = "SimCity";
    games = context.Games.FromSqlRaw($"select * from game where name = @p0", title);
    Console.WriteLine(games.ToQueryString());
    await games.ForEachAsync(game => Console.WriteLine($"{game.Name} - {game.Price} - {game.CategoryId}"));

    var sqlTitle = new SqlParameter("@title", "SimCity");
    games = context.Games.FromSqlRaw($"select * from game where name = @title", sqlTitle);
    Console.WriteLine(games.ToQueryString());
    await games.ForEachAsync(game => Console.WriteLine($"{game.Name} - {game.Price} - {game.CategoryId}"));
}

static async Task SqlQuery(GamesDbContext context)
{
    var titles = context.Database.SqlQuery<string>($"select name from game");
    Console.WriteLine(titles.ToQueryString());
    await titles.ForEachAsync(title => Console.WriteLine(title));

    var minPrice = context.Database.SqlQuery<decimal>($"select min(price) as value from game ").FirstOrDefault();
    Console.WriteLine(minPrice);
}