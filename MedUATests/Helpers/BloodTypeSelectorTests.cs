namespace MedUATests.Helpers
{
    using MedUA.DAL;
    using MedUA.Helpers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class BloodTypeSelectorTests
    {

        [TestMethod()]
        public void GetName_ReturnsABPlus()
        {
            //Arrange
            var type = BloodType.ABPlus;
            var expected = BloodTypeSelectorNames.ABPlus;

            this.AssertType(type, expected);
        }

        [TestMethod()]
        public void GetName_ReturnsAPlus()
        {
            //Arrange
            var type = BloodType.APlus;
            var expected = BloodTypeSelectorNames.APlus;

            this.AssertType(type, expected);
        }

        [TestMethod()]
        public void GetName_ReturnsBPlus()
        {
            //Arrange
            var type = BloodType.BPlus;
            var expected = BloodTypeSelectorNames.BPlus;

            this.AssertType(type, expected);
        }

        [TestMethod()]
        public void GetName_ReturnsOPlus()
        {
            //Arrange
            var type = BloodType.OPlus;
            var expected = BloodTypeSelectorNames.OPlus;

            this.AssertType(type, expected);
        }

        [TestMethod()]
        public void GetName_ReturnsABMinus()
        {
            //Arrange
            var type = BloodType.ABMinus;
            var expected = BloodTypeSelectorNames.ABMinus;

            this.AssertType(type, expected);
        }


        [TestMethod()]
        public void GetName_ReturnsAMinus()
        {
            //Arrange
            var type = BloodType.AMinus;
            var expected = BloodTypeSelectorNames.AMinus;

            this.AssertType(type, expected);
        }

        [TestMethod()]
        public void GetName_ReturnsBMinus()
        {
            //Arrange
            var type = BloodType.BMinus;
            var expected = BloodTypeSelectorNames.BMinus;

            this.AssertType(type, expected);
        }


        [TestMethod()]
        public void GetName_ReturnsOMinus()
        {
            //Arrange
            var type = BloodType.OMinus;
            var expected = BloodTypeSelectorNames.OMinus;

            this.AssertType(type, expected);
        }

        private void AssertType(BloodType type, string expected)
        {
            //Act
            var real = BloodTypeSelector.GetName(type);

            //Assert
            Assert.AreEqual(real, expected);
        }
    }
}