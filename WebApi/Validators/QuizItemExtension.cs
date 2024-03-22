using ApplicationCore.Models.QuizAggregate;

namespace WebApi.Validators;

public static class QuizItemExtension
{
    public static bool IsValid(this QuizItem item)
    {
        if (String.IsNullOrEmpty(item.Question))
        {
            return false;
        }

        if (item.IncorrectAnswers.Count == 0)
        {
            return false;
        }

        if (String.IsNullOrEmpty(item.CorrectAnswer))
        {
            return false;
        }

        return true;
    }
}