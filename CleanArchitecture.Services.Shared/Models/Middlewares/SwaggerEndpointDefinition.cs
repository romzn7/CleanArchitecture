namespace CleanArchitecture.Services.Shared.Models.Middlewares;

public class SwaggerEndpointDefinition
{
    public SwaggerEndpointDefinition(string url, string name)
    {
        this.Url = url;
        this.Name = name;
    }
    public string Url { get; private set; }
    public string Name { get; private set; }
}