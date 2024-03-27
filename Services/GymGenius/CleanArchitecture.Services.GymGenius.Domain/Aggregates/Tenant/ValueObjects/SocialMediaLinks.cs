namespace CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.ValueObjects;

public class SocialMediaLinks : ValueObject
{
    public string WebsiteUrl { get; private set; }
    public string FacebookURL { get; private set; }
    public string TwitterURL { get; private set; }
    public string InstagramURL { get; private set; }
    public string TiktokURL { get; private set; }

    public SocialMediaLinks(string websiteUrl, string facebookUrl, string twitterUrl, string instagramUrl, string tiktokUrl)
    {
        WebsiteUrl = Guard.Against.NullOrEmpty(websiteUrl);
        FacebookURL = Guard.Against.NullOrEmpty(facebookUrl);
        TwitterURL = Guard.Against.NullOrEmpty(twitterUrl);
        InstagramURL = Guard.Against.NullOrEmpty(instagramUrl);
        TiktokURL = Guard.Against.NullOrEmpty(tiktokUrl);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return WebsiteUrl;
        yield return FacebookURL;
        yield return TwitterURL;
        yield return InstagramURL;
        yield return TiktokURL;
    }
}