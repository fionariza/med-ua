namespace MedUA.Helpers
{
    using System;

    using MedUA.DAL;

    public class PatientYearMonthGenerator : BaseYearMonthGenerator<PatientUser>
    {
        public override Func<PatientUser, bool> YearMonth(int yearOfBirth, int monthOfBirth)
        {
            return patientUser => patientUser.DateOfBirth.Year == yearOfBirth && patientUser.DateOfBirth.Month == monthOfBirth;
        }
        public override Func<PatientUser, bool> YearDay(int yearOfBirth, int day)
        {
            return patientUser => patientUser.DateOfBirth.Year == yearOfBirth && patientUser.DateOfBirth.Day == day;
        }

        public override Func<PatientUser, bool> YearMonthDay(int yearOfBirth, int monthOfBirth, int day)
        {
            return patientUser => patientUser.DateOfBirth.Year == yearOfBirth && patientUser.DateOfBirth.Month == monthOfBirth && patientUser.DateOfBirth.Day == day;
        }

        public override Func<PatientUser, bool> MonthDay(int monthOfBirth, int day)
        {
            return patientUser => patientUser.DateOfBirth.Month == monthOfBirth && patientUser.DateOfBirth.Day == day;
        }

        public override Func<PatientUser, bool> Month(int monthOfBirth)
        {
            return patientUser => patientUser.DateOfBirth.Month == monthOfBirth;
        }

        public override Func<PatientUser, bool> Day(int dayOfBirth)
        {
            return patientUser => patientUser.DateOfBirth.Day == dayOfBirth;
        }

        public override Func<PatientUser, bool> Year(int yearOfBirth)
        {
            return patientUser => patientUser.DateOfBirth.Year == yearOfBirth;
        }
    }
}