using Bumbleberry.QMeService.Helper;
using System;

namespace QMeFrontend.Helper
{
    public static class ErrorHelper
    {
        public static string SetUserFriendlyErrorMessage(string errorMessage)
        {
            var userfriendlyMessage = "";
            if (string.IsNullOrWhiteSpace(errorMessage))
                return userfriendlyMessage;

            userfriendlyMessage = $"{Constants.USERFRIENDLYERRORMESSAGE}{errorMessage}{Constants.USERFRIENDLYERRORMESSAGE}";
            return userfriendlyMessage;
        }

        public static string SetSystemErrorMessage(string errorMessage)
        {
            var systemErrorMessage = "";
            if (string.IsNullOrWhiteSpace(errorMessage))
                return systemErrorMessage;

            systemErrorMessage = $"{Constants.SYSTEMERRORMESSAGE}{errorMessage}{Constants.SYSTEMERRORMESSAGE}";
            return systemErrorMessage;
        }
    }
}
