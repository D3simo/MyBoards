using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using MyBoards.Dto;
using MyBoards.Entities;
using MyBoards.Migrations;
using System.Linq.Expressions;
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
    option => option
        // Enable Lazy Loading - requires virtual navigation properties
        //.UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration.GetConnectionString("MyBoardsConnectionString"))
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

app.MapGet("data", async (MyBoardsContext db) =>
{
    //retrieve top authors from sql view
    var topAuthors = await db.ViewTopAuthors.ToListAsync();
    return topAuthors;
});

app.MapGet("dataTags", async (MyBoardsContext db) =>
{
    var tags = await db.Tags
    .AsNoTracking()
    .ToListAsync();
    return tags;
});

app.MapGet("filterData", async (MyBoardsContext db) =>
{
    var filterAdresses = await db.Addresses.Where(a => a.Coordinate.Latitude > 10).ToListAsync();
    return filterAdresses;
});

app.MapGet("dataLazyLoading", async (MyBoardsContext db) =>
{
    // Variable used for LazyLoading
    var withAddress = true;

    var user = await db.Users
        .FirstAsync(wt => wt.Id == Guid.Parse("8ACE902E-A25C-4168-CBC1-08DA10AB0E61"));
    if (withAddress)
    {
        // Lazy Loading
        var result = new { FullName = user.FullName, Address = $"{user.Address.Street} {user.Address.City}" };
        return result;
    }
    return new { FullName = user.FullName, Address = "" };
});

app.MapGet("pagination", (MyBoardsContext db) =>
{
    var filter = "a";
    string sortBy = "FullName";
    bool sortByDescending = false;
    int pageNumber = 1;
    int pageSize = 10;

    // Filtering
    var query = db.Users
        .Where(u => filter == null ||
            (u.Email.ToLower().Contains(filter.ToLower()) || u.FullName.ToLower().Contains(filter.ToLower())));

    var queryCount = query.Count();


    // Sorting
    if (sortBy != null)
    {
        var columnsSelector = new Dictionary<string, Expression<Func<User, object>>>
        {
            { nameof(User.FullName), u => u.FullName },
            { nameof(User.Email), u => u.Email },
        };

        var sortByExpression = columnsSelector[sortBy];

        query = sortByDescending 
            ? query.OrderByDescending(sortByExpression) 
            : query.OrderBy(sortByExpression);
    }

    // Pagination
    var result = query.Skip(pageSize * (pageNumber - 1))
        .Take(pageSize)
        .ToList();

    var pagedResult = new PagedResult<User>(result, queryCount, pageSize, pageNumber);
    return pagedResult;
});

app.MapGet("dataSelect", async (MyBoardsContext db) =>
{
    var comments = await db.Users
        .Include(u => u.Address)
        .Include(u => u.Comments)
        .Where(u => u.Address.Country == "Albania")
        .SelectMany(u => u.Comments)
        .Select(c => c.Message)
        .ToListAsync();

    return comments;
});

app.MapGet("n+1 problem | LazyLoading enabled", async (MyBoardsContext db) =>
{
    var users = await db.Users
        .Include(u => u.Address)
        .Include(u => u.Comments)
        .Where(u => u.Address.Country == "Albania")
        .ToListAsync();

    foreach (var user in users)
    {
        foreach (var comment in user.Comments)
        {
            // Process(comment);
        }
    }
});

//Before.NET 7
//app.MapPut("updateLinq2Db", async (MyBoardsContext db) =>
//{
//    var users = db.Users
//        .Where(u => u.FullName == "John");

//    await LinqToDB.LinqExtensions.UpdateAsync(users.ToLinqToDB, x => new User
//    {
//        Comments = "New User"
//    });
//});

app.MapPut("updateBulk", async (MyBoardsContext db) =>
{
    var users = await db.Users
        .Where(u => u.FullName == "John")
        .ToListAsync();
    foreach (var user in users)
    {
        user.FullName = "Test User John";
    }

    await db.SaveChangesAsync();
});

app.MapPut("updateBulkRefactored", async (MyBoardsContext db) =>
{
    var users = db.Users
        .Where(u => u.FullName == "John");

    await users
        .ExecuteUpdateAsync(s =>
            s.SetProperty(u => u.FullName, "Test User John"));

    await db.SaveChangesAsync();
});

app.MapPut("updateBulk>>.NET7", async (MyBoardsContext db) =>
{
    await db.Users
        .Where(u => u.FullName.Contains("John"))
        .ExecuteUpdateAsync(s => 
            s.SetProperty(e => e.FullName, "John 7+"));
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

app.MapDelete("deleteWithChangeTracker", (MyBoardsContext db) =>
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