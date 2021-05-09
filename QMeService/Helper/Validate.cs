using System;

namespace Bumbleberry.QMeService.Helper
{
    public static class Validate
    {
        public static void ValidateMandatoryFields(string countryId, string activityGuid)
        {
            if (string.IsNullOrWhiteSpace(countryId))
                throw new ArgumentException("CountryId is missing value");
            if (string.IsNullOrWhiteSpace(activityGuid))
                throw new ArgumentException("ActivityGuid is missing value");
        }

        public static void ValidateMandatoryField(string field, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(field))
                throw new ArgumentException($"{fieldName} is missing value");
        }

        public static void ValidateDateTimeHaveValue(DateTime field, string fieldName)
        {
            if (field <= DateTime.MinValue )
                throw new ArgumentException($"{fieldName} is missing value");
        }
    }
}
