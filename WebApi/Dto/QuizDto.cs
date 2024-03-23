using System.ComponentModel.DataAnnotations;
using ApplicationCore.Models.QuizAggregate;

namespace WebApi.Dto;

public class QuizDto
{   
    [Required]
    [Length(minimumLength: 3, maximumLength: 200)]
    public string Title { get; set; }
    
    

    public static Quiz ToQuiz(QuizDto dto)
    {
        return new Quiz()
        {
            Title = dto.Title
        };
    }
}