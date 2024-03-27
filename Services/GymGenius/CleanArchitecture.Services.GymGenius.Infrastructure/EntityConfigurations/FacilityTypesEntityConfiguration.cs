using CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Enumerations;

namespace CleanArchitecture.Services.GymGenius.Infrastructure.EntityConfigurations;

internal class FacilityTypesEntityConfiguration : IEntityTypeConfiguration<FacilityType>
{
    public void Configure(EntityTypeBuilder<FacilityType> builder)
    {
        builder.ToTable(nameof(FacilityType).Humanize().Pluralize().Pascalize(), GymGeniusDbContext.DEFAULT_SCHEMA);
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
