using DemoSocial.Domain.Exceptions;
using DemoSocial.Domain.Validators.UserProfileValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Domain.Aggregates.UserProfileAggregate;

public class BasicInfo
{
    private BasicInfo()
    {
    }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string CurrentCity { get; private set; }

    public static BasicInfo CreateBasicInfo(
        string firstName, string lastName, string email,
        string phone, DateTime dateOfBirth, string currentCity)
    {
        BasicInfoValidator validator = new();

        BasicInfo objectToValidate = new()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
            DateOfBirth = dateOfBirth,
            CurrentCity = currentCity
        };

        var validationResult = validator.Validate(objectToValidate);

        if (validationResult.IsValid) return objectToValidate;

        var exception = new UserProfileNotValidException("The user profile is not valid.");
        foreach (var error in validationResult.Errors)
        {
            exception.ValidationErrors.Add(error.ErrorMessage);
        }

        throw exception;
    }

        // public methods


    

}
