La carpeta `News/` es un **ejemplo de otro proyecto** que debes usar como plantilla para crear tus propios bounded contexts:

#### **Estructura a Copiar y Renombrar:**
```
News/                                    📋 Copiar estructura completa
├── Application/
│   └── Internal/
│       ├── CommandServices/
│       │   └── FavoriteSourceCommandService.cs    → TuEntidadCommandService.cs
│       └── QueryServices/
│           └── FavoriteSourceQueryService.cs      → TuEntidadQueryService.cs
├── Domain/
│   ├── Model/
│   │   ├── Aggregates/
│   │   │   ├── FavoriteSource.cs                  → TuEntidad.cs
│   │   │   └── FavoriteSourceAudit.cs            → TuEntidadAudit.cs
│   │   ├── Commands/
│   │   │   └── CreateFavoriteSourceCommand.cs    → CreateTuEntidadCommand.cs
│   │   └── Queries/
│   │       ├── GetAllFavoriteSourcesByNewsApiKeyQuery.cs  → GetTuEntidadQuery.cs
│   │       ├── GetFavoriteSourceByIdQuery.cs     → GetTuEntidadByIdQuery.cs
│   │       └── GetFavoriteSourceByNewsApiKeyAndSourceIdQuery.cs  → (adaptar)
│   ├── Repositories/
│   │   └── IFavoriteSourceRepository.cs          → ITuEntidadRepository.cs
│   └── Services/
│       ├── IFavoriteSourceCommandService.cs      → ITuEntidadCommandService.cs
│       └── IFavoriteSourceQueryService.cs        → ITuEntidadQueryService.cs
├── Infrastructure/
│   └── Persistence/
│       └── EFC/
│           └── Repositories/
│               └── FavoriteSourceRepository.cs   → TuEntidadRepository.cs
└── Interfaces/
    └── REST/
        ├── FavoriteSourcesController.cs          → TuEntidadController.cs
        ├── Resources/
        │   ├── CreateFavoriteSourceResource.cs   → CreateTuEntidadResource.cs
        │   └── FavoriteSourceResource.cs         → TuEntidadResource.cs
        └── Transform/
            ├── CreateFavoriteSourceCommandFromResourceAssembler.cs  → CreateTuEntidadCommandFromResourceAssembler.cs
            └── FavoriteSourceResourceFromEntityAssembler.cs         → TuEntidadResourceFromEntityAssembler.cs
```
**Pasos para crear un nuevo bounded context:**
1. Copiar toda la carpeta `News/`
2. Renombrar `News/` a `[TuBoundedContext]/`
3. Renombrar todos los archivos dentro (reemplazar `FavoriteSource` por `TuEntidad`)
4. Actualizar namespaces en todos los archivos
5. Modificar el contenido según tu dominio

El proyecto sigue una arquitectura DDD con las siguientes capas:

```
[nombreProyecto]/
├── [BoundedContext]/                    # Ej: News, Users, Products, etc.
│   ├── Application/                     # Capa de Aplicación
│   │   └── Internal/
│   │       ├── CommandServices/        # Servicios de comandos (CQRS)
│   │       └── QueryServices/          # Servicios de consultas (CQRS)
│   ├── Domain/                          # Capa de Dominio
│   │   ├── Model/
│   │   │   ├── Aggregates/             # Agregados (entidades raíz)
│   │   │   ├── Commands/               # Comandos (record)
│   │   │   └── Queries/                # Queries (record)
│   │   ├── Repositories/               # Interfaces de repositorios
│   │   └── Services/                   # Interfaces de servicios de dominio
│   ├── Infrastructure/                  # Capa de Infraestructura
│   │   └── Persistence/
│   │       └── EFC/
│   │           └── Repositories/       # Implementaciones de repositorios
│   └── Interfaces/                      # Capa de Interfaz
│       └── REST/
│           ├── [Entity]Controller.cs    # Controladores REST
│           ├── Resources/               # DTOs de entrada/salida (record)
│           └── Transform/              # Assemblers (mapeadores)
└── Shared/                              # Contexto compartido
    ├── Domain/
    │   └── Repositories/               # Interfaces base compartidas
    └── Infrastructure/
        ├── Interfaces/
        │   └── ASP/
        │       └── Configuration/      # Configuraciones ASP.NET Core
        └── Persistence/
            └── EFC/
                ├── Configuration/      # DbContext y configuraciones EF
                └── Repositories/       # Implementaciones base compartidas
```
## Patrón de Transformación de Objetos (Record Pattern)

El proyecto implementa un patrón de transformación de objetos que sigue el flujo:

### Flujo POST (Crear):
```
1. Create[Entity]Resource (2 campos o los que se pida) 
   ↓ [Assembler]
2. Create[Entity]Command (2 campos o los que se pida)
   ↓ [CommandService]
3. [Entity] Aggregate (5 campos o los que se pida: Id, campos del command, CreatedDate, UpdatedDate)
   ↓ [Assembler]
4. [Entity]Resource (3 campos o los que se pida: Id + campos principales)
```

**Ejemplo concreto:**
- `CreateFavoriteSourceResource` (2 campos: `NewsApiKey`, `SourceId`)
- `CreateFavoriteSourceCommand` (2 campos: `NewsApiKey`, `SourceId`)
- `FavoriteSource` Entity (5 campos: `Id`, `NewsApiKey`, `SourceId`, `CreatedDate`, `UpdatedDate`)
- `FavoriteSourceResource` (3 campos: `Id`, `NewsApiKey`, `SourceId`)

### Flujo GET (Consultar):
```
1. [Entity] Aggregate (5 campos o los que se pida)
   ↓ [Assembler]
2. [Entity]Resource (3 campos o los que se pida: Id + campos principales)
```

## 📝 Validaciones

Las validaciones se implementan en **múltiples capas** para garantizar la integridad de los datos:

### 1. **Capa de Interfaz (Resources)**
```csharp
public record CreateFavoriteSourceResource(
    [Required] string NewsApiKey, 
    [Required] string SourceId
);
```
- Validaciones con Data Annotations (`[Required]`, `[MaxLength]`, etc.)
- Validación automática con `ModelState.IsValid` en el controlador

### 2. **Capa de Aplicación (CommandService)**
```csharp
// Validación de reglas de negocio (duplicados, etc.)
var favoriteSource = await repository.FindByNewsApiKeyAndSourceIdAsync(...);
if (favoriteSource != null)
    throw new Exception("Favorite source already exists");
```

### 3. **Capa de Dominio (Aggregate)**
- El constructor del agregado valida la creación del objeto
- Propiedades privadas con setters privados para encapsulación

## 🗄️ Configuración de Base de Datos

### Convenciones de Nomenclatura

- **Tablas**: Snake_case + Plural (ej: `favorite_sources`)
- **Columnas**: Snake_case (ej: `news_api_key`, `source_id`)
- **Claves**: Snake_case
- **Índices**: Snake_case

Para implementar relaciones padre-hijo y herencia con discriminador:

#### 1. **Relación Padre-Hijo (One-to-Many)**

```csharp
// En AppDbContext.OnModelCreating
builder.Entity<ParentEntity>()
    .HasMany(p => p.Children)
    .WithOne(c => c.Parent)
    .HasForeignKey(c => c.ParentId)
    .OnDelete(DeleteBehavior.Cascade);
```

#### 2. **Herencia con Discriminador (Table-Per-Hierarchy)**

```csharp
// Clase base
public abstract class BaseEntity
{
    public int Id { get; set; }
    public string Discriminator { get; set; }
}

// Clases derivadas
public class ChildEntity1 : BaseEntity { }
public class ChildEntity2 : BaseEntity { }

// Configuración en AppDbContext
builder.Entity<BaseEntity>()
    .HasDiscriminator<string>("discriminator")
    .HasValue<ChildEntity1>("ChildEntity1")
    .HasValue<ChildEntity2>("ChildEntity2");
```

#### 3. **Herencia con Tabla Separada (Table-Per-Type)**

```csharp
builder.Entity<BaseEntity>()
    .ToTable("base_entities");

builder.Entity<ChildEntity1>()
    .ToTable("child_entity1s")
    .HasBaseType<BaseEntity>();
```
## 📚 Guía de Uso como Plantilla

### Paso 1: Crear un Nuevo Bounded Context

1. Crear la carpeta `[BoundedContext]/` en la raíz del proyecto
2. Crear las subcarpetas según la estructura DDD

### Paso 2: Crear el Agregado (Aggregate)

```csharp
// Domain/Model/Aggregates/[Entity].cs
public partial class [Entity]
{
    protected [Entity]() { }
    
    public [Entity](Create[Entity]Command command)
    {
        // Inicializar propiedades desde el command
    }
    
    public int Id { get; }
    // Propiedades del dominio
}

// Domain/Model/Aggregates/[Entity]Audit.cs (partial)
public partial class [Entity] : IEntityWithCreatedUpdatedDate
{
    [Column("CreatedAt")] 
    public DateTimeOffset? CreatedDate { get; set; }
    
    [Column("UpdatedAt")] 
    public DateTimeOffset? UpdatedDate { get; set; }
}
```

### Paso 3: Crear Commands y Queries

```csharp
// Domain/Model/Commands/Create[Entity]Command.cs
public record Create[Entity]Command(string Field1, string Field2);

// Domain/Model/Queries/Get[Entity]ByIdQuery.cs
public record Get[Entity]ByIdQuery(int Id);
```

### Paso 4: Crear Resources (DTOs)

```csharp
// Interfaces/REST/Resources/Create[Entity]Resource.cs
public record Create[Entity]Resource(
    [Required] string Field1, 
    [Required] string Field2
);

// Interfaces/REST/Resources/[Entity]Resource.cs
public record [Entity]Resource(int Id, string Field1, string Field2);
```

### Paso 5: Crear Assemblers (Mappers)

```csharp
// Interfaces/REST/Transform/Create[Entity]CommandFromResourceAssembler.cs
public static class Create[Entity]CommandFromResourceAssembler
{
    public static Create[Entity]Command ToCommandFromResource(
        Create[Entity]Resource resource) =>
        new Create[Entity]Command(resource.Field1, resource.Field2);
}

// Interfaces/REST/Transform/[Entity]ResourceFromEntityAssembler.cs
public static class [Entity]ResourceFromEntityAssembler
{
    public static [Entity]Resource ToResourceFromEntity([Entity] entity) =>
        new [Entity]Resource(entity.Id, entity.Field1, entity.Field2);
}
```

### Paso 6: Crear Repositorio

```csharp
// Domain/Repositories/I[Entity]Repository.cs
public interface I[Entity]Repository : IBaseRepository<[Entity]>
{
    Task<[Entity]?> FindByFieldAsync(string field);
}

// Infrastructure/Persistence/EFC/Repositories/[Entity]Repository.cs
public class [Entity]Repository(AppDbContext context)
    : BaseRepository<[Entity]>(context), I[Entity]Repository
{
    public async Task<[Entity]?> FindByFieldAsync(string field)
    {
        return await Context.Set<[Entity]>()
            .FirstOrDefaultAsync(e => e.Field == field);
    }
}
```

### Paso 7: Crear Servicios

```csharp
// Domain/Services/I[Entity]CommandService.cs
public interface I[Entity]CommandService
{
    Task<[Entity]?> Handle(Create[Entity]Command command);
}

// Application/Internal/CommandServices/[Entity]CommandService.cs
public class [Entity]CommandService(
    I[Entity]Repository repository, 
    IUnitOfWork unitOfWork) : I[Entity]CommandService
{
    public async Task<[Entity]?> Handle(Create[Entity]Command command)
    {
        // Validaciones de negocio
        var entity = await repository.FindByFieldAsync(...);
        if (entity != null)
            throw new Exception("Entity already exists");
        
        // Crear entidad
        entity = new [Entity](command);
        
        // Persistir
        await repository.AddAsync(entity);
        await unitOfWork.CompleteAsync();
        
        return entity;
    }
}
```

### Paso 8: Crear Controlador

```csharp
// Interfaces/REST/[Entity]Controller.cs
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("[Entity]s")]
public class [Entity]Controller(
    I[Entity]CommandService commandService,
    I[Entity]QueryService queryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a [entity]")]
    [SwaggerResponse(201, "Created", typeof([Entity]Resource))]
    [SwaggerResponse(400, "Bad Request")]
    [SwaggerResponse(409, "Conflict")]
    public async Task<ActionResult> Create[Entity](
        [FromBody] Create[Entity]Resource resource)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var command = Create[Entity]CommandFromResourceAssembler
            .ToCommandFromResource(resource);
        
        try
        {
            var result = await commandService.Handle(command);
            if (result is null) 
                return Conflict(localizer["[Entity]Duplicated"].Value);
            
            return CreatedAtAction(
                nameof(Get[Entity]ById), 
                new { id = result.Id }, 
                [Entity]ResourceFromEntityAssembler.ToResourceFromEntity(result));
        }
        catch (Exception ex) when (ex.Message.Contains("already exists"))
        {
            return Conflict(localizer["[Entity]Duplicated"].Value);
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Gets a [entity] by id")]
    [SwaggerResponse(200, "Found", typeof([Entity]Resource))]
    public async Task<ActionResult> Get[Entity]ById(int id)
    {
        var query = new Get[Entity]ByIdQuery(id);
        var result = await queryService.Handle(query);
        if (result is null) return NotFound();
        
        var resource = [Entity]ResourceFromEntityAssembler
            .ToResourceFromEntity(result);
        return Ok(resource);
    }
}
```

### Paso 9: Registrar en AppDbContext

```csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);
    
    builder.Entity<[Entity]>().HasKey(e => e.Id);
    builder.Entity<[Entity]>().Property(e => e.Id)
        .IsRequired().ValueGeneratedOnAdd();
    // Configuraciones adicionales...
    
    builder.UseSnakeCaseNamingConvention();
}
```

### Paso 10: Registrar en Program.cs

```csharp
// Dependency Injection
builder.Services.AddScoped<I[Entity]Repository, [Entity]Repository>();
builder.Services.AddScoped<I[Entity]CommandService, [Entity]CommandService>();
builder.Services.AddScoped<I[Entity]QueryService, [Entity]QueryService>();
```