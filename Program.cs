using Firebase.Database;
using Firebase.Database.Query;

var builder = WebApplication.CreateBuilder(args);

var firebaseClient = new FirebaseClient("https://lvr-portofolio-5a82e-default-rtdb.firebaseio.com");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();
app.UseCors();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/createuser", async (User user) =>
{
    try
    {
        if (user.Name.Length > 50)
        {
            return Results.BadRequest("Error creating user: Name exceeds 50 characters.");
        }

        if (!IsValidEmail(user.Mail))
        {
            return Results.BadRequest("Error creating user: Invalid email format.");
        }

        if (!int.TryParse(user.Age, out _))
        {
            return Results.BadRequest("Error creating user: Age must be a valid number.");
        }

        var usersReference = firebaseClient.Child("users");

        var response = await usersReference.PostAsync(new
        {
            Name = user.Name,
            Age = user.Age,
            Mail = user.Mail
        });

        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"Error creating user: {ex.Message}");
    }
})
.WithName("CreateUser")
.WithOpenApi();

app.MapGet("/allusers", async () =>
{
    try
    {
        var usersReference = firebaseClient.Child("users");

        var response = await usersReference.OrderByKey().OnceAsync<User>();

        return Results.Ok(response.Select(u => u.Object));
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"Error retrieving users: {ex.Message}");
    }
})
.WithName("AllUsers")
.WithOpenApi();

app.Run();


bool IsValidEmail(string email)
{
    try
    {
        var mailAddress = new System.Net.Mail.MailAddress(email);
        return mailAddress.Address == email;
    }
    catch
    {
        return false;
    }
}

