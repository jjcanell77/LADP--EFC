# Entity Framework Core Setup (controller-based) Steps
## What is EFC (Entity Framework Core)?
Most non-trivial web applications will need to reliably run operations on data, such as create, read, update, and delete (CRUD). They'll also need to persist any changes made by these operations between application restarts. Although there are various options for persisting data in .NET applications, Entity Framework (EF) Core is a user-friendly solution and a great fit for many .NET applications.
### Understanding EF Core
-	EF Core is a lightweight, extensible, open source, and cross-platform data access technology for .NET applications.
-	EF Core can serve as an ORM (object-relational mapper), which:
allow you to interact with the relational databases using .NET plain old Common Runtime Language (CLR) objects (POCOs) and Language Integrated Query (LINQ) syntax for objects.
eliminates the need for most of the data-access code that typically needs to be written.
-	EF Core supports a large number of popular databases, including SQLite, MySQL, PostgreSQL, Oracle, and Microsoft SQL Server.
-	Eliminates the need for most data-access code that is typically written in our web API systems
-	EFC uses LINQ methods to query data from the database using strongly types expressions
-	EFC is responsibilities range from opening the connection to the database, reading of data and mapping the data from SQL back to the .Net Entity
-	Objects (DbSet) which passes the DB context back to us

### [Fluent API in Entity Framework Core](https://www.entityframeworktutorial.net/efcore/fluent-api-in-entity-framework-core.aspx)
The Fluent API in Entity Framework Core (EF Core) is a way to configure the model classes and their relationships using a fluent syntax. This approach is an alternative to using data annotations and provides more control over the configuration.
- Model Configuration: Configures an EF model to database mappings. Configures the default Schema, DB functions, additional data annotation attributes and entities to be excluded from mapping.
- Entity Configuration: Configures entity to table and relationships mapping e.g. PrimaryKey, AlternateKey, Index, table name, one-to-one, one-to-many, many-to-many relationships etc.
- Property Configuration: Configures property to column mapping e.g. column name, default value, nullability, Foreignkey, data type, concurrency column etc.

### The context class
This application has only one entity class, but most applications will have multiple entity classes. The context class is responsible for querying and saving data to your entity classes, and for creating and managing the database connection.

### Perform CRUD operations with EF Core
After EF Core is configured, you can use it to perform CRUD operations on your entity classes. Then, you can develop against C# classes, delegating the database operations to the context class. Database providers in turn translate it to database-specific query language. An example is SQL for a relational database. Queries are always executed against the database, even if the entities returned in the result already exist in the context.

## Step 1: Starting From Scratch
For this tutorial we will be using a controller-based Fluent API. Here i plan to show you the steps I took to add the Food Resources to better explain the steps.
### Inital Set-up
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
##  Step 2 : Setting up entities/models 
The entity class will consist of the object that your storing. 

### Add a entity/model class
-	In Solution Explorer, right-click the project. Select Add > New Folder. Name the folder Models.
-	Right-click the Models folder and select Add > Class. Name the class FoodResource and select Add.
-	Should look something like:
```
public class FoodResource
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Area { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zipcode { get; set; }
    public string? Country { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? Description { get; set; }
    public ICollection<ResourceTags> ResourceTags { get; set; }
    public ICollection<BusinessHours> BusinessHours { get; set; }
}
```
- Repeat those steps to add the rest below:
```
public class ResourceTags
{
    public int TagId { get; set; }
    public Tag Tag { get; set; }
    public int FoodResourceId { get; set; }
    public FoodResource FoodResource { get; set; }
}
```
This table creates a many-to-many relationship between FoodResource and Tag by linking TagId and FoodResourceId.

```
public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<ResourceTags> ResourceTags { get; set; }
}
```
This relates to the Tag table through the ResourceTags table. In this case, "FoodResource" would be a record in the Tag table.

```
public class BusinessHours
{
    public int BusinessHourID { get; set; }
    public int FoodResourceID { get; set; }
    public FoodResource FoodResource { get; set; }
    public int DayId { get; set; }
    public Days Day { get; set; }
    public string OpenTime { get; set; }
    public string CloseTime { get; set; }
}
```
This relates to the BusinessHours table. Each entry in the businessHours array would be a record in this table.

```
public class Days
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<BusinessHours> BusinessHours { get; set; }
}
```
The Days table would hold the day names ("Monday", "Tuesday", etc.), and the BusinessHours table would reference the Days table using DayId.

##  Step 3 : Database Integration 
The database context is the main class that coordinates Entity Framework functionality for a data model. This will act as the “Fish-Hook” into the database from your API, imagine a line being cast from your API Application/System into a SQL database via your connection string value! This is called Data Persistence which is a fancy word for any changes done with CRUD operations to be reflected onto the database.
- In Solution Explorer, right-click the project.
- Select Add > New Folder > Name the folder Data.
-	Right-click the Data folder and select Add > Class. Name the class DataContext and click Add.
-	Enter the following code:
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
        public DbSet<FoodResource> FoodResources { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ResourceTags> ResourceTags { get; set; }
        public DbSet<BusinessHours> BusinessHours { get; set; }
        public DbSet<Days> Days { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        /*Leave this empty for now*/
        }
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
### Add Connection String
-	In Solution Explorer, open appsettings.json. 
-	Go ahead an add the connection string. 
-	Should look something like:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Enter Connection Strring Here"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

##  Step 4 : Setting up the Repository
This will have a similar functionality that the Services Folder/Files did in your Sabio Project. 
In Solution Explorer, right-click the project. Select Add > New Folder. Name the folder Repository.
-	Right-click the Repository folder and select Add > Interface. Name the Interface IRepositoryToDoItems and click Add.
-	Should look something like:

##  Step 5 : Set Up Controllers
As mentioned we are using a controler based API and a Web API controller is a class which can be created under the Controllers folder or any other folder under your project's root folder. It handles incoming HTTP requests and send response back to the caller. This can include multiple action methods whose names match with HTTP verbs like Get, Post, Put and Delete.
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
