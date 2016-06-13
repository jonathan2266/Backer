using System;

namespace BackerV2
{
    public static class DateFixer
    {
        public static string fix(DateTime date)
        {
            string fixedDate;
            fixedDate = date.ToString();
            fixedDate = fixedDate.Replace('/', '_');
            fixedDate = fixedDate.Replace(':', '-');
            fixedDate = fixedDate.Replace(" ", "__");

            return fixedDate;
        }
    }
}
