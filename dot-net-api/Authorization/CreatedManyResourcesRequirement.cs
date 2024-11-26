using Microsoft.AspNetCore.Authorization;

namespace dot_net_api.Authorization;

public class CreatedManyResourcesRequirement: IAuthorizationRequirement
{
    public int MinimumAmountRequired { get; }

    public CreatedManyResourcesRequirement(int minimumAmountRequired)
    {
        MinimumAmountRequired = minimumAmountRequired;
    }
    
}