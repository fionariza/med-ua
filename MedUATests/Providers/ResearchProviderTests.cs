namespace MedUATests.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using MedUA.Data;
    using MedUA.DAL;
    using MedUA.DAL.EntityModel;
    using MedUA.Helpers;

    using MedUATests.Helpers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class ResearchProviderTests
    {
        private Hospital currentHospital;

        private List<HospitalResearch> currentSettlementResearches;

        private List<HospitalResearch> regionResearches;

        private List<HospitalResearch> oblastResearches;

        private List<HospitalResearch> allResearches;

        private List<Hospital> hospitals;

        [TestInitialize]
        public void Init()
        {
            this.currentSettlementResearches = this.CreateResearch(2).ToList();
            this.currentHospital = new HospitalBuilder()
                .WithResearch(currentSettlementResearches[0])
                .Build();
            var sameSettlementAndRegion = new HospitalBuilder()
                .WithRegion(currentHospital.Region)
                .WithSettlement(currentHospital.SettlementName)
                .WithResearch(currentSettlementResearches[1])
                .Build();
            
            this.regionResearches = this.CreateResearch().ToList();
            var sameRegion = new HospitalBuilder()
                .WithResearches(this.regionResearches)
                .WithRegion(currentHospital.Region)
                .Build();

            this.regionResearches.AddRange(this.currentSettlementResearches);

            this.oblastResearches = this.CreateResearch().ToList();
            var sameOblast = new HospitalBuilder()
                .WithResearches(this.oblastResearches)
                .WithOblast(currentHospital.Region.Oblast).Build();
            this.oblastResearches.AddRange(this.regionResearches);

            this.allResearches = this.CreateResearch().ToList();
            var otherHospital = new HospitalBuilder().WithResearches(this.allResearches).Build();
            this.allResearches.AddRange(this.oblastResearches);
            this.hospitals = new List<Hospital>() { this.currentHospital, sameSettlementAndRegion, sameRegion, sameOblast, otherHospital };

        }

        [TestMethod()]
        public void GetResearches_SettlementScope_ReturnResearchesJustForSettlement()
        {
            var selectedResearches = this.CreateSut().GetResearches(ResearchSettlementScope.Settlement, this.currentHospital).ToList();

            this.Compare(selectedResearches, this.currentSettlementResearches);
        }

        [TestMethod()]
        public void GetResearches_SettlementScope_DifferentRegion_Skip()
        {
            var resultResearch = this.CreateResearch().ToList();
            resultResearch.AddRange(this.currentSettlementResearches);
            this.hospitals.Add(new HospitalBuilder().WithSettlement(this.currentHospital.SettlementName).WithResearches(resultResearch).Build());
            var selectedResearches = this.CreateSut().GetResearches(ResearchSettlementScope.Settlement, this.currentHospital).ToList();

            this.Compare(selectedResearches, this.currentSettlementResearches);
        }

        [TestMethod()]
        public void GetResearches_SettlementScope_ReturnDistinctResearches()
        {
            var resultResearch = this.CreateResearch().ToList();
            resultResearch.AddRange(this.currentSettlementResearches);
            this.hospitals.Add(new HospitalBuilder().WithRegion(this.currentHospital.Region).WithSettlement(this.currentHospital.SettlementName).WithResearches(resultResearch).Build());
            var selectedResearches = this.CreateSut().GetResearches(ResearchSettlementScope.Settlement, this.currentHospital).ToList();

            this.Compare(selectedResearches, resultResearch);
        }

        [TestMethod()]
        public void GetResearches_RegionScope_ReturnResearchesJustForRegion()
        {
            var selectedResearches = this.CreateSut().GetResearches(ResearchSettlementScope.Region, this.currentHospital).ToList();

            this.Compare(selectedResearches, this.regionResearches);
        }

        [TestMethod()]
        public void GetResearches_RegionScope_ReturnDistinctResearches()
        {
            var resultResearch = this.CreateResearch().ToList();
            resultResearch.AddRange(this.regionResearches);
            this.hospitals.Add(new HospitalBuilder().WithRegion(this.currentHospital.Region).WithResearches(resultResearch).Build());
            var selectedResearches = this.CreateSut().GetResearches(ResearchSettlementScope.Region, this.currentHospital).ToList();

            this.Compare(selectedResearches, resultResearch);
        }


        [TestMethod()]
        public void GetResearches_OblastScope_ReturnResearchesJustForOblast()
        {
            var selectedResearches = this.CreateSut().GetResearches(ResearchSettlementScope.Oblast, this.currentHospital).ToList();

            this.Compare(selectedResearches, this.oblastResearches);
        }

        [TestMethod()]
        public void GetResearches_OblastScope_ReturnDistinctResearches()
        {
            var resultResearch = this.CreateResearch().ToList();
            resultResearch.AddRange(this.oblastResearches);
            this.hospitals.Add(new HospitalBuilder().WithOblast(this.currentHospital.Region.Oblast).WithResearches(resultResearch).Build());
            var selectedResearches = this.CreateSut().GetResearches(ResearchSettlementScope.Oblast, this.currentHospital).ToList();

            this.Compare(selectedResearches, resultResearch);
        }

        [TestMethod()]
        public void GetResearches_AllScope_ReturnAllResearches()
        {
            var selectedResearches = this.CreateSut().GetResearches(ResearchSettlementScope.Country, this.currentHospital).ToList();

            this.Compare(selectedResearches, this.allResearches);
        }

        [TestMethod()]
        public void GetResearches_AllScope_ReturnDistinctResearches()
        {
            var resultResearch = this.CreateResearch().ToList();
            resultResearch.AddRange(this.allResearches);
            this.hospitals.Add(new HospitalBuilder().WithResearches(resultResearch).Build());
            var selectedResearches = this.CreateSut().GetResearches(ResearchSettlementScope.Country, this.currentHospital).ToList();

            this.Compare(selectedResearches, resultResearch);
        }

        private void Compare(IList<SelectListItem> selectedResearches, IList<HospitalResearch> comparable)
        {
            var dictionaryResearch = selectedResearches.ToDictionary(x => x.Value, x => x.Text);
            var comparableDictionary = comparable.ToDictionary(x => x.Research.Id.ToString(), x => x.Research.Name);

            Assert.IsNotNull(selectedResearches);
            Assert.AreEqual(selectedResearches.Count, comparable.Count);
            CollectionAssert.AreEquivalent(dictionaryResearch, comparableDictionary);
            Assert.IsFalse(selectedResearches.Any(x => x.Selected));
            Assert.IsFalse(selectedResearches.Any(x => x.Disabled));
        }

        private IEnumerable<HospitalResearch> CreateResearch(int count = 1)
        {
            for (int index = 0; index < count; index++)
            {
                yield return new HospitalResearch() { Research = new Research() { Id = Id.GetUnique(), Name = Guid.NewGuid().ToString() } };
            }
        }


        private ResearchProvider CreateSut()
        {
            throw new NotImplementedException();
            //return new ResearchProvider(this.hospitals);
        }
    }
}