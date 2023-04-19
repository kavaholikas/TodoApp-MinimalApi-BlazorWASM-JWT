using Microsoft.AspNetCore.Identity;
using TodoJWT.API.Services;
using TodoJWT.Models;

namespace TodoJWT.API.Routes
{
    public static class UserRoute
    {
        public static void MapUserRoute(this IEndpointRouteBuilder routeBuilder)
        {
            var app = routeBuilder.MapGroup("/user");

            app.MapPost("/login", (
                LoginRegisterDto loginModel,
                IDatabaseService db,
                IPasswordHasher<User> hasher,
                JwtGenerator jwt,
                IConfiguration configuration) =>
            {
                User? user = db.GetUser(loginModel.Username);

                if (user == null)
                {
                    return Results.NotFound("Bad Credentials");
                }

                PasswordVerificationResult result = hasher.VerifyHashedPassword(user, user.PasswordHash, loginModel.Password);

                if (result == PasswordVerificationResult.Failed)
                {
                    return Results.NotFound("Bad Credentials");
                }

                string token = jwt.GenerateJWT(user, configuration);

                return Results.Ok(token);
            });

            app.MapPost("/register", (
                LoginRegisterDto registerModel,
                IDatabaseService db,
                IPasswordHasher<User> hasher,
                JwtGenerator jwt,
                IConfiguration configuration) =>
            {
                if (db.UsernameTaken(registerModel.Username))
                {
                    return Results.BadRequest("Username Taken");
                }

                User user = new User();
                user.Username = registerModel.Username;
                user.PasswordHash = hasher.HashPassword(user, registerModel.Password);

                db.CreateUser(user);

                string token = jwt.GenerateJWT(user, configuration);

                return Results.Ok(token);
            });

            app.MapGet("/promote/{id}", (int id, IDatabaseService db) =>
            {
                bool result = db.PromoteUser(id);

                if (result)
                    return Results.Ok();
                else
                    return Results.BadRequest();
            }).RequireAuthorization("admin");

            app.MapGet("/users", (IDatabaseService db) =>
            {
                List<User> users = db.GetUsers();
                List<UserDto> usersDto = users.Select(u => u.CreateDto(u)).ToList();

                return Results.Ok<List<UserDto>>(usersDto);
            }).RequireAuthorization("admin");

            app.MapDelete("/delete/{id}", (int id, IDatabaseService db) =>
            {
                User? user = db.GetUser(id);
                if (user != null || !user.IsAdmin)
                {
                    db.RemoveUser(user);

                    return Results.Ok();
                }

                return Results.NotFound();
            }).RequireAuthorization("admin");

            app.MapPut("/change-password", (ChangePasswordDto changePasswordModel,
                IDatabaseService db,
                IPasswordHasher<User> hasher,
                JwtGenerator jwt,
                IConfiguration configuration) =>
            {
                User user = db.GetUser(changePasswordModel.UserID);

                PasswordVerificationResult passwordVerification = hasher.VerifyHashedPassword(user, user.PasswordHash, changePasswordModel.OldPassword);

                if (passwordVerification == PasswordVerificationResult.Failed)
                {
                    return Results.BadRequest("Incorect Password");
                }

                if (changePasswordModel.NewPassword != changePasswordModel.RepeatPassword)
                {
                    return Results.BadRequest("Passwords dont match");
                }

                user.PasswordHash = hasher.HashPassword(user, changePasswordModel.NewPassword);

                db.UpdateUser(user);

                string token = jwt.GenerateJWT(user, configuration);

                return Results.Ok(token);
            });
        }
    }
}
