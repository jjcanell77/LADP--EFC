# Entity Framework Core Setup (controller-based) Steps

## What is EFC (Entity Framework Core)?
Most non-trivial web applications will need to reliably run operations on data, such as create, read, update, and delete (CRUD). They'll also need to persist any changes made by these operations between application restarts. Although there are various options for persisting data in .NET applications, Entity Framework (EF) Core is a user-friendly solution and a great fit for many .NET applications.

### Understanding EF Core
-	EF Core is a lightweight, extensible, open source, and cross-platform data access technology for .NET applications.
-	EF Core can serve as an ORM (object-relational mapper), which:
allow you to interact with the relational databases using .NET plain old Common Runtime Language (CLR) objects (POCOs) and Language Integrated Query (LINQ) syntax for objects.
-	EF Core supports a large number of popular databases, including SQLite, MySQL, PostgreSQL, Oracle, and Microsoft SQL Server.
-	Eliminates the need for most data-access code that is typically written in our web API systems
-	EFC uses LINQ methods to query data from the database using strongly types expressions
-	EFC is responsibilities range from opening the connection to the database, reading of data and mapping the data from SQL back to the .Net Entity
-	Objects (DbSet) which passes the DB context back to us

### Perform CRUD operations with EF Core
After EF Core is configured, you can use it to perform CRUD operations on your entity classes. Then, you can develop against C# classes, delegating the database operations to the context class. Database providers in turn translate it to database-specific query language. An example is SQL for a relational database. Queries are always executed against the database, even if the entities returned in the result already exist in the context.

## Step 1: Starting From Scratch
For this tutorial we will be using a controller-based Fluent API. Here I plan to show you the steps I took to add the Food Resources to better explain the steps.
### Inital Set-up
Make sure you have the latest installed:
-	[.NET SDK](https://dotnet.microsoft.com/en-us/download)
-	[Visual Studio](https://visualstudio.microsoft.com/downloads/)
-	[Git](https://git-scm.com/downloads)
  
### Create Project
-	Open Visual Studio (This will give you a base template for an API system)
-	Select `Create new project` on bottom right side
-	For `All Languages` dropdown select `C#`
-	For `Project Types` dropdown select `Web API`
-	Should be two options available. 
-	Select `ASP.NET Core Web API`
![Create Project](https://github.com/user-attachments/assets/427ebd7e-69ac-49a3-aca3-5a488ab59e99)
- For solution name I will be using but you can name this whatever you like
```
LADP__EFC
```
### Installing NuGet Packages
-	Right click the Solution you just created in the Solution Exlplorer
-	Then, select `Manage NuGet Packages for Solution…`
-	Should see `Browse`, `Installed`, `Updates`, and `Consolidate`
-	Make sure `Browse` in this window is selected
-	In the search bar search and install the following:
 ``` { attributes go here }
Microsoft.EntityFrameworkCore
```
```
Microsoft.EntityFrameworkCore.Design
```
```
Microsoft.EntityFrameworkCore.SqlServer
```
```
Microsoft.EntityFrameworkCore.Tools
```
```
Microsoft.VisualStudio.Web.CodeGeneration.Design
```

### Remove Unnecessary built-in Classes
-	In your Solution explorer you should see your project along with some folders/classes
-	Inside the folder called `Controllers` folder you will see `WeatherForcastController.cs`.
-	You may delete this as you won’t be needing it. 
-	 Underneath the Program.cs you should see `WeatherForcast.cs`. 
-	You may delete this also.
-	 Do NOT delete the folder controller, as we are designing a [Controller-Based](https://www.c-sharpcorner.com/article/choosing-between-controllers-and-minimal-api-for-net-apis/) API.   

##  Step 2 : Setting up entities/models 
The entity class will consist of the object that your storing. This model is built using a set of [conventions](https://www.entityframeworktutorial.net/efcore/conventions-in-ef-core.aspx) that look for common patterns. When making your model you will see that some that show a One-to-Many, Many-to-Many, or even One-to-One relationship.

- Here is an example of the object we are looking to create in JS:
- There may of course be some changes but this gives us a general idea of what we are trying to achieve.
```
 {
         "name": "Agnes Memorial Church Of God In Christ",
         "area": "Oakland",
         "streetAddress": "2372 International Boulevard",
         "city": "Oakland",
         "state": "CA",
         "zipcode": 94601,
         "country": "United States",
         "latitude": 37.783047,
         "longitude": -122.234238,
         "tags": [
             "Food Pantry"
         ],
         "phone": "510-533-1101",
         "website": "http://agnesmemorialchurch.com",
         "description": "Partners with the City of Oakland's Hunger Relief Program. Serves as a distribution food pantry site for the Lower San Antonio District.",
         "businessHours": [
             {
                 "day": "Monday",
                 "open": null,
                 "close": "12:30"
             },
             {
                 "day": "Tuesday",
                 "open": "9:am",
                 "close": "12:30"
             },
             {
                 "day": "Wednesday",
                 "open": "9:am",
                 "close": "12:30"
             },
             {
                 "day": "Thursday",
                 "open": "9:am",
                 "close": "12:30"
             },
             {
                 "day": "Friday",
                 "open": "9:am",
                 "close": "12:30"
             },
             {
                 "day": "Saturday",
                 "open": null,
                 "close": null
             },
             {
                 "day": "Sunday",
                 "open": null,
                 "close": null
             }
         ]
     },
```

### Add entity/model class
First thing is first we need to create our model/entity that will create that object we saw above.
-	In Solution Explorer, right-click the `project`. Select `Add` > `New Folder`. Name the folder Models.
-	Right-click the `Models` folder and select `Add` > `Class`. Name the class `FoodResource` and select `Add`.
-	Should look something like:
```
public class FoodResource
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Area { get; set; }
    public string StreetAddress { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Zipcode { get; set; } = null!;
    public string? Country { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? Description { get; set; }
    public List<Tag> Tag { get; } = []; // Many-to-Many relationship with Tag through the join table ResourceTags
    public List<BusinessHours> BusinessHours { get; set; } = [];// One-to-Many relationship with BusinessHours
}
```
You may have notice the `= null!`, `string?`, and `= []`. Easiest way to think about them as required, not required, and can be an empty array. I also added some notes in regards to the relationships which will be touched on more as we go on. 

[Many-to-Many](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many) relationships are used when any number entities of one entity type is associated with any number of entities of the same or another entity type. For example, using the object FoodResource, it can have many associated Tags, and each Tag can in turn be associated with any number of FoodResource. In our particular situation this relationship is made between FoodResource and Tag by joining TagId and FoodResourceId using ResourceTags.
- it should look something like:
```
public class ResourceTags
{
    public int TagId { get; set; }
    public int FoodResourceId { get; set; }
}
```

```
 public class Tag
 {
     public int Id { get; set; }
     public string Name { get; set; } = null!;
 }
```

[One-to-Many](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many) relationships are used when a single entity is associated with any number of other entities. For example, a FoodResource can have many associated BusinessHours, but each BusinessHours is associated with only one FoodResource.
- Should look something like:
```
public class BusinessHours
{
    public int BusinessHourId { get; set; }
    public Day Day { get; set; } = null!; // Many-to-One relationship with Day
    public string? OpenTime { get; set; }
    public string? CloseTime { get; set; }
}
```

The same can be said when it comes to the relationship between Day and BusinessHours. This is because a Day can have many associated BusinessHours, but each BusinessHours is associated with only one Day.The Days table holds the day names ("Monday", "Tuesday", etc.), which can be used by many BusinessHours. It holds only one version of those days, and the BusinessHours table references the Days table.
- Should look something like:
```
public class Day
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
```
Now Depending on how we intend to configure these models we will be returning to the classes in [step 4](https://github.com/jjcanell77/LADP--EFC/blob/master/Instructions.md#step-4-configuring-a-model). 
<!--- will need to update link for step 4 -->
 
##  Step 3: Database Integration 
The database context is the main class that coordinates Entity Framework functionality for a data model. This class is responsible for querying and saving data to your entity classes, and for creating and managing the database connection. It consists of these sequence of events:
- A DbContext instance is created.
- Entities are tracked using this instance.
- Changes are made to the tracked entities.
- The SaveChanges method is invoked to store the entities in memory to the underlying database.
- The DbContext object is disposed or garbage-collected when it is no longer needed by the application.

It acts sort of like a “Fish-Hook” into the database from your API, imagine a line being cast from your API Application/System into a SQL database via your connection string value. This is called Data Persistence which is a fancy word for any changes done with CRUD operations to be reflected onto the database.
- In `Solution Explorer`, right-click the `project`.
- Select `Add` > `New Folder` > `Name` the folder `Data`.
-	Right-click the `Data` folder and select `Add` > `Class`. Name the class `DataContext` and click `Add`.
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
        public DbSet<Day> Day { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          /*Leave this empty for now*/
        }
  }
```

### Register the database context
There a couple of ways to accomplish this, but for this project we will be  registering the DbContext using `dependency injection (DI)` in our main program. This in turn will provide the services to our controllers using the `AddDbContext` method as shown in the example below. With DI, we are able to create a DbContext instance for each request and dispose of it when that request terminates.
- You should be adding this to your `Program.cs` to look something like:
```
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});
```

You can now take advantage of constructor injection in the controllers to retrieve an instance of DbContext which we will go over further in [step 6](https://github.com/jjcanell77/LADP--EFC/blob/master/Instructions.md#step-6--set-up-controllers). But here is an example that will be implemented later when creating the Controller.
<!---  will need to update link for step 6 -->
```
 public class FoodResourcesController : ControllerBase
    {
        private readonly DataContext _context;

        public FoodResourcesController(DataContext context)
        {
            _context = context;
        }
}
```


### Add Connection String
Inside our DbContext instance that we referenced the connection string which will give our application access to the database. 
-	In `Solution Explorer`, open `appsettings.json`. 
-	Go ahead an add the connection string. 
-	Should look something like:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Enter Connection String Here"
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

## Step 4: Configuring a Model 
The model can then be customized using mapping attributes (also known as data annotations) and/or calls to the ModelBuilder methods (also known as fluent API) in OnModelCreating, both of which will override the configuration performed by conventions.

#### Remember to Choose Either to Use Data Annotations or Fluent API
- Fluent API will overwrite the use of Data Annotations.

### [Data Annotations/Mapping Atrributes](https://www.learnentityframeworkcore.com/configuration/data-annotation-attributes) (Optional if not using prefered Fluent API)
Using atrributes are nice because they are applied directly to the domain model, so it is easy to see how the model is configured just by examining the class files. You may have seen using some attributes, such as Required and StringLength before but basicaly these annotations are used to provide UI-based validation based on the specified configuration you choose.
- One of the cons to using Data Annotations is that it may not cover every type of configuration, and could result in you having to implement some Fluent API as well. This will split configurations to more than one location adding unecessary complexity.
- EFC will thankfully most of the times set defaults for some information not given and is best seen when using data Migrations which is covered later in [step 5](https://github.com/jjcanell77/LADP--EFC/blob/master/Instructions.md#step-5--migrations).
<!--- will need to update link for step 5 -->

##### Table()
- Will map an entity to the table with the same name as the `DbSet<Entity>` by default or you can use `Table()` to specify the table name.
- Also the default schema is `dbo` but if you need it to be different this is where you would make the change `Table("TableName", Schema = "dbo")`

##### Key
- Is used to identify the entity's primary key.

##### Column()
- Can be used to specify column name, order, and type.
- The default of the name of the property will be used as the column name if not specified. A big deal if not the same.
- Type also plays a big role as EFC's default is usually the largest for that type which can cause many issues if not correctly specified.

##### MaxLength()
- As mentioned earlier the default chosen is usllay the largest possible choice for example with string would be nvarchar(Max) if not specified.

##### Required
- If you weren't sure is applied to an entity that is not nullable.

##### ForeignKey()
- Is used to specify which property is the foreign key in a relationship.

Here is some examples of how you would implment these:
- First make sure to add `using System.ComponentModel.DataAnnotations;`
- Here is an example of the FoodResource entitie:
```
[Table("FoodResource", Schema = "dbo")]
public class FoodResource
{
    [Key]
    [Column("Id", TypeName = "int")]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("Name", TypeName = "nvarchar(255)")]
    public string Name { get; set; }  = null!;

    [MaxLength(100)]
    [Column("Area", TypeName = "nvarchar(100)")]
    public string? Area { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("StreetAddress", TypeName = "nvarchar(255)")]
    public string StreetAddress { get; set; }  = null!;

    [Required]
    [MaxLength(100)]
    [Column("City", TypeName = "nvarchar(100)")]
    public string City { get; set; }  = null!;

    [Required]
    [MaxLength(2)]
    [Column("State", TypeName = "nvarchar(2)")]
    public string State { get; set; }  = null!;

    [Required]
    [MaxLength(10)]
    [Column("Zipcode", TypeName = "nvarchar(10)")]
    public string Zipcode { get; set; }  = null!;

    [MaxLength(50)]
    [Column("Country", TypeName = "nvarchar(50)")]
    public string? Country { get; set; }

    [Column("Latitude", TypeName = "decimal(9, 6)")]
    public decimal? Latitude { get; set; }

    [Column("Longitude", TypeName = "decimal(9, 6)")]
    public decimal? Longitude { get; set; }

    [MaxLength(20)]
    [Column("Phone", TypeName = "nvarchar(20)")]
    public string? Phone { get; set; }

    [MaxLength(255)]
    [Column("Website", TypeName = "nvarchar(255)")]
    public string? Website { get; set; }

    [Column("Description", TypeName = "nvarchar(MAX)")]
    public string? Description { get; set; }

    public List<ResourceTags> ResourceTags { get; set; } = []; // Many-to-Many relationship with Tag

    public List<BusinessHours> BusinessHours { get; set; } = []; // One-to-Many relationship with BusinessHours
}
```
```
[Table("ResourceTags", Schema = "dbo")]
public class ResourceTags
{
    [Key, Column(Order = 0)]
    [Column("TagId", TypeName = "int")]
    public int TagId { get; set; }

    [Key, Column(Order = 1)]
    [Column("FoodResourceId", TypeName = "int")]
    public int FoodResourceId { get; set; }

    [ForeignKey("TagId")]
    public Tag Tag { get; set; } // Many-to-One relationship with Tag

    [ForeignKey("FoodResourceId")]
    public FoodResource FoodResource { get; set; } // Many-to-One relationship with FoodResource
}
```
```
[Table("Tag", Schema = "dbo")]
public class Tag
{
    [Key]
    [Column("Id", TypeName = "int")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("Name", TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    public List<FoodResource> FoodResources { get; set; } // Many-to-One relationship with ResourceTags
}
```
```
[Table("BusinessHours", Schema = "dbo")]
public class BusinessHours
{
    [Key]
    [Column("BusinessHourID", TypeName = "int")]
    public int BusinessHourID { get; set; }

    [Required]
    [Column("FoodResourceID", TypeName = "int")]
    public int FoodResourceID { get; set; }

    [ForeignKey("FoodResourceID")]
    public FoodResource FoodResource { get; set; } // Many-to-One relationship with FoodResource

    [Required]
    [Column("DayId", TypeName = "int")]
    public int DayId { get; set; }

    [ForeignKey("DayId")]
    public Days Day { get; set; } // Many-to-One relationship with Days

    [MaxLength(10)]
    [Column("OpenTime", TypeName = "nvarchar(10)")]
    public string? OpenTime { get; set; }

    [MaxLength(10)]
    [Column("CloseTime", TypeName = "nvarchar(10)")]
    public string? CloseTime { get; set; }
}
```
```
[Table("Days", Schema = "dbo")]
public class Day
{
    [Key]
    [Column("Id", TypeName = "int")]
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    [Column("Name", TypeName = "nvarchar(10)")]
    public string Name { get; set; }

    public List<BusinessHours> BusinessHours { get; set; } = [];  // One-to-Many relationship with BusinessHours
}
```
### [Fluent API](https://www.learnentityframeworkcore.com/configuration/fluent-api) (Optional if not using Data Annotations/Mapping Atrributes)
This approach is an alternative to using data annotations and provides more control over the configuration with the plus of being able to be located in one place, away from the model classes. This is applied inside of the database context class to entities/models properties via chained methods. 
- These methods can be in conjuntion with Data annotations to cover areas that are missed. Such as default Schema, DB functions, additional data annotation attributes and entities to be excluded from mapping.
- Also can handle entity to table and relationships mapping e.g. PrimaryKey, AlternateKey, Index, table name, one-to-one, one-to-many, many-to-many relationships etc.
- And lastly, provide property configuration meaning column name, default value, nullability, ForeignKey, data type, concurrency column etc.

#### Implementing Fluent API:
Adding an entity to OnModelCreating method inside the DataContext Class we made ealier but left empty. Starting with FoodResource using the ToTable method to specify the name of the database table that the entity should map to.
- Should look like this:
```
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
   
    modelBuilder.Entity<FoodResource>(entity =>
    {
        entity.ToTable("FoodResource");
    });
}
```

Now using the [HasKey](https://www.learnentityframeworkcore.com/configuration/fluent-api/haskey-method) method is used to denote the property that uniquely identifies an entity Primary Key field in a database:
- Add this right after the first entry inside FoodResource:
```
 entity.HasKey(e => e.Id).HasName("PK_FoodResource");
```

Entity Framework Core provides a range of options for configuring entity [properties using the Fluent API](https://www.learnentityframeworkcore.com/configuration/fluent-api/property-configuration). These can include column types, names, if required, length, and couple more. 
- Add this right after the our last entry inside FoodResource:
```
entity.Property(e => e.Name).IsRequired().HasMaxLength(255).HasColumnName("Name").HasColumnType("nvarchar(255)");
```

Using these steps you should be able to fill out and address the rest of the properties of the FoodResource Entity
- Should end up like this:
```
modelBuilder.Entity<FoodResource>(entity =>
{
 entity.ToTable("FoodResource");
    entity.HasKey(e => e.Id).HasName("PK_FoodResource");
    entity.Property(e => e.Id).HasColumnName("Id").HasColumnType("int");
    entity.Property(e => e.Name).IsRequired().HasMaxLength(255).HasColumnName("Name").HasColumnType("nvarchar(255)");
    entity.Property(e => e.Area).HasMaxLength(100).HasColumnName("Area").HasColumnType("nvarchar(100)");
    entity.Property(e => e.StreetAddress).IsRequired().HasMaxLength(255).HasColumnName("StreetAddress").HasColumnType("nvarchar(255)");
    entity.Property(e => e.City).IsRequired().HasMaxLength(100).HasColumnName("City").HasColumnType("nvarchar(100)");
    entity.Property(e => e.State).IsRequired().HasMaxLength(2).HasColumnName("State").HasColumnType("nvarchar(2)");
    entity.Property(e => e.Zipcode).IsRequired().HasMaxLength(50).HasColumnName("Zipcode").HasColumnType("varchar(50)");
    entity.Property(e => e.Country).HasMaxLength(100).HasColumnName("Country").HasColumnType("nvarchar(100)");
    entity.Property(e => e.Latitude).IsRequired().HasColumnName("Latitude").HasColumnType("decimal(9, 6)");
    entity.Property(e => e.Longitude).IsRequired().HasColumnName("Longitude").HasColumnType("decimal(9, 6)");                    
    entity.Property(e => e.Phone).HasMaxLength(20).HasColumnName("Phone").HasColumnType("nvarchar(20)");
    entity.Property(e => e.Website).HasMaxLength(255).HasColumnName("Website").HasColumnType("nvarchar(255)");
    entity.Property(e => e.Description).HasColumnName("Description").HasColumnType("nvarchar(MAX)");
});
```

While this is how you would address the basic properties. Next would be to move on to the different types of relationships we touched on earlier. 
##### One-To-One
Fluent API uses a `HasOne` method to configure the one side of a one to many relationship, or one end of a one to one relationship. 
- `Note:` It is never necessary to configure a relationship twice, once starting from the principal, and then again starting from the dependent. Also, attempting to configure the principal and dependent halves of a relationship separately generally does not work. Choose to configure each relationship from either one end or the other and then write the configuration code only once.
- `One-to-One` relationships are used when one entity is associated with at most one other entity. For example, a Blog has one BlogHeader, and that BlogHeader belongs to a single Blog.
- [One to One](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-one) example would look something like this:
```
  public class SampleContext : DbContext
{
    public DbSet<Blog> Blog { get; set; }
    public DbSet<BlogHeader> BlogHeader { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
          .HasOne(e => e.Header)
          .WithOne(e => e.Blog)
          .HasForeignKey<BlogHeader>(e => e.BlogId)
          .IsRequired(false);
    }
}
// Principal (parent/principal)
public class Blog
{
    public int Id { get; set; }
    public BlogHeader? Header { get; set; } // Reference navigation to dependent
}
}
// Dependent (child/dependent)
public class BlogHeader
{
    public int Id { get; set; }
    public Blog Blog { get; set; } = null!; // Required reference navigation to principal
}
```

##### One-To-Many
With that being said when used in conjunction with `HasOne` method the `WithMany` can configure a `One-to-Many` relationship. The example below descibes the relationship between the FoodResource and the BusinessHours but inisde the entity FoodResource. If you see how it is written you can see how it be defined in either enitity. This kind of realtionship is made up of:
- One or more primary or alternate key properties on the principal entity; that is the "one" end of the relationship.
- One or more ForeignKeys properties on the dependent entity; that is the "many" end of the relationship.
- `Optional:` a collection navigation on the principal entity referencing the dependent entities.
- `Optional:` a reference navigation on the dependent entity referencing the principal entity.
- [One to Many](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many) example would look something like this:
```
  public class SampleContext : DbContext
{
    public DbSet<FoodResource> FoodResource { get; set; }
    public DbSet<BusinessHours> BusinessHours { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FoodResource>(entity =>
        {
            entity.ToTable("FoodResource");
            etc...
            entity.HasMany(e => e.BusinessHours)
                  .WithOne(e => e.FoodResource)
                  .HasForeignKey(e => e.FoodResourceId)
                  .IsRequired();
        });
    }
public class FoodResource
{
    public int Id { get; set; }
    // etc....
    public List<BusinessHours> BusinessHours { get; } = []; // Collection navigation containing dependents
}
public class BusinessHours
{
    public int BusinessHourId { get; set; }
    public int FoodResourceId { get; set; }  // Required foreign key property
    // etc....
    public FoodResource FoodResource { get; set; } = null!; // Required reference navigation to principal
}
```

##### Many-To-Many
When it comes to [Many-To-Many](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many) relationships instead of using the `hasOne` method we will instead use `hasMany` with `WithMany`. If you think of how this would be reflected in a database you can kinda see why I mentioned adding the join table when it came to describing the realtionship between FoodResource and Tags. This can be a bit tricky if you check the documentation there are a lot of ways to go about this without the use of join tables described as an enitity like ResourceTags. This is beccause EFC has many default setting and if defined correctly will make those assumptions for you and should therefore be reflected similarly database.
- `Note:` Join tables in particular are unique to Many to Many relationships. 
- Here is one way to acheive a Many-to-Many relationship example:
```
  public class SampleContext : DbContext
{
    public DbSet<FoodResource> FoodResource { get; set; }
    public DbSet<ResourceTags> ResourceTags { get; set; }
    public DbSet<Tag> Tag { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FoodResource>(entity =>
        {
            entity.ToTable("FoodResource");
            // etc....
           entity.HasMany(e => e.Tag)
                  .WithMany(e => e.FoodResource)
                  .UsingEntity<ResourceTags>(
                      l => l.HasOne<Tag>().WithMany().HasForeignKey(e => e.TagId),
                      r => r.HasOne<FoodResource>().WithMany().HasForeignKey(e => e.FoodResourceId));
        });
    }
public class FoodResource
{
    public int Id { get; set; }
    // etc....
    public List<Tag> Tag { get; } = []; // Collection navigation containing dependents
}
public class ResourceTags
{
    public int TagId { get; set; } // Required ForeignKey property
    public int FoodResourceId { get; set; } // Required ForeignKey property
}
public class Tag
{
     public int Id { get; set; }
     public string Name { get; set; } = null!;

     public List<FoodResource> FoodResource { get; } = [];  // Required reference navigation to principal
}
```

##### OnDelete()
When working with relationships there may also come a time when you want preserve dependent data or specify that they should be deleted as well. This is where the [onDelete](https://www.learnentityframeworkcore.com/configuration/fluent-api/ondelete-method) method comes in to play.
- `Cascade` means that dependents should be deleted
- `Restrict` means that dependents are unaffected
- `SetNull` means that the ForeignKey values in dependent rows should update to NULL
- Here is an example Referening BusinessHours and Day:
```
 public class SampleContext : DbContext
{
    public DbSet<FoodResource> FoodResource { get; set; }
    public DbSet<BusinessHours> BusinessHours { get; set; }
    public DbSet<Day> Day { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BusinessHours>(entity =>
        {
            entity.ToTable("BusinessHours");
            // etc....

            entity.HasOne(d => d.FoodResource).WithMany(p => p.BusinessHours)
                  .HasForeignKey(d => d.FoodResourceId)
                  .OnDelete(DeleteBehavior.Cascade) // Cascade delete for FoodResource
                  .HasConstraintName("FK_BusinessHours_FoodResource");
            entity.HasOne(d => d.Day).WithMany(p => p.BusinessHours)
                  .HasForeignKey("DayId")
                  .OnDelete(DeleteBehavior.Restrict) // No action on delete for Day
                  .HasConstraintName("FK_BusinessHours_Days");
        });
        modelBuilder.Entity<FoodResource>(entity =>
        {
          entity.HasMany(e => e.Tags)
                .WithMany(e => e.FoodResource)
                .UsingEntity<ResourceTags>(
                    l => l.HasOne<Tag>().WithMany().HasForeignKey(e => e.TagId).HasConstraintName("FK_ResourceTags_Tags"),
                    r => r.HasOne<FoodResource>().WithMany().HasForeignKey(e => e.FoodResourceId).HasConstraintName("FK_ResourceTags_FoodResource")
                );
        });
    }
    public class FoodResource
    {
      public int Id { get; set; }
      // etc....

      public List<BusinessHours> BusinessHours { get; } = []; // Collection navigation containing dependents
    }
    public class BusinessHours
    {
      public int BusinessHourId { get; set; }
      public int FoodResourceId { get; set; } // Required foreign key property
      public int DayId { get; set; } // Required foreign key property
      public string? OpenTime { get; set; }
      public string? CloseTime { get; set; }
  
      public Day Day { get; set; } = null!; // Required reference navigation to principal
      public FoodResource FoodResource { get; set; } = null!; // Required reference navigation to principal
    }
    public class Day
     {
         public int Id { get; set; }
         public string Name { get; set; } = null!;
    
         public List<BusinessHours> BusinessHours { get; } = [];  // Collection navigation containing dependents
     }
}
```

<!--
In the examples so far, the join table has been used only to store the ForeignKey pairs representing each association. However, it can also be used to store information about the association--for example, the time it was created.This is the case when we get to the BusinessHours table as it is [Many-to-many and join table with payload](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many).
- Here is how you would handle this situation:
```
  public class SampleContext : DbContext
{
    public DbSet<FoodResource> FoodResource { get; set; }
    public DbSet<ResourceTags> ResourceTags { get; set; }
    public DbSet<Tag> Tag { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ResourceTags>(entity =>
        {
            entity.ToTable("ResourceTags");
            entity.HasKey(e => new { e.TagId, e.FoodResourceId });
            entity.HasOne(e => e.FoodResource).WithMany(fr => fr.ResourceTags).HasForeignKey(e => e.FoodResourceId).IsRequired().HasConstraintName("FK_ResourceTags_FoodResource");
            entity.HasOne(e => e.Tag).WithMany(t => t.ResourceTags).HasForeignKey(e => e.TagId).IsRequired().HasConstraintName("FK_ResourceTags_Tags");
        });
    }
public class FoodResource
{
    public int Id { get; set; }
    // etc....
    public List<BusinessHours> BusinessHours { get; } = []; // Collection navigation containing dependents
}
public class BusinessHours
{
    public int TagId { get; set; } // Required foreign key property
    public int FoodResourceId { get; set; } // Required foreign key property
    public Tag Tag { get; set; } = null!; // Required reference navigation to principal
    public FoodResource FoodResource { get; set; } = null!; // Required reference navigation to principal
}
 public class Tag
 {
     public int Id { get; set; }
     public string Name { get; set; } = null!;

     public List<ResourceTags> ResourceTags { get; } = [];  // Collection navigation containing dependents
 }
```
-->

#### Go ahead and finish up the rest of the configurations and if you get stuck please refer to the [code](https://github.com/jjcanell77/LADP--EFC/tree/master/LADP-%20EFC) for help.

##  Step 5 : Migrations (Can be Skipped for until this section is completed)
EF Core provides us with two primary ways of keeping the EF Core model and database schema in sync. It does this by letting us choose between whether the EF Core model or the database schema is the source of truth and go from there.
- [Model](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli): if the model is the source truth then as we make changes to the model we will incrementally applies the corresponding schema changes to your database so that it remains compatible with your EF Core model.
- [Database](https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli): vise versa if you want your database schema to be the source of truth we can use Reverse Engineering to scaffold a DbContext and the entity type classes by reverse engineering your database schema into an EF Core model.
#### My intentions are to proceed with the Model First Aprroach

##  Step 6 : Set Up Controllers
As mentioned we are using a `controler-based API` and a Web API controller is a class which can be created under the Controllers folder or any other folder under your project's root folder. It handles incoming HTTP requests and send response back to the caller. This can include multiple action methods whose names match with HTTP verbs like Get, Post, Put and Delete.
### Scaffold a controller (Optional)
One way to do this is to take advatage of Scaffolding, or you can build it from scratch. Scaffolding uses ASP.Net's templates to create a basic API Controller. This is just to get you started as you will need to make updates which is why it is optional. 
This template will mark the class with the [ApiController] attribute, that indicates that the controller responds to web API requests. It also uses DI to inject the database context (DataContext) into the controller. The database context is used in each of the CRUD methods in the controller.
-	Right-click the `Controllers` folder.
-	Select `Add` > `New Scaffolded Item`.
-	Select `API Controller with actions`, using `Entity Framework`, and then select `Add`.
-	Select `FoodResource(LADP_EFC.Models)` in the `Model class`.
-	Select `DataContext(LADP_EFC.Data)` in the `Data context class`.
-	Select `Add`.
-	Should look something like this but we will add logic in the methods later:
```
namespace LADP__EFC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodResourcesController : ControllerBase
    {
        private readonly DataContext _context;

        public FoodResourcesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/FoodResources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodResource>>> GetFoodResources(){}

        // GET: api/FoodResources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodResource>> GetFoodResource(int id){}

        // PUT: api/FoodResources/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodResource(int id, FoodResource foodResource){}

        // POST: api/FoodResources
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FoodResource>> PostFoodResource(FoodResource foodResource)
        {}

        // DELETE: api/FoodResources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodResource(int id)
        {}

        private bool FoodResourceExists(int id)
        {}
    }
}
```

#### Update Controller Methods
Now that we have the basic set up for the controllers its is time to update the methods. Remember we will be utilizing a [Repository](https://github.com/jjcanell77/LADP--EFC/blob/master/Instructions.md#step-7-setting-up-the-repository) that will handle all of the logic behind the actual execution of these so it us best to set to handle exceptions, status codes, and returning the object if neccessary.
<!-- Need to update link when moved to main project -->
##### Action Return Types
Here are the following options for web API controller action return types that can be used:
- void: if you are returning nothing.
- [Specific type](https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-8.0#specific-type): can just return a specific type like a string or a custom entity.

This action will return a `200 Ok` status code along with whichever type you choose when it runs successfully. Of course, in case of errors, it will return a `500 Error` status code along with error details.However, if we want to add some validations into the action and return a validation failure with a `400 Bad Request` response, this approach won’t work and we have to use either `IActionResult` or `ActionResult<T>` types.
- [IActionResult](https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-8.0#iactionresult-type)

Whenever an action has multiple return paths and needs to support returning multiple `ActionResult` types, then  `IActionResult` is a great choice. 

On top of that, the ControllerBase class has defined some convenience methods which is equivalent to creating and returning an ActionResult type. For instance, if we want to return a 400 Bad Request response, instead of using the return new BadRequestResult();, we could just write return BadRequest(); . Similarly, we could use Ok() and NotFound() methods to return the 200 Ok and 404 Not Found responses respectively.

While using IActionResult type, it is important to provide the [ProducesResponseType] attribute for all possible scenarios since multiple response types and paths are possible.

Ex.
```
[HttpGet("{id}")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FoodResource))]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public IActionResult GetById(int id)
{
    var foodResource = _repository.GetById(id);
    if (foodResource == null)
    {
        return NotFound();
    }

    return Ok(foodResource);
}
```
- [ActionResult<T>](https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-8.0#actionresultt-type)

One advantage of using this type is that we can skip the Type property of the [ProducesResponseType] attribute. This is because the expected return type can be inferred from the T in the ActionResult<T>. Or we exclude [ProducesResponseType] all together.
Ex.
```
[HttpGet("{id}")]
public ActionResult<FoodResource> GetFoodResource(int id)
{
        var foodResource = _repository.GetById(id);

        if (foodResource == null)
        {
            return NotFound();
        }

        return Ok(foodResource);
    }
}
```

<!-- - [HttpResults](https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-8.0#httpresults-type) -->

## Step 7: Setting up the Repository
Using the repository pattern in Entity Framework Core helps create a clean separation between the data access and business logic layers. This will have a similar functionality that the Services Folder/Files did in your Sabio Project. 
- In Solution Explorer, right-click the project.
- Select Add > New Folder. Name the folder Repository.
-	Right-click the Repository folder and Add > New Folder inside that called Interfaces.
-	Should look something like:
![Interface Folder](https://github.com/user-attachments/assets/19d18104-d078-4840-8fb4-6d970fd4c7fa)
### The Interface
The best way to think of an interface is a contract. By having this contract we can have better control on change management and other breaking changes. 
-	Right-click the Interfaces folder, then  Select Add > New Item...
-	Select Add > Interface. Name the Interface IRepositoryFoodResource and click Add.
-	Next your going to add you basic CRUD methods for now.
-	Should look something like almost mimicking the methods in your base controller:
```
public interface IRepositoryFoodResource
{
    IEnumerable<FoodResource> GetFoodResources();
    FoodResource GetFoodResource(int id);
    FoodResource PutFoodResource(int id, FoodResource foodResource);
    FoodResource PostFoodResource(FoodResource foodResource);
    FoodResource DeleteFoodResource(int id);
}
```

### The Repository/Service
-	Right-click the Repository folder, then  Select Add > class...
-	Name the class RepositoryFoodResource and click Add.
-	Next your going to inherit from IRepositoryFoodResource.
```
public class RepositoryFoodResource : IRepositoryFoodResource
{
}
```
-	You should see an error appear on IRepositoryFoodResource.
-	Right-click IRepositoryFoodResource, then  Select Lightbulb for Quick Refactorings...
-	 Select Implement the interface.
-	 Should look like this:
```
public class RepositoryFoodResource : IRepositoryFoodResource
{
    public FoodResource DeleteFoodResource(int id)
    {
        throw new NotImplementedException();
    }

    public FoodResource GetFoodResource(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<FoodResource> GetFoodResources()
    {
        throw new NotImplementedException();
    }

    public FoodResource PostFoodResource(FoodResource foodResource)
    {
        throw new NotImplementedException();
    }

    public FoodResource PutFoodResource(int id, FoodResource foodResource)
    {
        throw new NotImplementedException();
    }
}
```
- Just like when we scaffolded the controller this is only a starting point.
- Next we need to link out Repository to our DataContext that holds our model configurations.
- This is done by copying the constructor from the the controller to our Repository.
-	 Should look like this:
```
private readonly DataContext _context;

public RepositoryFoodResource(DataContext context)
{
    _context = context;
}
```
- This should be pasted at the top of our Repository class.

### Adjusting the Controller
Because we now linked the the DataContext to our Repository, we now need to replace that logic to link the Repository to our controller. 
- The same logic that we copied to the Repository shoud be replaced with:
```
private readonly IRepositoryFoodResource _repositroy;
public FoodResourcesController(IRepositoryFoodResource repository)
{
    _repositroy = repository;
}
```
- Go ahead ad swap all _context with _repositroy.
- Followed by there corresponding CRUD method.
- This is because all of the logic is moving the the Repository. 
-	 Should look like this:
```
public class FoodResourcesController : ControllerBase
{
    private readonly IRepositoryFoodResource _repositroy;
    public FoodResourcesController(IRepositoryFoodResource repository)
    {
        _repositroy = repository;
    }

    // GET: api/FoodResources
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FoodResource>>> GetFoodResources()
    {
        var foodResource = _repositroy.GetFoodResources();
        return Ok(foodResource);
    }

    // GET: api/FoodResources/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FoodResource>> GetFoodResource(int id)
    {
        var foodResource =  _repositroy.GetFoodResource(id);

        if (foodResource == null)
        {
            return NotFound();
        }

        return foodResource;
    }

    // PUT: api/FoodResources/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFoodResource(int id, FoodResource foodResource)
    {
        if (id != foodResource.Id)
        {
            return BadRequest();
        }

        var item = _repositroy.PutFoodResource(id, foodResource);
        if (item != null)
        {
            return NoContent();
        }
        return NotFound();
    }

    // POST: api/FoodResources
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<FoodResource>> PostFoodResource(FoodResource foodResource)
    {
        var item = _repositroy.PostFoodResource(foodResource);
        return CreatedAtAction("GetFoodResource", new { id = foodResource.Id }, foodResource);
    }

    // DELETE: api/FoodResources/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFoodResource(int id)
    {
        var item = _repositroy.DeleteFoodResource(id);
        if (item == null)
        {
            return NotFound();
        }
        return NoContent();
    }
}
```

### Adding Repository to Dependency injection container
In ASP.NET Core, the DI container provides several lifetimes for services: Singleton, Scoped, and Transient. Each has specific use cases, and choosing the correct lifetime is crucial for the application's performance and correctness.
- A singleton is a single instance of the service is created and shared across the entire application lifecycle.
- Scoped is a new instance of the service is created once per request and shared within that request.
- Transient is a new instance of the service is created each time it is requested.

For our project we will be implementing AddScoped into our Program.cs similar to how we add singletons in Sabio to add IRepositoryFoodResource.
-	Should look something like:
```
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        // Add Repositories to the container.
        builder.Services.AddScoped<IRepositoryFoodResource, RepositoryToDoItems>();
        builder.Services.AddScoped<IRepositoryToDoItems, RepositoryToDoItems>();
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
```
