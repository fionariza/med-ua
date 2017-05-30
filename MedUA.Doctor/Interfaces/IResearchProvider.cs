using System.Collections.Generic;
using System.Web.Mvc;

namespace MedUA.Data
{
    public interface IResearchProvider
    {
        IEnumerable<SelectListItem> GetResearches();
    }
}