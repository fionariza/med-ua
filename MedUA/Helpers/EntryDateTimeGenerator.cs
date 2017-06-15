using System;

namespace MedUA.Helpers
{
    using MedUA.DAL;
    public class EntryDateTimeGenerator : BaseYearMonthGenerator<Entry>
    {
        public override Func<Entry, bool> YearMonth(int yearOfBirth, int monthOfBirth)
        {
            return entry => entry.TimeStamp.Year == yearOfBirth && entry.TimeStamp.Month == monthOfBirth;
        }
        public override Func<Entry, bool> YearDay(int yearOfBirth, int day)
        {
            return entry => entry.TimeStamp.Year == yearOfBirth && entry.TimeStamp.Day == day;
        }

        public override Func<Entry, bool> YearMonthDay(int yearOfBirth, int monthOfBirth, int day)
        {
            return entry => entry.TimeStamp.Year == yearOfBirth && entry.TimeStamp.Month == monthOfBirth && entry.TimeStamp.Day == day;
        }

        public override Func<Entry, bool> MonthDay(int monthOfBirth, int day)
        {
            return entry => entry.TimeStamp.Month == monthOfBirth && entry.TimeStamp.Day == day;
        }

        public override Func<Entry, bool> Month(int monthOfBirth)
        {
            return entry => entry.TimeStamp.Month == monthOfBirth;
        }

        public override Func<Entry, bool> Day(int dayOfBirth)
        {
            return entry => entry.TimeStamp.Day == dayOfBirth;
        }

        public override Func<Entry, bool> Year(int yearOfBirth)
        {
            return entry => entry.TimeStamp.Year == yearOfBirth;
        }
    }
}