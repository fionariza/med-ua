using System.Collections.Generic;
using System.Web.Mvc;

namespace MedUA.Data
{
    using System;

    using MedUA.DAL;
    using MedUA.DAL.EntityModel;
    using MedUA.Helpers;
    using MedUA.Models;

    public interface IResearchProvider
    {
        IEnumerable<SelectListItem> GetResearches(ResearchSettlementScope scope, Hospital hospital);

        IEnumerable<ResearchesPartialViewModel> GetResearches(ResearchSettlementScope scope, Hospital hospital, int researchId);

        IEnumerable<SelectListItem> GetAvailiableTimes(string dateTime, HospitalResearch research);
    }
}