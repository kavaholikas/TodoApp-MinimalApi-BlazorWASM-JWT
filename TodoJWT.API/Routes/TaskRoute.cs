using TodoJWT.Models;

namespace TodoJWT.API.Routes
{
    public static class TaskRoute
    {
        public static void MapTaskRoute(this IEndpointRouteBuilder routeBuilder)
        {
            var app = routeBuilder.MapGroup("/tasks").RequireAuthorization("user");

            app.MapGet("/", (HttpContext context, IDatabaseService db) =>
            {
                string username = context.User.Identity?.Name ?? string.Empty;

                User? user = db.GetUser(username);

                if (user == null)
                {
                    return Results.BadRequest($"Failed to get User: {username}");
                }

                List<TodoTask> tasks = db.GetUserTasks(user);
                List<TodoTaskDto> tasksDto = tasks.Select(t => t.CreateDto()).ToList();

                return Results.Ok<List<TodoTaskDto>>(tasksDto);
            });

            app.MapPost("/create", (
                TodoTaskDto todoTask,
                IDatabaseService db,
                HttpContext context) =>
            {
                if (context.User.Identity == null || context.User.Identity.Name == null)
                {
                    return Results.NotFound("User Not found");
                }

                User? user = db.GetUser(context.User.Identity.Name);
                if (user == null)
                {
                    return Results.NotFound("User Not found");
                }

                TodoTask task = new TodoTask().CreateFromDto(todoTask);

                db.CreateUserTask(user, task);

                return Results.Ok();
            });

            app.MapPatch("/complete/{id}", (int id, IDatabaseService db) =>
            {
                bool result = db.CompleteTask(id);

                if (result)
                {
                    return Results.Ok();
                }
                else
                {
                    return Results.NotFound();
                }
            });

            app.MapDelete("/delete/{id}", (int id, IDatabaseService db) =>
            {
                bool result = db.RemoveTask(id);

                if (result)
                {
                    return Results.Ok();
                }
                else
                {
                    return Results.NotFound();
                }
            });

            app.MapGet("/archive/", (IDatabaseService db, HttpContext context) =>
            {
                User user = db.GetUser(context.User.Identity.Name);

                List<TodoTask> tasks = db.GetUserArchivedTasks(user);
                List<TodoTaskDto> tasksDto = tasks.Select(t => t.CreateDto()).ToList();

                return Results.Ok<List<TodoTaskDto>>(tasksDto);
            });


            app.MapPatch("/archive/{id}", (int id, IDatabaseService db) =>
            {
                bool result = db.ArchiveTask(id);

                if (result)
                {
                    return Results.Ok();
                }
                else
                {
                    return Results.NotFound();
                }
            });
        }
    }
}
