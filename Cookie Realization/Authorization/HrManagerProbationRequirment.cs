using Microsoft.AspNetCore.Authorization;

namespace Cookie_Realization.Authorization
{
    public class HrManagerProbationRequirment : IAuthorizationRequirement
    {
        public HrManagerProbationRequirment(int probationMonths)
        {
            probationMonths = ProbationMonths;
        }
        public int ProbationMonths { get; set; } 
        
    }

    public class HrManagerProbationRequiermentHandler : AuthorizationHandler<HrManagerProbationRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HrManagerProbationRequirment requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "EmploymentDate"))
                return Task.CompletedTask;

            if (DateTime.TryParse(context.User.FindFirst(x => x.Type == "EmploymentDate")?.Value, out DateTime ewmploymentDate))
            {
                var period = DateTime.Now - ewmploymentDate;
                if (period.Days > 30 * requirement.ProbationMonths)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;  
        }
    }
}
