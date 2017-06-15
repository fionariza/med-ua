namespace MedUATests.Helpers
{
    using System;
    using System.Collections.Generic;

    using MedUA.DAL;
    using MedUA.DAL.EntityModel;

    public class HospitalBuilder
    {
        private Region region = new Region()
                                {
                                    Id = Id.GetUnique(),
                                    Name = Guid.NewGuid().ToString(),
                                    Oblast = new Oblast() { Id = Id.GetUnique(), Name = Guid.NewGuid().ToString() }
                                };

        private string settlement = Guid.NewGuid().ToString();

        private ICollection<HospitalResearch> researches = new List<HospitalResearch>();

        public HospitalBuilder WithSettlement(string settlement)
        {
            this.settlement = settlement;
            return this;
        }

        public HospitalBuilder WithRegion(Region region)
        {
            this.region = region;
            return this;
        }

        public HospitalBuilder WithOblast(Oblast oblast)
        {
            this.region.Oblast = oblast;
            return this;
        }

        public HospitalBuilder WithResearches(ICollection<HospitalResearch> researches)
        {
            this.researches = researches;
            return this;
        }

        public HospitalBuilder WithResearch(HospitalResearch research)
        {
            this.researches.Add(research);
            return this;
        }

        public Hospital Build()
        {
            return new Hospital() { Id = Id.GetUnique(), SettlementName = this.settlement, Region = this.region, Researches = this.researches };
        }
    }
}
