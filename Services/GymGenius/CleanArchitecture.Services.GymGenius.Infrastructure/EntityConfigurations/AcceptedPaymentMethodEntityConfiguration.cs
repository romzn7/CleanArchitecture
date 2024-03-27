using CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Enumerations;

namespace CleanArchitecture.Services.GymGenius.Infrastructure.EntityConfigurations;

internal class AcceptedPaymentMethodEntityConfiguration : IEntityTypeConfiguration<AcceptedPaymentMethod>
{
    public void Configure(EntityTypeBuilder<AcceptedPaymentMethod> builder)
    {
        builder.ToTable(nameof(AcceptedPaymentMethod).Humanize().Pluralize().Pascalize(), GymGeniusDbContext.DEFAULT_SCHEMA);
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
