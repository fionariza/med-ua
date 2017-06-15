namespace MedUA.DAL.UserBuilder
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using MedUA.DAL.EntityModel;
    using MedUA.DAL.TestData;

    public class RegionsGenerator
    {
        private ApplicationDbContext context;

        private Random random;

        public RegionsGenerator(ApplicationDbContext context)
        {
            this.context = context;
            this.random = new Random();
        }

        public void Generate()
        {
            this.RegisterResearches(TestDataGenerator.GetResearches());
            this.RegistratePlaces(TestDataGenerator.GetPlaces());
        }

        private void RegistratePlaces(IEnumerable<Place> places)
        {
            foreach (var place in places)
            {
                var region = this.context.Regions.Include(r => r.Hospitals).ToList().FirstOrDefault(x => x.Name == place.Region);
                if (region == null)
                {
                    var oblast = GetOrAddOblast(place.Oblast);
                    region = new Region() { Name = place.Region, Oblast = oblast, Hospitals = new List<Hospital>() };
                    this.context.Regions.Add(region);
                }
                RegisterHospital(place, region);
            }
        }

        private void RegisterHospital(Place place, Region region)
        {
            var hospital = region.Hospitals.FirstOrDefault(x => x.SettlementName == place.SettlementName);
            var hospitalNonExist = hospital == null;
            hospital = hospital ?? new Hospital() { Researches = new List<HospitalResearch>() };
            hospital.Region = region;
            hospital.Address = $"{place.SettlementName}, {this.random.Next(100)}";
            hospital.Name = $" Лікарня № {this.random.Next(100)}";
            hospital.SettlementName = place.SettlementName;
            if (!hospital.Researches.Any())
            {
                foreach (var hospitalResearch in GenerateHospitalResearches())
                {
                    hospital.Researches.Add(hospitalResearch);
                }
            }
            if (hospitalNonExist)
            {
                this.context.Hospitals.Add(hospital);
            }
            this.context.SaveChanges();
        }

        /// <summary>
        /// Реєстрація досліджень
        /// </summary>
        /// <param name="researches">Назви досліджень</param>
        private void RegisterResearches(IEnumerable<string> researches)
        {
            foreach (var research in researches)
            {
                if (this.context.Researches.FirstOrDefault(x => x.Name == research) != null) continue;
                var researchSave = new Research() { Name = research };
                this.context.Researches.Add(researchSave);
            }

            this.context.SaveChanges();
        }

        private IEnumerable<HospitalResearch> GenerateHospitalResearches()
        {
            var researchList = this.context.Researches.ToList();
            var count = this.random.Next(1, researchList.Count);
            for (var index = 0; index < count; index++)
            {
                var researchIndex = this.random.Next(researchList.Count() - 1);
                yield return new HospitalResearch() { Price = this.random.Next(0, 3000) + this.random.NextDouble(), Research = researchList[researchIndex], StartTime = new DateTime(2010, 1, 1, 8, 30, 0).ToString("O"), EndTime = new DateTime(2010, 1, 1, 22, 0, 0).ToString("O") };
                researchList.RemoveAt(researchIndex);
            }
        }

        private Oblast GetOrAddOblast(string placeOblast)
        {
            var oblast = this.context.Oblasts.ToList().FirstOrDefault(o => o.Name == placeOblast);
            if (oblast != null)
            {
                return oblast;
            }
            oblast = new Oblast() { Name = placeOblast };
            this.context.Oblasts.Add(oblast);
            return oblast;
        }
    }
}
