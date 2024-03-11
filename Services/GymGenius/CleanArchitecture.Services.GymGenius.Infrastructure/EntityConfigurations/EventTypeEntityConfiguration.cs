using CleanArchitecture.Services.GymGenius.Infrastructure;

namespace CleanArchitecture.Services.Person.Infrastructure.EntityConfigurations;
internal class EventTypeEntityConfiguration : IEntityTypeConfiguration<EventType>
{
    public void Configure(EntityTypeBuilder<EventType> builder)
    {
        builder.ToTable(nameof(EventType).Humanize().Pluralize().Pascalize(), GymGeniusDbContext.DEFAULT_SCHEMA);
        builder.HasKey(x => x.Id);

        builder.Property(ct => ct.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(ct => ct.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
