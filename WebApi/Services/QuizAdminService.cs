using ApplicationCore.Commons.Repository;
using ApplicationCore.Models.QuizAggregate;
using Microsoft.AspNetCore.SignalR.Protocol;
using WebApi.Validators;

namespace WebApi.Services;

public class QuizAdminService(IGenericRepository<Quiz, int> quizRepo, IGenericRepository<QuizItem, int> itemRepo)
{
    private readonly IGenericRepository<Quiz, int> _quizRepo = quizRepo;
    private readonly IGenericRepository<QuizItem, int> _itemRepo = itemRepo;

    public Quiz AddQuiz(Quiz quiz)
    {
        return _quizRepo.Add(quiz);
    }

    public void AddItemToQuiz(int quizId, QuizItem item)
    {
        Quiz? quiz = _quizRepo.FindById(quizId);
        if (quiz is null)
        {
            return;
        }
        if (!item.IsValid())
        {
            return;
        }
        _itemRepo.Add(item);
        quiz.Items?.Add(item);
        _quizRepo.Update(quizId, quiz);
    }

    public void RemoveItemFromQuiz(int quizId, int itemId)
    {
        
    }

    public void UpdateQuizItem(int itemId, QuizItem item)
    {
        
    }
    
}