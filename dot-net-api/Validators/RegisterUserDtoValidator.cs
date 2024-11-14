using dot_net_api.Dtos;
using dot_net_api.Entities;
using FluentValidation;

namespace dot_net_api.Validators;

public class RegisterUserDtoValidator: AbstractValidator<RegisterUserDto>
{
    private readonly ClimbingGymDbContext _dbContext;

    public RegisterUserDtoValidator(ClimbingGymDbContext dbContext)
    {
        _dbContext = dbContext;
 
        RuleFor(dto => dto.FirstName)
            .NotNull()
            .MinimumLength(ValidationConstants.MinLength)
            .MaximumLength(ValidationConstants.ShortStringMaxLength);
        
        RuleFor(dto => dto.LastName)
            .NotNull()
            .MinimumLength(ValidationConstants.MinLength)
            .MaximumLength(ValidationConstants.ShortStringMaxLength);

        RuleFor(dto => dto.ConfirmEmail)
            .NotNull()
            .EmailAddress();

        RuleFor(dto => dto.Email)
            .NotNull()
            .Equal(dto => dto.ConfirmEmail).WithMessage("Email and ConfirmEmail are not equal")
            .Custom((email, context) =>
            {
                var emailExists = _dbContext.Users.Any(u => u.Email.ToLower().Equals(email.ToLower()));
                if (emailExists)
                {
                    context.AddFailure("User with provided email already exists");
                }
            });

        RuleFor(dto => dto.ConfirmPassword)
            .NotNull()
            .MinimumLength(ValidationConstants.PasswordMinLength)
            .MaximumLength(ValidationConstants.PasswordMaxLength);

        RuleFor(dto => dto.Password)
            .NotNull()
            .Equal(dto => dto.ConfirmPassword);

        RuleFor(dto => dto.RoleId)
            .NotNull()
            .NotEmpty()
            // .Must(roleId => roleId == 1 || roleId == 2 || roleId == 3)
            .InclusiveBetween(1,3)
            .WithMessage("Role id is required and needs to be 1 for user, 2 for manager or 3 for admin");

    }
    
}