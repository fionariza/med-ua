namespace MedUA.Helpers
{
    using System;

    using MedUA.DAL;

    public class CompareFunctionCreator
    {
        public Func<Entry, bool> EntryFunc(string diagnosis, int? yearOfVisit, int? monthOfVisit, int? dayOfVisit, string doctorId)
        {
            var comparator = new EntryDateTimeGenerator().GetCompareFunc(yearOfVisit, monthOfVisit, dayOfVisit);
            if (string.IsNullOrEmpty(diagnosis?.Trim()))
            {
                
                if (comparator == null)
                {
                    return null;
                }
                
                return x => x.Doctor.Id == doctorId && comparator.Invoke(x);
            }
            if (comparator == null)
            {
                return x => x.Doctor.Id == doctorId && string.Compare(diagnosis, x.Diagnosis, StringComparison.OrdinalIgnoreCase) == 0;
            }
            return x => x.Doctor.Id == doctorId && comparator.Invoke(x);
        }

        public Func<PatientUser, bool> PatientFunc(int? yearOfBirth, int? monthOfBirth, int? dayOfBirth)
        {
            var patientComparator = new PatientYearMonthGenerator();
            return patientComparator.GetCompareFunc(yearOfBirth, monthOfBirth, dayOfBirth);
        }
    }
}