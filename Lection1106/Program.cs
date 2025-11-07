using Lection1106;
using System.Security.Cryptography;
using System.Text;

Console.WriteLine("password");

static void LockUser()
{
    var login = "admin";
    var password = "123";
    using var context = new AppDbContext();
    var user = context.Users.FirstOrDefault(u => u.Login == login);

    if (user is null)
    {
        Console.WriteLine("not found");
        return;
    }
    // проверка, что пользователь заблокирован
    if (IsUserLocked(user))
    {
        Console.WriteLine($"locked until {user.LockedUntil:HH:mm:ss}");
        return;
    }

    // проверка, что попытка аутентификации неуспешна
    if (!IsCorrectPassword(password, user))
    {
        Console.WriteLine("Incorrect pass");
        context.SaveChanges();
        return;
    }

    SuccessLogin(user);
    context.SaveChanges();

    Console.WriteLine("welcome");
    return;
}

//static async Task InsertData()
//{
//    var users = new List<User>()
//{
//    new() { Login = "admin", Password = "qwerty"},
//    new() { Login = "manager", Password = "123"},
//    new() { Login = "customer", Password = "1"},
//};
//    using var context = new AppDbContext();
//    context.Users.AddRange(users);
//    await context.SaveChangesAsync();
//} нада добавить migration 

static void ComputeHash()
{
    var salt = "123456";
    var password = "qwerty" + salt;
    byte[] bytes = Encoding.UTF8.GetBytes(password);

    //MD5 algo = MD5.Create();
    SHA384 algo = SHA384.Create();

    var hashBytes = algo.ComputeHash(bytes);
    var hash = Convert.ToBase64String(hashBytes); // base64
    hash = Convert.ToHexString(hashBytes);        // hex: 0-9A-F
}

static bool IsUserLocked(User user)
{
    if (user.LockedUntil.HasValue && user.LockedUntil <= DateTime.UtcNow)
    {
        user.FailedLoginAttempts = 0;
        user.LockedUntil = null;
        return false;
    }

    return user.LockedUntil.HasValue;
}

static bool IsCorrectPassword(string password, User user)
{
    int attempts = 3;
    int timeout = 30;
    if (user.Password != password)
    {
        user.FailedLoginAttempts++;
        if (user.FailedLoginAttempts >= attempts)
            user.LockedUntil = DateTime.UtcNow.AddSeconds(timeout);
        return false;
    }

    return true;
}

static void SuccessLogin(User user)
{
    user.FailedLoginAttempts = 0;
    //user.LockedUntil = null;
    user.LastAccess = DateTime.UtcNow;
}

//static void ComputeBcryptHash()
//{
//    var password = "qwerty";
//    var hash = BCrypt.Net.Bcrypt.EnchancedHashPassword(password);

//    var input = "12345";
//    var isCorrect = BCrypt.Net.Bcrypt.EnchancedVerify(input, hash);
//} не работаит Bcrypt