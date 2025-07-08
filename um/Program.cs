
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// In-memory list of users
var users = new List<User>
{
    new User { UserName = "Alice", UserAge = 30 },
    new User { UserName = "Bob", UserAge = 25 }
};

// Create user
app.MapPost("/users", (User newUser) =>
{
    users.Add(newUser);
    return Results.Created($"/users/{newUser.UserName}", newUser);
});

// Read all users
app.MapGet("/users", () => users);

// Read one user by username
app.MapGet("/users/{username}", (string username) =>
{
    var user = users.FirstOrDefault(u => u.UserName == username);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

// Update user by username
app.MapPut("/users/{username}", (string username, User updatedUser) =>
{
    var index = users.FindIndex(u => u.UserName == username);
    if (index == -1)
        return Results.NotFound();

    users[index] = updatedUser;
    return Results.Ok(updatedUser);
});

// Delete user by username
app.MapDelete("/users/{username}", (string username) =>
{
    var user = users.FirstOrDefault(u => u.UserName == username);
    if (user is null)
        return Results.NotFound();

    users.Remove(user);
    return Results.Ok(user);
});

app.Run();


public class User
{
    public required string UserName { get; set; }
    public int UserAge { get; set; }
}