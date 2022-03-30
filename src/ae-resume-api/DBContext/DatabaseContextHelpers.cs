using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Data;
using System.Linq;

public static class DatabaseContextHelpers
{
    private static string GetName(IEntityType entityType, string defaultSchemaName = "ae-resume-db")
    {
        var schema = entityType.FindAnnotation("Relational:Schema").Value;
        string tableName = entityType.GetAnnotation("Relational:TableName").Value.ToString();
        string schemaName = schema == null ? defaultSchemaName : schema.ToString();
        string name = string.Format("[{0}].[{1}]", schemaName, tableName);
        return name;
    }

    public static string TableName<T>(DbContext dbContext) where T : class
    {
        var entityType = dbContext.Model.FindEntityType(typeof(T));
        return GetName(entityType);
    }

    public static string TableName<T>(DbSet<T> dbSet) where T : class
    {
        var entityType = dbSet.EntityType;
        return GetName(entityType);
    }

    public static string Truncate<T>(this DbSet<T> dbSet) where T : class
    {
        string cmd = $"TRUNCATE TABLE {TableName(dbSet)}";
        var context = dbSet.GetService<ICurrentDbContext>().Context;
        context.Database.ExecuteSqlRaw(cmd);
        return cmd;
    }
}