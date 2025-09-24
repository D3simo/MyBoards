using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using MyBoards.Entities;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// To prevent cyclical references during JSON serialization
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

// only using minimal APIs without controllers:
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Registers MyBoardsContext to use SQL Server with the connection string
builder.Services.AddDbContext<MyBoardsContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyBoardsConnectionString"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

void ApplyPendingMigrations(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<MyBoardsContext>();
    var pendingMigrations = dbContext.Database.GetPendingMigrations();
    if (pendingMigrations.Any())
    {
        dbContext.Database.Migrate();
    }

    //List of Users
    var users = dbContext.Users.ToList();
    if (!users.Any())
    {
        var user1 = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "John Doe",
            Email = "johndoe@gmail.com",
            Address = new Address()
            {
                Id = Guid.NewGuid(),
                Street = "123 Main St",
                City = "Anytown",
            }
        };

        var user2 = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "John Doe 2",
            Email = "johndoe2@gmail.com",
            Address = new Address()
            {
                Id = Guid.NewGuid(),
                Street = "456 Main St",
                City = "Anytown",
            }
        };

        dbContext.Users.AddRange(user1, user2);
        dbContext.SaveChanges();
    }

    // List of Tags
    var tags = dbContext.Tags.ToList();
    if (!tags.Any())
    {
        var tag1 = new Tag()
        {
            Id = 1,
            Value = "Web",
            Category = "IT"
        };
        var tag2 = new Tag()
        {
            Id = 2,
            Value = "UI",
            Category = "IT"
        };
        var tag3 = new Tag()
        {
            Id = 3,
            Value = "Desktop",
            Category = "IT"
        };
        var tag4 = new Tag()
        {
            Id = 4,
            Value = "API",
            Category = "IT"
        };
        var tag5 = new Tag()
        {
            Id = 5,
            Value = "Service",
            Category = "IT"
        };
        dbContext.Tags.AddRange(tag1, tag2, tag3, tag4, tag5);
        dbContext.SaveChanges();
    }
}
// Call the function before app.Run()
ApplyPendingMigrations(app.Services);

app.MapGet("data", (MyBoardsContext db) =>
{
    var topAuthors = db.ViewTopAuthors.ToList();
    return topAuthors;
});

app.MapGet("dataTags", (MyBoardsContext db) =>
{
    var tags = db.Tags
    .AsNoTracking()
    .ToList();
    return tags;
});

app.MapPost("update", async (MyBoardsContext db) =>
{ 
    var epic = await db.Epics.FirstAsync(e => e.Id == 1);

    epic.Area = "Updated Area";
    epic.Priority = 1;
    epic.StartDate = DateTime.Now;

    await db.SaveChangesAsync();

    return epic;
});

app.MapPost("create", async (MyBoardsContext db) =>
{
    var address = new Address()
    {
        Id = Guid.NewGuid(),
        Street = "789 Main St",
        City = "Anytown",
        Country = "USA",
    };

    var user = new User()
    {
        FullName = "Jane Smith",
        Email = "test@gmail.com",
        Address = address,
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return user;
});

app.MapDelete("deleteCascade", async (MyBoardsContext db) =>
{
    var workItemTag = await db.WorkItemTag
    .Where(wt => wt.WorkItemId == 12)
    .ToListAsync();
    db.WorkItemTag.RemoveRange(workItemTag);

    var workItem = await db.WorkItems
    .FirstAsync(w => w.Id == 16);
    //Remove range if multiple
    db.RemoveRange(workItem);

    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapDelete("deleteCommnentsCascade", async (MyBoardsContext db) =>
{
    var author = await db.Users
    .Include(u => u.Comments)
    .FirstAsync(wt => wt.Id == Guid.Parse("8ACE902E-A25C-4168-CBC1-08DA10AB0E61"));

    ////DeleteBehavior related Comments
    //var comments = db.Comments
    //.Where(c => c.AuthorId == author.Id).ToList();
    //db.Comments.RemoveRange(comments);

    db.Users.Remove(author);

    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapDelete("deleteWithChangeTracker", async (MyBoardsContext db) =>
{
    var workItem = new Epic
    {
        Id = 12
    };

    var entry = db.Attach(workItem);
    entry.State = EntityState.Deleted;

   db.SaveChanges();
    return Results.Ok();
});

app.Run();