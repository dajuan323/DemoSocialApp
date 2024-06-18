using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.UserProfiles;

public record UserProfileErrorMessages
{
    public string UserProfileNotFound = "No UserProfile found with ID: {0}";
}
