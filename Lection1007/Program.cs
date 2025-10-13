using Lection1007.Contexts;
using Lection1007.Filters;
using Lection1007.Models;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Применение ORM");

var optionsBuilder = new DbContextOptionsBuilder<StoreDbContext>();
optionsBuilder.UseSqlServer("Data Source=mssql;Initial Catalog=ispp3104;Persist Security Info=True;User ID=ispp3104;Password=3104;Encrypt=True;Trust Server Certificate=True");
using var context = new StoreDbContext(optionsBuilder.Options);


var isAllGamesDeleted = context.Games.All(g => g.IsDeleted);

//join(context);

//GroupBy(context);

Console.WriteLine();

//IQueryable<string> titles = SelectDto(context);

//IOrderedQueryable<Game> games = Sort(context);


//IQueryable<Game> games = FilterBy(context);

//Console.WriteLine(titles.ToQueryString());


//var categoryService = new CategoryService(context);
//var categories = await categoryService.GetCategoriesAsync();
//foreach (var category in categories)
//    Console.WriteLine(category.Name);


//var games = context.Games;
//Console.WriteLine(games.ToQueryString());

//Include(context);

//Filter(context);

//PAgination(context);





//await AddCategoryAsync(context);

//RemoveCategory(context);

//update
//1
//UpdateCategoryFromDb(context);

//2
//UpdateCategoryFromApp(context);

//context.Games.Where(g=> g.GameId > 4).ExecuteDelete();

//context.Games.Where(g => g.CategoryId == 1)
//    .ExecuteUpdate(setters => setters
//        .SetProperty(g => g.IsDeleted, g => false)
//        .SetProperty(g => g.KeysAmount, g => g.KeysAmount > 100 ? 120 : 75));

//GetList(context);
//await GetValueAsync(context);

static async Task GetValueAsync(AppDbContext context)
{
    var game = context.Games.Find(1);
    game = await context.Games.FindAsync(1);
    game = context.Games.FirstOrDefault(g => g.GameId > 2);
    game = await context.Games.FirstOrDefaultAsync(g => g.GameId > 2);

    game = context.Games.SingleOrDefault(g => g.GameId > 2);
    game = await context.Games.SingleOrDefaultAsync(g => g.GameId > 2);
}

static void GetList(AppDbContext context)
{
    var categories = context.Categories.Include(c => c.Games);
    foreach (var category in categories)
        Console.WriteLine($"{category.CategoryId} - {category.Name} - {category.Games?.Count()}");

    var games = context.Games.Include(g => g.Category);
    foreach (var game1 in games)
        Console.WriteLine($"{game1.GameId} - {game1.Name} - {game1.Category?.Name}");
}

static async Task AddCategoryAsync(AppDbContext context)
{
    //insert
    var category = new Category()
    {
        Name = "casual"
    };

    //context.Categories.Add(category);
    await context.Categories.AddAsync(category);

    //context.SaveChanges();
    await context.SaveChangesAsync();
}

static void RemoveCategory(AppDbContext context)
{
    var category = context.Categories.Find(6);
    if (category is not null)
    {
        context.Categories.RemoveRange(category);
        context.SaveChanges();
    }
}

static void UpdateCategoryFromDb(AppDbContext context)
{
    var category = context.Categories.Find(1);
    if (category is null)
        throw new ArgumentException("категория не найдена");
    category.Name = "Симулятор2";
    context.SaveChanges();
}

static void UpdateCategoryFromApp(AppDbContext context)
{
    var category = new Category()
    {
        CategoryId = 1,
        Name = "бравлик",
    };
    context.Categories.Update(category);
    context.SaveChanges();
}

static void Include(StoreDbContext context)
{
    var result = context.Games
        .Include(g => g.Category);
    Console.WriteLine(result.ToQueryString());
    foreach (var game in result)
        Console.WriteLine($"{game.Name} - {game.Category?.Name}");

    var categories = context.Categories
        .Include(c => c.Games);
    foreach (var x in categories)
        Console.WriteLine($"{x.Name} - {x.Games?.Count()}");
}

static void PAgination(StoreDbContext context)
{
    int pageSize = 3;
    int currentPage = 1;
    var games = context.Games
        .Skip(pageSize * currentPage)
        .Take(pageSize);


    foreach (var game in games)
        Console.WriteLine(game.Name);
}

static void Filter(StoreDbContext context)
{
    var games = context.Games.AsQueryable();
    if (true)
        games = games.Where(g => g.Price < 500);
    if (true)
        games = games.Where(g => g.Name.Contains("a"));
    if (true)
        games = games.Where(g => !String.IsNullOrWhiteSpace(g.Description));

    Console.WriteLine(games.ToQueryString());
}

static IQueryable<Game> FilterBy(StoreDbContext context)
{
    GameFilter filter = new()
    {
        Price = 500,
        Category = "аркада"
    };

    var games = context.Games.AsQueryable();
    if (filter.Price is not null)
        games = games.Where(g => g.Price < filter.Price);
    if (filter.Name is not null)
        games = games.Where(g => g.Name == filter.Name);
    if (filter.Category is not null)
        games = games.Where(g => g.Category.Name == filter.Category);
    return games;
}

static IOrderedQueryable<Game> Sort(StoreDbContext context)
{
    var games = context.Games.OrderByDescending(g => g.Price);
    games = context.Games.OrderByDescending(g => EF.Property<object>(g, "Name"));
    foreach (var g in games)
        Console.WriteLine($"{g.Name} - {g.Price}");
    return games;
}

static IQueryable<string> SelectDto(StoreDbContext context)
{
    var titles = context.Games.Select(games => games.Name);

    foreach (var t in titles)
        Console.WriteLine(t);

    var games = context.Games
        .Select(g => g.ToDto());
    foreach (var game in games)
        Console.WriteLine($"{game.Title} - {game.Price} - {game.Category}");
    return titles;
}

static void GroupBy(StoreDbContext context)
{
    var categories = context.Games
        .GroupBy(g => g.Category!.Name)
        .Select(group => new
        {
            group.Key,
            GamesCount = group.Count()

        });

    var categories2 = context.Games
        .GroupBy(g => new { g.Category!.Name, g.IsDeleted })
        .Select(group => new
        {
            CategoryName = group.Key.Name,
            group.Key.IsDeleted,
            GamesCount = group.Count(g => g.IsDeleted)

        });
}

static void join(StoreDbContext context)
{
    var games = context.Games
        .Join(context.Categories,
        g => g.CategoryId,
        c => c.CategoryId,
        (g, c) => new
        {
            g.Name,
            CategoryName = c.Name
        });
}