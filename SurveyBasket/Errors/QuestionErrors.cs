namespace SurveyBasket.Errors;

public static class QuestionErrors
{
    public static readonly Error QuestionNotFound = new("Question.NotFound", "Question With Given Id is not found.");

    public static readonly Error DuplicatedQuestionContent = new("Question.DuplicatedContent", "Another Question With The Same Content Exists.");
        }
