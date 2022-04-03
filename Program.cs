using LightWeightDotNetService.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PostDbContext>(c =>
      c.UseNpgsql(builder.Configuration.GetConnectionString("sqlServerConnection")));

//builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("myClient", client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
});

// Add services to the container.

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.MapGet("/weatherforecast", async(IHttpClientFactory clientFactory, PostDbContext context) =>
{
    //var context = app.Services.GetService<PostDbContext>();
    var httpClient = clientFactory.CreateClient("myClient");
    var result = await httpClient.GetFromJsonAsync<List<Post>>("/posts");
    var postItem = new Post();
    foreach (var post in result)
    {
        postItem.UserId = post.UserId;
        postItem.Title = post.Title;
        postItem.Body = post.Body;
        context.Posts.Add(postItem);
    }
    await context.SaveChangesAsync();

    return result;
});

app.Run();

