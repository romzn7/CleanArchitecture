namespace CleanArchitecture.Services.Shared.Extensions;

public static class EnumerableExtension
{
    public static T GetRandomEntry<T>(this IEnumerable<T> enumeration)
    {
        if (!enumeration.Any()) return default(T);

        Random rnd = new Random();
        var arr = enumeration.ToArray();
        int index = rnd.Next(arr.Length);
        return arr[index];
    }
}
