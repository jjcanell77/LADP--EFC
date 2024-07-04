# Entity Framework Core Setup (controller-based) Steps
### The entity class
The entity class will consist of the object that your storing

### The context class
This application has only one entity class, but most applications will have multiple entity classes. The context class is responsible for querying and saving data to your entity classes, and for creating and managing the database connection.

### Perform CRUD operations with EF Core
After EF Core is configured, you can use it to perform CRUD operations on your entity classes. Then, you can develop against C# classes, delegating the database operations to the context class. Database providers in turn translate it to database-specific query language. An example is SQL for a relational database. Queries are always executed against the database, even if the entities returned in the result already exist in the context.

## Step 1: Starting From Scratch
Make sure you have the latest installed:
-	[.NET SDK](https://dotnet.microsoft.com/en-us/download)
-	[Visual Studio](https://visualstudio.microsoft.com/downloads/)
-	[Git](https://git-scm.com/downloads)
  
### Create Project
-	Open Visual Studio (This will give you a base template for an API system)
-	Select Create new project on bottom right side
-	For ‘All Languages’ dropdown select C#
-	For ‘Project Types’ dropdown select Web API
-	Should be two options available. 
-	Select ‘ASP.NET Core API’

### Installing NuGet Packages
-	Right click the Solution you just created in the Solution Exlplorer
-	Then, select ‘Manage NuGet Packages for Solution…’
-	Should see ‘Browse’, ‘Installed’, ‘Updates’, and ‘Consolidate’
-	Make sure ‘Browse’ in this window is selected
-	In the search bar search and install the following:
 ```
Microsoft.EntityFrameworkCore
```
```
Microsoft.EntityFrameworkCore.SqlServer
```
```
Microsoft.EntityFrameworkCore.Tools
```
### Add a model class
-	In Solution Explorer, right-click the project. Select Add > New Folder. Name the folder Models.
-	Right-click the Models folder and select Add > Class. Name the class TodoItem and select Add.
-	Should look something like:
```
public partial class TodoItem
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool IsComplete { get; set; }
}
```

## Set Up Controllers
Web API controller is a class which can be created under the Controllers folder or any other folder under your project's root folder.It handles incoming HTTP requests and send response back to the caller. This can include multiple action methods whose names match with HTTP verbs like Get, Post, Put and Delete.
### Scaffold a controller (Optional)
One way to do this is to take advatage of Scaffolding, or you can build it from scratch. Scaffolding uses ASP.Net's templates to create a basic API Controller. This is just o get you started as you will need to make updates which is why it is optional. 
This template will Mark the class with the [ApiController] attribute, that indicates that the controller responds to web API requests. It also uses DI to inject the database context (TodoContext) into the controller. The database context is used in each of the CRUD methods in the controller.
-	Right-click the Controllers folder.
-	Select Add > New Scaffolded Item.
-	Select API Controller with actions, using Entity Framework, and then select Add.
-	Select TodoItem (TodoApi.Models) in the Model class.
-	Select TodoContext (TodoApi.Models) in the Data context class.
-	Select Add.
-	Should look something like:
```
namespace LADP__EFC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
          return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

```

##  Step 2 : Database Integration 
The database context is the main class that coordinates Entity Framework functionality for a data model. This will act as the “Fish-Hook” into the database from your API, imagine a line being cast from your API Application/System into a SQL database via your connection string value! This is called Data Persistence which is a fancy word for any changes done with CRUD operations to be reflected onto the database.
- In Solution Explorer, right-click the project. Select Add > New Folder. Name the folder Data.
-	Right-click the Data folder and select Add > Class. Name the class DataContext and click Add.
-	Should look something like:
```
using LADP__EFC.Models;
using Microsoft.EntityFrameworkCore;

namespace LADP__EFC.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<TodoItem> TodoItems { get; set; } = null!;
  }
```
### Register the database context
In ASP.NET Core, services such as the DB context must be registered with the dependency injection (DI) container. The container provides the service to controllers.
- Update Program.cs with something like:
```
using Microsoft.EntityFrameworkCore;
using LADP__EFC.Data;

namespace LADP__EFC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
```
