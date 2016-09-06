using System;

namespace BackerV2
{
    public static class DateFixer
    {
        public static string dateToFolderFormat(DateTime date)
        {
            string fixedDate;
            fixedDate = date.ToString();
            fixedDate = fixedDate.Replace('/', '_');
            fixedDate = fixedDate.Replace(':', '-');
            fixedDate = fixedDate.Replace(" ", "__");

            return fixedDate;
        }
        public static DateTime FolderToDateFormat(string folder)
        {
            DateTime date;
            string convert = folder;
            convert = convert.Replace("__", " ");
            convert = convert.Replace('_', '/');
            convert = convert.Replace('-', ':');

            try
            {
                date = Convert.ToDateTime(convert);
            }
            catch (FormatException)
            {
                date = DateTime.MaxValue;
            }

            return date;
        }
    }
}
