using System;
using MedUA.DAL;

namespace MedUA.Helpers
{
    public abstract class BaseYearMonthGenerator<TEntity>
    {
        public Func<TEntity, bool> GetCompareFunc(int? yearOfBirth, int? monthOfBirth, int? dayOfBirth)
        {
            if (yearOfBirth == null && monthOfBirth == null && dayOfBirth == null)
            {
                return null;
            }

            if (yearOfBirth != null)
            {
                if (monthOfBirth != null)
                {
                    if (dayOfBirth != null)
                    {
                        return YearMonthDay(yearOfBirth.Value, monthOfBirth.Value, dayOfBirth.Value);
                    }
                    return YearMonth(yearOfBirth.Value, monthOfBirth.Value);
                }
                if (dayOfBirth != null)
                {
                    return YearDay(yearOfBirth.Value, dayOfBirth.Value);
                }
                return Year(yearOfBirth.Value);
            }

            if (monthOfBirth != null)
            {
                if (dayOfBirth != null)
                {
                    return MonthDay(monthOfBirth.Value, dayOfBirth.Value);
                }
                return Month(monthOfBirth.Value);
            }

            return Day(dayOfBirth.Value);

        }
        public abstract Func<TEntity, bool> Day(int dayOfBirth);
        public abstract Func<TEntity, bool> Month(int monthOfBirth);
        public abstract Func<TEntity, bool> MonthDay(int monthOfBirth, int day);
        public abstract Func<TEntity, bool> Year(int yearOfBirth);
        public abstract Func<TEntity, bool> YearDay(int yearOfBirth, int day);
        public abstract Func<TEntity, bool> YearMonth(int yearOfBirth, int monthOfBirth);
        public abstract Func<TEntity, bool> YearMonthDay(int yearOfBirth, int monthOfBirth, int day);
    }
}