namespace MedUA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using MedUA.DAL;
    using MedUA.Resources;

    public class EntryHistoryViewModel
    {
        public int Id { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public DateTime TimeStampDateTime { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "EntryHistoryViewModelDate")]
        public string TimeStamp { get { return TimeStampDateTime.ToString("g", CultureInfo.CurrentCulture); } }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [Display(ResourceType = typeof(Resource), Name = "EntryHistoryViewModelComplains")]
        public string Complains { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [Display(ResourceType = typeof(Resource), Name = "EntryHistoryViewModelExamination")]
        public string Examination { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "EntryHistoryViewModelDiagnosis")]
        public string Diagnosis { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [Display(ResourceType = typeof(Resource), Name = "EntryHistoryViewModelRecomendations")]
        public string Recomendations { get; set; }
        
        [Display(ResourceType = typeof(Resource), Name = "EntryHistoryViewModelResearches")]
        public IEnumerable<string> ResearchIds { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "EntryHistoryViewModelDoctorSign")]
        public string DoctorSign { get; set; }

        public static EntryHistoryViewModel Convert(Entry arg)
        {
            return new EntryHistoryViewModel()
            {
                Id = arg.Id,
                Complains = arg.Complains,
                Diagnosis = arg.Diagnosis,
                Examination = arg.Examination,
                Recomendations = arg.Recomendations,
                ResearchIds = arg.Researches.Select(x => x.Name).ToList(),
                TimeStampDateTime = arg.TimeStamp,
                DoctorSign = string.Format(CultureInfo.CurrentCulture, "{0} {1}, {2}", arg.Doctor.Surname, arg.Doctor.Name, arg.Doctor.Position),
                DoctorId = arg.Doctor.Id,
                PatientId = arg.Patient.Id
            };
        }

        public static Entry ConvertBack(EntryHistoryViewModel model, ICollection<Research> researches, DoctorUser doctorUser, PatientUser patientUser)
        {
            var entry = new Entry
                        {
                            Complains = model.Complains,
                            Diagnosis = model.Diagnosis,
                            Examination = model.Examination,
                            Recomendations = model.Recomendations,
                            Researches = researches,
                            TimeStamp = model.TimeStampDateTime,
                            Doctor = doctorUser,
                            Patient = patientUser
                        };
            return entry;

        }
    }
}