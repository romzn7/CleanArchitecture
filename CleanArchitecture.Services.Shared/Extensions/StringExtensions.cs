using System.Xml.Serialization;
using System.Xml;

namespace CleanArchitecture.Services.Shared.Extensions;

public static class StringExtensions
{
    public static T ConvertXMLResponseToClass<T>(this string data)
        where T : new()
    {

        var serializer = new XmlSerializer(typeof(T));
        T result = new T();

        using var reader = new StringReader(data);
        result = (T)serializer.Deserialize(reader);
        return result;
    }

    public static T TryConvertXMLResponseToClass<T>(this string data)
        where T : new()
    {
        try
        {
            var serializer = new XmlSerializer(typeof(T));
            T result = new T();

            using var reader = new StringReader(data);
            using var xmlReader = XmlReader.Create(reader)!;
            result = serializer.CanDeserialize(xmlReader) ?
                (T)serializer.Deserialize(xmlReader) : default(T);
            return result;
        }
        catch (Exception)
        {
            return default(T);
        }
    }

    public static (string FirstName, string LastName) SplitFullName(this string fullName)
    {
        var names = fullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        if (names.Count > 1)
        {
            var firstName = string.Join(" ", names.Take(names.Count - 1));
            var lastName = names[names.Count - 1];

            return (firstName, lastName);
        }
        return (null, fullName);
    }
}

