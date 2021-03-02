
namespace Bumbleberry.QMeService.Helper
{
    public static class Constants
    {
        public const string SYSTEM_USER = "SYSTEM_USER";
        public const string USERFRIENDLYERRORMESSAGE = "UFEM";
        public const string SYSTEMERRORMESSAGE = "SEM";
    }

    public enum StatusEnum
    {
        Closed,
        Closing,
        OutOfOrder,
        TechnicalProblem,
        Open,
        Opening
    }

    public enum ItemColors
    {
        AntiqueWhite = 0,
        Red,
        Blue,
        Yellow,
        Orange,
        White,
        Black,
        Pink,
        Gray,
        LightGreen,
        DarkGreen
    }

    public enum Sex
    {
        Male,
        Female,
        Hen,
        NotSet
    }

    public enum AddressType
    {
        Primary,
        Workplace
    }
}
