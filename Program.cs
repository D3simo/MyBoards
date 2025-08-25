using Microsoft.EntityFrameworkCore;
using MyBoards.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
}

// Call the function before app.Run()
ApplyPendingMigrations(app.Services);
app.Run();