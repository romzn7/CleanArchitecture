namespace CleanArchitecture.Services.Shared.Models.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class StripHtmlAttribute : Attribute
{
}
