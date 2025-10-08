using Lection1007.Contexts;
using Lection1007.Models;
using Lection1007.Services;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Применение ORM");

var optionsBuilder = new DbContextOptionsBuilder<StoreDbContext>();
optionsBuilder.UseSqlServer("Data Source=mssql;Initial Catalog=ispp3104;Persist Security Info=True;User ID=ispp3104;Password=3104;Encrypt=True;Trust Server Certificate=True");
using var context = new StoreDbContext(optionsBuilder.Options);


var categoryService = new CategoryService(context);
var categories = await categoryService.GetCategoriesAsync();
foreach (var category in categories)
    Console.WriteLine(category.Name);





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