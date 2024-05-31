using FluentValidation;
using ParsingProject.DTO;

namespace ParsingProject.Validators;

public class SaveChannelDtoValidator : AbstractValidator<SaveChannelDto>
{
    public SaveChannelDtoValidator()
    {
        RuleFor(c => c.ChannelLink)
            .NotEmpty()
            .Must(l => l.StartsWith("https://t.me/"))
            .WithMessage("Invalid link on telegram channel");
    }
}