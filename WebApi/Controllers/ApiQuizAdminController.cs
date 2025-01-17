using ApplicationCore.Interfaces.AdminService;
using ApplicationCore.Models.QuizAggregate;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using WebApi.Validators;
using FluentValidation;

namespace WebApi.Controllers;

public class ApiQuizAdminController : Controller
{
    private IQuizAdminService _service;
    private readonly IMapper _mapper;
    private readonly IValidator<QuizItem> _quizItemValidator;

    public ApiQuizAdminController(IQuizAdminService service, IMapper mapper, IValidator<QuizItem> quizItemValidator)
    {
        _service = service;
        _mapper = mapper;
        _quizItemValidator = quizItemValidator;
    }


    [HttpPost]
    public ActionResult<object> AddQuiz(LinkGenerator link, NewQuizDto dto)
    {
        //var quiz = _service.AddQuiz(new Quiz() {Title = dto.Title});
        var quiz = _service.AddQuiz(_mapper.Map<Quiz>(dto));
        return Created(
            link.GetPathByAction(
                HttpContext, 
                nameof(GetQuiz),         // nazwa metody kontrolera zwracająca quiz
                null,                    // kontroler, null oznacza bieżący
                new { quiId = quiz.Id }),// parametry ścieżki, id utworzonego quiz
            quiz
        );
    }

    [HttpGet]
    [Route("{quizId}")]
    public ActionResult<Quiz> GetQuiz(int quizId)
    {
        var quiz = _service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
        return quiz is null ? NotFound() : quiz;
    }
    
    
    [HttpPatch]
    [Route("{quizId}")]
    [Consumes("application/json-patch+json")]
    public ActionResult<Quiz> AddQuizItem(int quizId, JsonPatchDocument<Quiz>? patchDoc)
    {
        var quiz = _service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
        if (quiz is null || patchDoc is null)
        {
            return NotFound(new
            {
                error = $"Quiz width id {quizId} not found"
            });
        }
        int previousCount = quiz.Items.Count;
        patchDoc.ApplyTo(quiz, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        foreach (var item in patchDoc.Operations)
        {
            if (item.path.EndsWith("/Question"))
            {
                var question = item.value?.ToString();
                var quizItem = new QuizItem { Question = question };
                var validationResult = _quizItemValidator.Validate(quizItem);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
            }
        }

        //{
        //    "op": "add",
        //"path": "/Items",
        //"value": {
        //        "Question": "Nowe pytanie",
        //    "CorrectAnswer": "Poprawna odpowiedź",
        //    "IncorrectAnswers": ["Niepoprawna odpowiedź 1", "Niepoprawna odpowiedź 2"]
        //}

        if (previousCount < quiz.Items.Count)
        {
            QuizItem item = quiz.Items[^1];
            quiz.Items.RemoveAt(quiz.Items.Count - 1);
            _service.AddQuizItemToQuiz(quizId, item);
        }
        return Ok(_service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId));
    }

}