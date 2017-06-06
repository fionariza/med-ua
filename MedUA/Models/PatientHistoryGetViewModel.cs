namespace MedUA.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PatientHistoryGetViewModel
    {
        public bool FilterDoctor { get; set; } = true;
        public int Skip { get; set; }
        public int Page { get; set; } = 1;
    }
}