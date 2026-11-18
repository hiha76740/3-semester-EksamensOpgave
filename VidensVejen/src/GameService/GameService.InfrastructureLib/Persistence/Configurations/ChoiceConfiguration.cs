using GameService.DomainLib.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameService.InfrastructureLib.Persistence.Configurations;

internal sealed class ChoiceConfiguration : IEntityTypeConfiguration<Choice>
{
    void IEntityTypeConfiguration<Choice>.Configure(EntityTypeBuilder<Choice> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            id => id.Value,
            value => new ChoiceId(value));

        builder.Property<QuestionId>("QuestionId")
            .HasConversion(
            id => id.Value,
            value => new QuestionId(value));

        builder.Property(x => x.Text)
            .HasMaxLength(500)
            .IsRequired();
    }
}
