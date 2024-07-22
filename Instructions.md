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
 ``` { attributes go here }
Microsoft.EntityFrameworkCore
```
```
Microsoft.EntityFrameworkCore.SqlServer
```
```
Microsoft.EntityFrameworkCore.Tools
```

##  Step 2 : Setting up entities/models 
The entity class will consist of the object that your storing. This model is built using a set of [conventions](https://www.entityframeworktutorial.net/efcore/conventions-in-ef-core.aspx) that look for common patterns. And when making your model you will see that some that show a One-to-Many, Many-to-Many, or even One-to-One realtionship.

- Here is an example of the object we are creating in JS:
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

### Add a entity/model class
-	In Solution Explorer, right-click the project. Select Add > New Folder. Name the folder Models.
-	Right-click the Models folder and select Add > Class. Name the class FoodResource and select Add.
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
    public List<ResourceTags> ResourceTags { get; } = []; // One-to-Many relationship with Tag
    public List<BusinessHours> BusinessHours { get; set; } = [];// One-to-Many relationship with BusinessHours
}
```
The table for ResourceTags creates a [Many-to-One](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many) relationship between FoodResource and Tag by linking TagId and FoodResourceId. This is because FoodResource can have have many associated tags, and each Tag can in turn be associated with any number of FoodResource giving us the [one-to-many relationship](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many).
- Should look something like:
```
public class ResourceTags
{
    public int TagId { get; set; }
    public int FoodResourceId { get; set; }
    public Tag Tag { get; set; } = null!; // Many-to-One relationship with Tag

    public FoodResource FoodResource { get; set; } = null!; // Many-to-One relationship with FoodResource
}
```

```
 public class Tag
 {
     public int Id { get; set; }
     public string Name { get; set; } = null!;

     public List<ResourceTags> ResourceTags { get; } = [];  // One-to-Many relationship with ResourceTags
 }
```
Now the table for BusinessHours table gives us a one-to-many relationship, because Each entry in the businessHours array would be a record in this table.

- Should look something like:
```
public class BusinessHours
{
    public int BusinessHourId { get; set; }
    public int FoodResourceId { get; set; }
    public int DayId { get; set; }
    public string? OpenTime { get; set; }
    public string? CloseTime { get; set; }

    public Day Day { get; set; } = null!; // Many-to-One relationship with Days
    public FoodResource FoodResource { get; set; } = null!; // Many-to-One relationship with FoodResource
}
```
The Days table holds the day names ("Monday", "Tuesday", etc.), which can be used by many BusinessHours. It holds only one version of those days, and the BusinessHours table references the Days table using DayId.

- Should look something like:
```
public class Day
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public List<BusinessHours> BusinessHours { get; set; } = []; // One-to-Many relationship with BusinessHours
}
```
 Now Depending on how we intend to configure these models we will be returning to the classes in step 4. 
 
##  Step 3: Database Integration 
The database context is the main class that coordinates Entity Framework functionality for a data model. This class is responsible for querying and saving data to your entity classes, and for creating and managing the database connection. Acting sort of like a “Fish-Hook” into the database from your API, imagine a line being cast from your API Application/System into a SQL database via your connection string value! This is called Data Persistence which is a fancy word for any changes done with CRUD operations to be reflected onto the database.
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
        public DbSet<Day> Day { get; set; }
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
## Step 4: Configuring a Model 
The model can then be customized using mapping attributes (also known as data annotations) and/or calls to the ModelBuilder methods (also known as fluent API) in OnModelCreating, both of which will override the configuration performed by conventions.

### [Data Annotations/Mapping Atrributes](https://www.learnentityframeworkcore.com/configuration/data-annotation-attributes) (Optional if not using prefered Fluent API)
Using atrributes are nice because they are applied directly to the domain model, so it is easy to see how the model is configured just by examining the class files. You may have remember using Some attributes, such as Required and StringLength are leveraged by client frameworks such as ASP.NET MVC to provide UI-based validation based on the specified configuration.
- One of the cons to using Data Annotations is that it may not cover every type of configuration, and could result in you having to implement some Fluent API as well. This will split configurations to more than one location adding unecessary complexity. 
- Here is an example of the entities:
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
    public string Name { get; set; }

    [MaxLength(100)]
    [Column("Area", TypeName = "nvarchar(100)")]
    public string? Area { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("StreetAddress", TypeName = "nvarchar(255)")]
    public string StreetAddress { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("City", TypeName = "nvarchar(100)")]
    public string City { get; set; }

    [Required]
    [MaxLength(2)]
    [Column("State", TypeName = "nvarchar(2)")]
    public string State { get; set; }

    [Required]
    [MaxLength(10)]
    [Column("Zipcode", TypeName = "varchar(10)")]
    public string Zipcode { get; set; }

    [MaxLength(50)]
    [Column("Country", TypeName = "varchar(50)")]
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

    public ICollection<ResourceTags> ResourceTags { get; set; } // Many-to-Many relationship with Tag

    public ICollection<BusinessHours> BusinessHours { get; set; } // One-to-Many relationship with BusinessHours
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

    public ICollection<ResourceTags> ResourceTags { get; set; } // One-to-Many relationship with ResourceTags
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
    public string OpenTime { get; set; }

    [MaxLength(10)]
    [Column("CloseTime", TypeName = "nvarchar(10)")]
    public string CloseTime { get; set; }
}
```
```
[Table("Days", Schema = "dbo")]
public class Days
{
    [Key]
    [Column("Id", TypeName = "int")]
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    [Column("Name", TypeName = "nchar(10)")]
    public string Name { get; set; }

    public ICollection<BusinessHours> BusinessHours { get; set; } // One-to-Many relationship with BusinessHours
}
```
### [Fluent API](https://www.learnentityframeworkcore.com/configuration/fluent-api) (Optional if not using Data Annotations/Mapping Atrributes)
This approach is an alternative to using data annotations and provides more control over the configuration with the plus of being able to be located in one place, away from the model classes. This is applied inside of the database context class to entities/models properties via chained methods. 
- These methods can be in conjuntion with Data annotations to cover areas that are missed. such as default Schema, DB functions, additional data annotation attributes and entities to be excluded from mapping.
- Also can handle entity to table and relationships mapping e.g. PrimaryKey, AlternateKey, Index, table name, one-to-one, one-to-many, many-to-many relationships etc.
- And lastly, provide property onfiguration meaning column name, default value, nullability, Foreignkey, data type, concurrency column etc.

#### The steps:
Adding an entity to OnModelCreating method inside the DataContext Class we made ealier. Starting with FoodResource using the ToTable method to specify the name of the database table that the entity should map to.
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

Using these steps you should be able to fill out the rest of the FoodResource Entity
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

The only exceptions would be when dealing with different types of relationships we touched on earlier. Fluent API uses a HasOne method to configure the one side of a one to many relationship, or one end of a one to one relationship. It is never necessary to configure a relationship twice, once starting from the principal, and then again starting from the dependent. Also, attempting to configure the principal and dependent halves of a relationship separately generally does not work. Choose to configure each relationship from either one end or the other and then write the configuration code only once.
- [One to One](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-one) example would look soemthing like this:
```
  public class SampleContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>()
            .HasOne(a => a.Biography)
            .WithOne(b => b.Author);
    }
}
public class Author
{
    public int AuthorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public AuthorBiography Biography { get; set; }
}
public class AuthorBiography
{
    public int AuthorBiographyId { get; set; }
    public string Biography { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
}
```

With that being said when used in conjunction with HasOne method the WithMany can configure a One to Many relationship. The example below has two one-to-many relationships, one for each of the foreign keys defined in the join table. This realtionship is made up of:
- One or more primary or alternate key properties on the principal entity; that is the "one" end of the relationship.
- One or more foreign key properties on the dependent entity; that is the "many" end of the relationship.
- Optionally, a collection navigation on the principal entity referencing the dependent entities.
- Optionally, a reference navigation on the dependent entity referencing the principal entity.
- [One to Many](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many) example would look soemthing like this:
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
            entity.HasOne(e => e.FoodResource).WithMany(fr => fr.ResourceTags).HasForeignKey(e => e.FoodResourceId).IsRequired();
            entity.HasOne(e => e.Tag).WithMany(t => t.ResourceTags).HasForeignKey(e => e.TagId).IsRequired();
        });
    }
public class FoodResource
{
    public int Id { get; set; }
    // etc....
    public List<ResourceTags> ResourceTags { get; } = []; // Collection navigation containing dependents
}
public class ResourceTags
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

Now with the example given above with FoodResource, ResourceTags, and Tags have one-to-many relationships between them and that works just fine. You just have to imagine that from FoodResource to Tags there is a [Many to Many](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many) relationship, this is due to the join table or ResourceTags in this case. Join tables in particular are is unique to Many to Many relationships. If you check the documentationt there a alot of ways to go about this achiecing this without the use of join tables. I will show you one of these way below but this is optional as the two one-to-many relationships will suffice. 

- Here is an optional way to acheive a Many-to-Many relationship example:
```
  public class SampleContext : DbContext
{
    public DbSet<FoodResource> FoodResource { get; set; }
    public DbSet<Tag> Tag { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FoodResource>(entity =>
        {
            entity.ToTable("FoodResource");
            // etc....
            entity.HasMany(e => e.Tags).WithMany(e => e.FoodResources);
        });
    }
public class FoodResource
{
    public int Id { get; set; }
    // etc....
    public List<Tag> Tags { get; } = []; // Collection navigation containing dependents
}
 public class Tag
 {
     public int Id { get; set; }
     public string Name { get; set; } = null!;

     public List<FoodResource> FoodResources { get; } = [];  // Collection navigation containing dependents
 }
```

When working with relationships there may also come a time when you want preserve dependent data or sepcidfy that they should be deleted as well. This is where the [onDelete](https://www.learnentityframeworkcore.com/configuration/fluent-api/ondelete-method) method comes in to play. 
- Cascade means that dependents should be deleted
- Restrict means that  dependents are unaffected
- SetNull means that the foreign key values in dependent rows should update to NULL
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
            entity.HasKey(e => e.BusinessHourId).HasName("PK_BusinessHours");
            entity.Property(e => e.BusinessHourId).HasColumnName("BusinessHourID").HasColumnType("int");
            entity.HasOne(d => d.FoodResource).WithMany(p => p.BusinessHours)
              .HasForeignKey(d => d.FoodResourceId)
              .OnDelete(DeleteBehavior.Cascade) // Cascade delete for FoodResource
              .HasConstraintName("FK_BusinessHours_FoodResource");
            entity.Property(e => e.FoodResourceId).HasColumnName("FoodResourceID").HasColumnType("int");
            entity.HasOne(d => d.Day).WithMany(p => p.BusinessHours)
              .HasForeignKey(d => d.DayId)
              .OnDelete(DeleteBehavior.Restrict) // No action on delete for Day
              .HasConstraintName("FK_BusinessHours_Days");
            entity.Property(e => e.OpenTime).HasMaxLength(10).HasColumnName("OpenTime").HasColumnType("nvarchar(10)");
            entity.Property(e => e.CloseTime).HasMaxLength(10).HasColumnName("CloseTime").HasColumnType("nvarchar(10)");
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
```

<!--
In the examples so far, the join table has been used only to store the foreign key pairs representing each association. However, it can also be used to store information about the association--for example, the time it was created.This is the case when we get to the BusinessHours table as it is [Many-to-many and join table with payload](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many).
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
##  Step 5 : Set Up Controllers
As mentioned we are using a controler based API and a Web API controller is a class which can be created under the Controllers folder or any other folder under your project's root folder. It handles incoming HTTP requests and send response back to the caller. This can include multiple action methods whose names match with HTTP verbs like Get, Post, Put and Delete.
### Scaffold a controller (Optional)
One way to do this is to take advatage of Scaffolding, or you can build it from scratch. Scaffolding uses ASP.Net's templates to create a basic API Controller. This is just o get you started as you will need to make updates which is why it is optional. 
This template will Mark the class with the [ApiController] attribute, that indicates that the controller responds to web API requests. It also uses DI to inject the database context (DataContext) into the controller. The database context is used in each of the CRUD methods in the controller.
-	Right-click the Controllers folder.
-	Select Add > New Scaffolded Item.
-	Select API Controller with actions, using Entity Framework, and then select Add.
-	Select FoodResource(LADP_EFC.Models) in the Model class.
-	Select DataContext(LADP_EFC.Data) in the Data context class.
-	Select Add.
-	Should look something like this but with logic in the methods:
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

## Step 6: Setting up the Repository
Using the repository pattern in Entity Framework Core helps create a clean separation between the data access and business logic layers. This will have a similar functionality that the Services Folder/Files did in your Sabio Project. 
- In Solution Explorer, right-click the project.
- Select Add > New Folder. Name the folder Repository.
-	Right-click the Repository folder and Add > New Folder inside that called Interfaces.
-	Should look something like:
![Interface Folder](https://github.com/user-attachments/assets/19d18104-d078-4840-8fb4-6d970fd4c7fa)
### The Interface
The best way to think of an interface is a contract. By having this contract we can have bette control on change management and other breaking changes. 
-	Right-click the Interfaces folder, then  Select Add > New Item...
-	Select Add > Interface. Name the Interface IRepositoryFoodResource and click Add.
-	Next your going to add you basic CRUD methods for now.
-	Should look something like almost mimicing the methods in your base controller:
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
-	Right-click IRepositoryFoodResource, then  Select Lightbulb for Quick Refactotrings...
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
- A singletion is a single instance of the service is created and shared across the entire application lifecycle.
- Scoped is a new instance of the service is created once per request and shared within that request.
- Transientis a new instance of the service is created each time it is requested.

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
