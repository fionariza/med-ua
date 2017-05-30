namespace MedUA.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DAL;

    public class ResearchProvider : IResearchProvider
    {
        private IEnumerable<Research> researches;

        public ResearchProvider(IEnumerable<Research> researches)
        {
            if (researches == null)
            {
                throw new NullReferenceException();
            }
            this.researches = researches;
        }
        

        public IEnumerable<SelectListItem> GetResearches()
        {
            return researches.Select(
                x =>
                    new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()}).ToList();
        }
    }
}