using ApplicationCore.Interfaces.AdminService;
using ApplicationCore.Models.QuizAggregate;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/v1/admin/quizzes")]
public class ApiQuizAdminController : ControllerBase
{
    public readonly IQuizAdminService _service;

    public ApiQuizAdminController(IQuizAdminService service)
    {
        _service = service;
    }

    [HttpPost]
    public ActionResult<object> AddQuiz(LinkGenerator link, QuizDto dto)
    {
        var quiz = _service.AddQuiz(QuizDto.ToQuiz(dto));
        return Created(
            link.GetPathByAction(HttpContext, nameof(GetQuiz), null, new { quiId = quiz.Id }),
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
    public ActionResult<Quiz> AddQuizItem(int quizId, [FromBody] JsonPatchDocument<Quiz>? patchDoc)
    {
        var quiz = _service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
        if (quiz is null || patchDoc is null)
        {
            return BadRequest();
        }
        patchDoc.ApplyTo(quiz, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        _service.UpdateQuiz(quiz);
        return Ok(quiz);
    }
}