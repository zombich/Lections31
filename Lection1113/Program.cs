using Lection1113;

Console.WriteLine("jwt");

var authService = new AuthService();
var accessToken = authService.GenerateToken(123,"user1");
Console.WriteLine(accessToken);
