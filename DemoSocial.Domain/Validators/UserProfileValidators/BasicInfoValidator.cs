using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Domain.Validators.UserProfileValidators;

public class BasicInfoValidator : AbstractValidator<BasicInfo>
{
    public BasicInfoValidator() 
    {
        RuleFor(info => info.FirstName)
            .NotNull().WithMessage("First name required")
            .MinimumLength(3).WithMessage("First name must be at least 3 characters long...")
            .MaximumLength(50).WithMessage("First name must be lower than 50 characters...");
        RuleFor(info => info.LastName)
            .NotNull().WithMessage("Last name required...")
            .MinimumLength(3).WithMessage("Last name must be at least 3 characters long...")
            .MaximumLength(50).WithMessage("Last name must be lower than 50 characters...");
        RuleFor(info => info.Email)
            .NotNull().WithMessage("Email required...")
            .EmailAddress().WithMessage("Provided string is not a currect email address format");
        //RuleFor(info => info.DateOfBirth)
        //    .InclusiveBetween(new DateTime(DateTime.Now.AddYears(-125).Ticks), 
        //         new DateTime(DateTime.Now.AddYears(-18).Ticks))
        //    .WithMessage("Age needs to be between 18 and 125");

    }
}
