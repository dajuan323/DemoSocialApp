using DemoSocial.Domain.Aggregates.PostAggregate;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Domain.Validators.PostValidators;

public class PostCommentValidator : AbstractValidator<PostComment>
{
    public PostCommentValidator()
    {
        RuleFor(pc => pc.Text)
            .NotNull().WithMessage("Comment text required")
            .NotEmpty().WithMessage("Comment text should not be empty")
            .MinimumLength(5).WithMessage("First name must be at least 5 characters long...")
            .MaximumLength(1000).WithMessage("First name must be lower than 1000 characters...");
    }

}
