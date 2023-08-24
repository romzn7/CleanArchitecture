namespace CleanArchitecture.Services.Person.Infrastructure.EntityConfigurations;
internal class GenderEntityConfiguration : IEntityTypeConfiguration<Gender>
{
    public void Configure(EntityTypeBuilder<Gender> builder)
    {
        builder.ToTable(nameof(Gender).Humanize().Pluralize().Pascalize(), PersonDbContext.DEFAULT_SCHEMA);
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