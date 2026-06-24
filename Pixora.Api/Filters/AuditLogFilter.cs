using Microsoft.AspNetCore.Mvc.Filters;
using Pixora.BL.Services.Logs;
using Pixora.DAL.Models;
using System.Security.Claims;

namespace Pixora.Api.Filters
{
    public class AuditLogFilter : IActionFilter
    {
        private readonly IUserActionLogService _logService;

        public AuditLogFilter(IUserActionLogService logService)
        {
            _logService = logService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                return;
            }

            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            var actionType = GetActionType(controller, action, context);

            if (actionType == null)
            {
                return;
            }

            _logService.Log(userId, actionType.Value, $"{controller} {action?.ToLower()} was executed.");
        }

        private static UserActionType? GetActionType(string? controller, string? action, ActionExecutedContext context)
        {
            return (controller, action) switch
            {
                ("Photos", "Upload") => UserActionType.UploadedPhoto,
                ("Photos", "EditMetadata") => UserActionType.EditedPhoto,
                ("Photos", "Download") => UserActionType.DownloadedPhoto,

                ("Photos", "Delete") => context.HttpContext.User.IsInRole("Admin")
                    ? UserActionType.AdminDeletedPhoto
                    : UserActionType.DeletedPhoto,

                ("User", "ChangePlan") => UserActionType.ChangedPlan,

                ("Admin", "UpdateUser") => UserActionType.AdminUpdatedUser,
                ("Admin", "DeletePhoto") => UserActionType.AdminDeletedPhoto,

                _ => null
            };
        }
    }
}