using CleanArchitecture.Services.Person.Domain.Aggregates.Person.Enumerations;
using CleanArchitecture.Services.Person.Domain.Aggregates.Person.ValueObjects;
using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Services.Person.Infrastructure.EntityConfigurations;

internal class PersonEntityConfiguration : IEntityTypeConfiguration<Domain.Aggregates.Person.Entities.Person>
{
    public void Configure(EntityTypeBuilder<Domain.Aggregates.Person.Entities.Person> builder)
    {
        builder.ToTable(nameof(Domain.Aggregates.Person.Entities.Person).Humanize().Pluralize().Pascalize(), PersonDbContext.DEFAULT_SCHEMA);
        builder.HasKey(x => x.Id);
        builder.Property(o => o.Id)
             .UseHiLo("personseq", PersonDbContext.DEFAULT_SCHEMA);
        builder.Ignore(x => x.DomainEvents);

        builder.Property(x => x.PersonGuid)
            .IsRequired(true);

        builder.Property(x => x.Email)
            .IsRequired(true);

        builder.Property(x => x.Age)
            .IsRequired(true);

        builder.OwnsMany(x => x.Address, sa =>
            {
                sa.Property(x => x.WardNo).IsRequired(true);
                sa.Property(x => x.Location).HasMaxLength(200).IsRequired(true);
                sa.Property(x => x.City).HasMaxLength(200).IsRequired(true);
                sa.WithOwner().HasForeignKey("PersonId");
                sa.Property<long>("Id");
                sa.HasKey("Id");
                sa.ToTable(nameof(Address).Humanize().Pluralize().Pascalize(), PersonDbContext.DEFAULT_SCHEMA);
            });


        builder
           .Property<int>("GenderId")
           .HasDefaultValue(Gender.Male.Id)
           .IsRequired();

        builder.HasOne(p => p.Gender)
            .WithMany()
            .HasForeignKey("GenderId")
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired(true);
    }
}
