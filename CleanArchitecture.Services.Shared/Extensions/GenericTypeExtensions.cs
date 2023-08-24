namespace CleanArchitecture.Services.Shared.Extensions;

public static class GenericTypeExtensions
{
    public static string GetGenericTypeName(this Type type)
    {
        var typeName = type.FullName ?? type.Name;

        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            typeName = $"{typeName.Remove(typeName.IndexOf('`'))}<{genericTypes}>";
        }

        return typeName;
    }

    public static string GetGenericTypeName(this object @object) => @object.GetType().GetGenericTypeName();
}