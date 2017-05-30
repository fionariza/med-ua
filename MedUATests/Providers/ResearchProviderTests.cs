namespace MedUATests.Providers
{
    using System.Linq;

    using MedUA.Data;
    using MedUA.DAL;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class ResearchProviderTests
    {
        private Research[] researches;

        private const string FirstResearch = "FirstResearch";

        private const int FirstId = 1;
        private const string SecondResearch = "SecondResearch";

        private const int SecondId = 2;

        [TestInitialize]
        public void Init()
        {
            this.researches = new[]
            {
                new Research() { Name = FirstResearch, Id = FirstId},
                new Research() { Name = SecondResearch, Id = SecondId}
            };
        }

        [TestMethod()]
        public void ResearchProviderTest()
        {
            var selectedResearches = this.CreateSut().GetResearches().ToList();
            
            Assert.IsNotNull(selectedResearches);
            Assert.AreEqual(selectedResearches.Count, this.researches.Length);
            CollectionAssert.AreEqual(selectedResearches.ToDictionary(x=>x.Value, x=>x.Text), this.researches.ToDictionary(x=>x.Id.ToString(), x=>x.Name));
            Assert.IsFalse(selectedResearches.Any(x=>x.Selected));
            Assert.IsFalse(selectedResearches.Any(x => x.Disabled));
        }
        

        private ResearchProvider CreateSut()
        {
            return new ResearchProvider(this.researches);
        }
    }
}