using FluentValidation;
using ParsingProject.DTO;

namespace ParsingProject.Validators;

public class ChannelsParsingDtoValidator : AbstractValidator<ChannelsParsingDto>
{
    public ChannelsParsingDtoValidator()
    {
        RuleFor(c => c.ParsingDate)
            .LessThan(DateTime.Now)
            .WithMessage("Messages can't be parsed from future");
    }
}