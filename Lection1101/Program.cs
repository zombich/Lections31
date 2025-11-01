using Lection1101.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
Console.WriteLine("call web-api");


var client = new HttpClient();
string baseUrl = "https://api.escuelajs.co/api/v1/";
client.BaseAddress = new Uri(baseUrl);

var category = new Category { Name = "tesqqqt1OPA", Image = "https://i.imgur.com/BG8J0Fj.jpg" };

var jsonOptions = new JsonSerializerOptions() { 
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

var json = JsonSerializer.Serialize(category, jsonOptions);
var content = new StringContent(json, Encoding.UTF8, "application/json");

var response = await client.PostAsync("categories", content);
response.EnsureSuccessStatusCode();

// получение объекта из ответа
if (response.IsSuccessStatusCode)
{
    var responseJson = await response.Content.ReadAsStringAsync();
    var category2 = JsonSerializer.Deserialize<Category>(responseJson,jsonOptions);
}



//using HttpResponseMessage response = await TestGet(client);

Console.WriteLine();



//(HttpResponseMessage response1, HttpResponseMessage response2, HttpResponseMessage response3) = await Http(client);

static async Task<(HttpResponseMessage response1, HttpResponseMessage response2, HttpResponseMessage response3)> Http(HttpClient client)
{
    var categories = await client.GetFromJsonAsync<List<Category>>("categories/");
    Console.WriteLine();

    int id = 41;
    var category = await client.GetFromJsonAsync<Category>($"categories/{id}");

    category = new Category { Name = "tesqqqt1", Image = "https://i.imgur.com/BG8J0Fj.jpg" };
    using var response1 = await client.PostAsJsonAsync("categories", category);
    response1.EnsureSuccessStatusCode();

    var result = await response1.Content.ReadFromJsonAsync<Category>();


    category.Name = "newName";
    using var response2 = await client.PutAsJsonAsync($"categories/{category.Id}", category);
    response2.EnsureSuccessStatusCode();

    using var response3 = await client.DeleteAsync($"categories/{category.Id}");
    response3.EnsureSuccessStatusCode();
    return (response1, response2, response3);
}

static async Task<HttpResponseMessage> TestGet(HttpClient client)
{
    var response = await client.GetAsync("categories");
    response.EnsureSuccessStatusCode();

    var jsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // с маленькой буквы названия
        WriteIndented = true, // для отступов
    };

    var content = await response.Content.ReadAsStringAsync();
    var categories = JsonSerializer.Deserialize<List<Category>>(content, jsonOptions);
    return response;
}