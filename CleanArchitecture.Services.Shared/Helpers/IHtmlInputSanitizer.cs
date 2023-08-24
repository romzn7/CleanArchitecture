using Ganss.Xss;

namespace CleanArchitecture.Services.Shared.Helpers;

public interface IHtmlInputSanitizer
{
    string RemoveHtmlElemets(string? html);
    string SantizeHtml(string html);
}

internal class HtmlInputSanitizer : IHtmlInputSanitizer
{
    private readonly HtmlSanitizer _htmlSanitizer = new();
    private readonly HtmlSanitizer _htmlTagRemover;

    public HtmlInputSanitizer()
    {
        var options = new HtmlSanitizerOptions();
        options.AllowedTags.Add($"thisisarediculustagthatnooneshouldbeabletoguessASDFGHJK12345678{Guid.NewGuid().ToString().Replace("-", "")}");
        //workaround to remove all tags
        _htmlTagRemover = new HtmlSanitizer(options);

        //To retain the text inside the htmltags
        _htmlTagRemover.KeepChildNodes = true;
    }

    public string RemoveHtmlElemets(string? html) => html != null ? _htmlTagRemover.Sanitize(html) : html;

    public string SantizeHtml(string html) => _htmlSanitizer.Sanitize(html);
}
