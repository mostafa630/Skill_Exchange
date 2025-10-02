using System;
using FluentValidation;
using Skill_Exchange.Application.DTOs.User;

namespace Skill_Exchange.Application.FluentValidation.User
{
    public class UpdateUserDTOValidator : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserDTOValidator()
        {
            // FirstName: validate only if it's not null and not whitespace
            RuleFor(x => x.FirstName)
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("First name must contain only letters.")
                .When(x => x.FirstName != null && !string.IsNullOrWhiteSpace(x.FirstName));

            // LastName: validate only if it's not null and not whitespace
            RuleFor(x => x.LastName)
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("Last name must contain only letters.")
                .When(x => x.LastName != null && !string.IsNullOrWhiteSpace(x.LastName));

            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("Email is required.")
               .EmailAddress().WithMessage("Invalid email format.");

            // DateOfBirth: validate only if it has a value
            RuleFor(x => x.DateOfBirth)
                .Must(BeAValidAge).WithMessage("User must be at least 13 years old.")
                .When(x => x.DateOfBirth.HasValue);
        }

        private bool BeAValidAge(DateOnly? dateOfBirth)
        {
            if (!dateOfBirth.HasValue)
                return true; // Accept null

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year - dateOfBirth.Value.Year;

            if (dateOfBirth.Value > today.AddYears(-age))
                age--;

            return age >= 13;
        }
    }
}
