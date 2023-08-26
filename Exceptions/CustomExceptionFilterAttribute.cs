using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ArtisanELearningSystem.Exceptions
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is UserNotFoundException)
            {
                // Handle the UserNotFoundException by signing out the user and redirecting to the error page
                context.Result = new RedirectToActionResult("Logout", "Home", null);
                context.ExceptionHandled = true;
            }
           /* else
            {
                // Handle other exceptions here (if needed) and redirect to the error page
                context.Result = new RedirectToActionResult("Error", "Home", null);
                context.ExceptionHandled = true;
            }*/
        }
    }
}
