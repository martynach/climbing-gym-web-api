﻿using Microsoft.AspNetCore.Authorization;

namespace dot_net_api.Authorization;

public class MinimumAgeRequirement: IAuthorizationRequirement
{
    
    public int MinimumAge { get; }

    public MinimumAgeRequirement(int minimumAge)
    {
        MinimumAge = minimumAge;
    }
    
}