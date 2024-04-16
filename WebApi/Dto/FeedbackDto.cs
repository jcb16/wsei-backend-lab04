using ApplicationCore.Models;
using ApplicationCore.Models.QuizAggregate;
using AutoMapper;
using WebApi.Controllers;

namespace WebApi.Dto;

public class FeedbackDto
{
    public int QuizId { get; set; }
    public int UserId { get; set; }
    public int TotalQuestions { get; set; }
    public IEnumerable<AnswerDto> Answers { get; set; }
}

public class AnswerDto
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public bool IsCorrect { get; set; }
}

public class MappingProfile : Profile
{
    //public MappingProfile()
    //{
    //    CreateMap<QuizItemUserAnswer, AnswerDto>()
    //        .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.QuizItem.Question))
    //        .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer))
    //        .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect()));

    //    CreateMap<IEnumerable<QuizItemUserAnswer>, FeedbackDto>()
    //        .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => src.FirstOrDefault()?.QuizId))
    //        .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.FirstOrDefault()?.UserId))
    //        .ForMember(dest => dest.TotalQuestions, opt => opt.MapFrom(src => src.FirstOrDefault()?.TotalQuestions))
    //        .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<AnswerDto>>(src)));
    //}
}