namespace MedUA.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DAL;

    using MedUA.DAL.EntityModel;
    using MedUA.Helpers;
    using MedUA.Models;

    public class ResearchProvider : IResearchProvider
    {
        private IEnumerable<HospitalResearch> hospitalResearches;

        public ResearchProvider(IEnumerable<HospitalResearch> hospitalResearches)
        {
            if (hospitalResearches == null)
            {
                throw new NullReferenceException();
            }
            this.hospitalResearches = hospitalResearches;
        }

        public IEnumerable<ResearchesPartialViewModel> GetResearches(ResearchSettlementScope scope, Hospital hospital, int researchId)
        {
            foreach (var hospitalResearch in this.GetFilteredResearches(scope, hospital, this.hospitalResearches.Where(r => r.Research.Id == researchId)))
            {
                var dictionary = GenerateDates();
                var times = GetAvailiableTimes(dictionary.Keys.First(), hospitalResearch);
                yield return
                    new ResearchesPartialViewModel()
                    {
                        ResearchHospitalId = hospitalResearch.Id,
                        HospitalAddress = hospitalResearch.Hospital.Address,
                        HospitalName = hospitalResearch.Hospital.Name,
                        Price =
                            hospitalResearch.Price <= 0
                                ? Resources.Resource.Free
                                : hospitalResearch.Price.ToString("C", CultureInfo.CurrentCulture),
                        ResearchName = hospitalResearch.Research.Name,
                        Dates = dictionary.Select((d, index) => new SelectListItem() { Text = d.Value, Value = d.Key, Selected = index == 0 }),
                        Times = times
                    };
            }
        }

        private Dictionary<string, string> GenerateDates()
        {
            var dictionary = new Dictionary<string, string>();
            var dateTime = DateTime.Now.AddDays(1);
            while (dictionary.Count != 22)
            {
                if (dateTime.DayOfWeek != DayOfWeek.Saturday && dateTime.DayOfWeek != DayOfWeek.Sunday)
                {
                    dictionary.Add(dateTime.ToCompareDate(), dateTime.ToShowDate());
                }
                dateTime = dateTime.AddDays(1);
            }
            return dictionary;
        }

        public IEnumerable<SelectListItem> GetAvailiableTimes(string date, HospitalResearch research)
        {
            var dateTime = DateTime.Parse(date);
            var dic = this.GenerateTimes(research.StartTime, research.EndTime);
            foreach (var researchAppointment in research.Appointments.Where(a => a.Status == Status.Upcoming && a.Appointment.Year == dateTime.Year && a.Appointment.Month == dateTime.Month && a.Appointment.Day == dateTime.Day))
            {
                var appointmentTime = researchAppointment.Appointment.ToCompareTime();
                if (dic.ContainsKey(appointmentTime))
                {
                    dic.Remove(appointmentTime);
                }
            }
            return dic.Select(pair => new SelectListItem() { Text = pair.Value, Value = pair.Key});
        }

        private Dictionary<string, string> GenerateTimes(string researchStartTime, string researchEndTime)
        {
            var startTime = DateTime.Parse(researchStartTime);
            var endTime = DateTime.Parse(researchEndTime);
            var dic = new Dictionary<string, string>();
            while (startTime.Hour <= endTime.Hour)
            {
                dic.Add(startTime.ToCompareTime(), startTime.ToShowTime());
                startTime = startTime.AddMinutes(30);
            }
            return dic;
        }

        public IEnumerable<SelectListItem> GetResearches(ResearchSettlementScope scope, Hospital hospital)
        {
            var researchIds = new HashSet<int>();
            foreach (var hospitalResearch in this.GetFilteredResearches(scope, hospital, this.hospitalResearches))
            {
                var researchId = hospitalResearch.Research.Id;
                if (researchIds.Contains(researchId))
                {
                    continue;
                }
                researchIds.Add(researchId);
                yield return new SelectListItem() { Text = hospitalResearch.Research.Name, Value = hospitalResearch.Research.Id.ToString() };
            }
        }

        private IEnumerable<HospitalResearch> GetFilteredResearches(ResearchSettlementScope scope, Hospital hospital, IEnumerable<HospitalResearch> hospitalResearches)
        {
            switch (scope)
            {
                case ResearchSettlementScope.Settlement:
                    return hospitalResearches.Where(hr => hr.Hospital.Region.Id == hospital.Region.Id && hr.Hospital.SettlementName == hospital.SettlementName);
                case ResearchSettlementScope.Region:
                    return hospitalResearches.Where(hr => hr.Hospital.Region.Id == hospital.Region.Id);
                case ResearchSettlementScope.Oblast:
                    return hospitalResearches.Where(hr => hr.Hospital.Region.Oblast.Id == hospital.Region.Oblast.Id);
                case ResearchSettlementScope.Country:
                    return hospitalResearches;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scope), scope, null);
            }
        }

    }
}