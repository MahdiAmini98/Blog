using MudBlazor;
using System.Globalization;

namespace Blog.PanelAdmin.Pages.Utilities
{
    public class MudJalaliDatePicker : MudDatePicker
    {
        public MudJalaliDatePicker()
        {
            var persianCulture = new CultureInfo("fa-IR");
            persianCulture.DateTimeFormat.Calendar = new PersianCalendar();

            persianCulture.DateTimeFormat.MonthNames = new[]
            {
                 "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
                "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", ""
            };

            persianCulture.DateTimeFormat.AbbreviatedMonthNames = persianCulture.DateTimeFormat.MonthNames;

            persianCulture.DateTimeFormat.DayNames = new[]
            {
                "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه"
            };


            persianCulture.DateTimeFormat.AbbreviatedDayNames = new[]
            {
                "ی", "د", "س", "چ", "پ", "ج", "ش"
            };

            Culture = persianCulture;
            DateFormat = "yyyy/MM/dd";
            Date = DateTime.Now;


            AdditionalDateClassesFunc = (date) =>
            {

                var pc = new PersianCalendar();
                int m = pc.GetMonth(date);
                int d = pc.GetDayOfMonth(date);

                if (SolarHolidays.Contains((m, d)))
                    return "red-text text-accent-4";

                if (date.DayOfWeek == DayOfWeek.Friday)
                {
                    return "red-text text-accent-4";
                }
                return null;
            };

        }


        protected override string GetTitleDateString()
        {

            DateTime? date = Date;
            if (date == null)
                return string.Empty;

            var pc = new PersianCalendar();
            int year = pc.GetYear(date.Value);
            int month = pc.GetMonth(date.Value);
            int day = pc.GetDayOfMonth(date.Value);

            string[] persianDayNames =
            {
                "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه"
            };

            int dayOfWeekIndex = (int)date.Value.DayOfWeek;
            string dayName = persianDayNames[dayOfWeekIndex];


            string monthName = Culture.DateTimeFormat.MonthNames[month - 1];

            return $"{dayName}، {day} {monthName}";

        }

        public static readonly List<(int Month, int Day)> SolarHolidays = new()
           {
             (1, 1),   // 1 فروردین
             (1, 2),   // 2 فروردین
             (1, 3),   // 3 فروردین
             (1, 4),   // 4 فروردین
             (1, 12),  // 12 فروردین
             (1, 13),  // 13 فروردین
             (3, 14),  // 14 خرداد
             (3, 15),  // 15 خرداد
             (11, 22), // 22 بهمن
             (12, 29), // 29 اسفند
         };
    }
}
