[AttributeUsage(AttributeTargets.Class)]
public class GeneratedFiles(params GeneratedType[] generations) : Attribute
{
    public GeneratedType[] Generations { get; } = generations;
}

[AttributeUsage(AttributeTargets.Class)]
public class SkipGeneration : Attribute
{ }

/// <summary>
/// EntityView.cs file/// <summary>
/// Represents the types of code artifacts that can be generated for a class.
/// </summary>
public enum GeneratedType
{
    /// <summary>
    /// Generates the EntityView.cs file.
    /// </summary>
    View,

    /// <summary>
    /// Generates command classes (e.g., CreateCommand, UpdateCommand).
    /// </summary>
    Commands,

    /// <summary>
    /// Generates an interface definition (e.g., IEntityService).
    /// </summary>
    Interface,

    /// <summary>
    /// Generates a data mapper (e.g., EntityMapper.cs).
    /// </summary>
    Mapper,

    /// <summary>
    /// Generates the service class implementation.
    /// </summary>
    Service,

    /// <summary>
    /// Generates the Razor page and backing C# code for a form view.
    /// </summary>
    Form,

    /// <summary>
    /// Generates the Razor page and C# class for creating a new entity.
    /// </summary>
    Create,

    /// <summary>
    /// Generates the Razor page and C# class for updating an existing entity.
    /// </summary>
    Update,

    /// <summary>
    /// Generates the Razor page and C# class for listing entities.
    /// </summary>
    List,
}