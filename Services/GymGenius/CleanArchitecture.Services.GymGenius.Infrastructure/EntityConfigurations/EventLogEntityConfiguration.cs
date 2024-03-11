﻿namespace CleanArchitecture.Services.GymGenius.Infrastructure.EntityConfigurations;
internal class EventLogEntityConfiguration : IEntityTypeConfiguration<EventLog>
{
    public void Configure(EntityTypeBuilder<EventLog> builder)
    {
        builder.ToTable(nameof(EventLog).Humanize().Pluralize().Pascalize(), GymGeniusDbContext.DEFAULT_SCHEMA);
        builder.HasKey(x => x.Id);
        builder.Property(o => o.Id)
             .UseHiLo("eventlogseq", GymGeniusDbContext.DEFAULT_SCHEMA);
        builder.Ignore(x => x.DomainEvents);

        builder.Property(x => x.EventLogGUID)
            .IsRequired(true);

        builder.HasIndex(x => x.EventLogGUID)
            .IsUnique(true);

        builder.Property<int>("EventTypeId")
            .IsRequired(true);

        builder.HasOne(p => p.EventType)
            .WithMany()
            .HasForeignKey("EventTypeId")
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.Description)
            .HasMaxLength(1000)
            .IsRequired(true);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired(true);
    }
}
