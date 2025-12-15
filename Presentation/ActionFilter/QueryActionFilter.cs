using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.ActionFilter
{
    public class QueryActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.Count == 0) return;

          var queryStringParameter=
                context.ActionArguments.SingleOrDefault(x => x.Key == "q");

            if (queryStringParameter.Value == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            if(queryStringParameter.Value.ToString().Trim() == "")
            {
                context.Result = new ForbidResult();
                return;
            }



        }
    }
}
