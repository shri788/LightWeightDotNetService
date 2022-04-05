using Hangfire;
using LightWeightDotNetService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PostDbContext>(c =>
      c.UseNpgsql(builder.Configuration.GetConnectionString("sqlServerConnection")));

//builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDataSyncRepo, DataSyncRepo>();

builder.Services.AddHttpClient("myClient", client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
});

// Add Hangfire services.
builder.Services.AddHangfire(x => 
        x.UseSqlServerStorage(builder.Configuration.GetConnectionString("hangFireServerConnection")));
builder.Services.AddHangfireServer();

// Add services to the container.

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.MapGet("/getFromApiEnd", ([FromServices] IDataSyncRepo PostRepo) =>
{ 
RecurringJob.AddOrUpdate("FirstJob", () =>
       PostRepo.DoDbSync(), Cron.Daily);
    /*);
    foreach (var post in context.Posts)
    {
        context.Remove(post);
    }
    await context.SaveChangesAsync();
    var httpClient = clientFactory.CreateClient("myClient");
    var result = await httpClient.GetFromJsonAsync<List<Post>>("/posts");
    
    foreach (var post in result)
    {
        var postItem = new Post();
        postItem.UserId = post.UserId;
        postItem.Title = post.Title;
        postItem.Body = post.Body;
        context.Posts.Add(postItem);
    }
    await context.SaveChangesAsync();

    return result;*/
});

app.Run();

public class DataSyncRepo: IDataSyncRepo
{
    private readonly IHttpClientFactory clientFactory;

    private readonly PostDbContext context;

    public DataSyncRepo(IHttpClientFactory clientFactory, PostDbContext context)
    {
        this.clientFactory = clientFactory;
        this.context = context;
    }

    public async Task<IEnumerable<Post>> DoDbSync()
    {
        foreach (var post in context.Posts)
        {
            context.Remove(post);
        }
        await context.SaveChangesAsync();
        var httpClient = clientFactory.CreateClient("myClient");
        var result = await httpClient.GetFromJsonAsync<List<Post>>("/posts");

        foreach (var post in result)
        {
            var postItem = new Post();
            postItem.UserId = post.UserId;
            postItem.Title = post.Title;
            postItem.Body = post.Body;
            context.Posts.Add(postItem);
        }
        await context.SaveChangesAsync();

        return result;
    }

}

public interface IDataSyncRepo
{
    Task<IEnumerable<Post>> DoDbSync();
}