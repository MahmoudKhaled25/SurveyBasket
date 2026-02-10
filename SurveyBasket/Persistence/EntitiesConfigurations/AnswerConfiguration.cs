
namespace SurveyBasket.Persistence.EntitiesConfigurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.Property(x => x.Content).HasMaxLength(1000);
        builder.HasIndex(x => new { x.QuestionId, x.Content }).IsUnique();
    }
}
