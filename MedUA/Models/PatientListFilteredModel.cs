namespace MedUA.Models
{
    using System;

    public class PatientListFilteredModel
    {
        public int? YearOfBirth { get; set; }
        public int? DayOfBirth { get; set; }
        public int? MonthOfBirth { get; set; }
        public int? YearOfVisit { get; set; }
        public int? DayOfVisit { get; set; }
        public int? MonthOfVisit { get; set; }
        public string Diagnosis { get; set; }

    }
}