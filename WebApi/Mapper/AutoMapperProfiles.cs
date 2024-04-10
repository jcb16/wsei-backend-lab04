using ApplicationCore.Models.QuizAggregate;
using AutoMapper;
using WebApi.Dto;

namespace WebApi.Mapper;

public class AutoMapperProfiles:Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<QuizItem, QuizItemDto>()
            .ForMember(
                q => q.Options,
                op => op.MapFrom(i => new List<string>(i.IncorrectAnswers) { i.CorrectAnswer }));
        CreateMap<Quiz, QuizDto>()
            .ForMember(
                q => q.Items,
                op => op.MapFrom<List<QuizItem>>(i => i.Items)
            );
        //CreateMap<NewQuizDto, Quiz>();
        CreateMap<NewQuizDto, Quiz>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Items, opt => opt.Ignore());

    }
}