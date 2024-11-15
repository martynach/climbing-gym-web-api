using dot_net_api.Dtos;
using dot_net_api.Entities;
using FluentValidation;

namespace dot_net_api.Validators;

public class GetAllQueryValidator: AbstractValidator<GetAllQuery>
{

    public GetAllQueryValidator()
    {
        RuleFor(q => q.PageSize)
            .Must(pageSize => pageSize <= 5 || pageSize == 10 || pageSize == 15)
            .WithMessage("PageSize must be less or equal to 5 or equal to 10 or 15");

        RuleFor(q => q.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.SortBy)
            .Must(sortBy => sortBy == null
                            || sortBy == nameof(ClimbingGym.Name)
                            || sortBy == nameof(ClimbingGym.Description)
                            || sortBy == nameof(ClimbingGym.Address.City)
                            || sortBy == nameof(ClimbingGym.Address.Street))
            .WithMessage($"sortBy must be equal to '{nameof(ClimbingGym.Name)}'" +
                         $"or '{nameof(ClimbingGym.Description)}'" +
                         $"or '{nameof(ClimbingGym.Address.City)}'" +
                         $"or '{nameof(ClimbingGym.Address.Street)}'");

        RuleFor(q => q.SortDirection)
            .Must(sortDirection => String.IsNullOrWhiteSpace(sortDirection) || sortDirection.ToUpper() == SortDirection.ASC.ToString() || sortDirection.ToUpper() == SortDirection.DESC.ToString())
            .WithMessage($"sortDirection must be empty or equal to '{SortDirection.ASC}' or '{SortDirection.DESC}'");
        
        RuleFor(q => q.SearchBy)
            .MinimumLength(3)
            .MaximumLength(30);
    }
    
}