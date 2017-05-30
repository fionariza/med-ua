namespace MedUATests.Helpers
{
    using System;

    using MedUA.DAL;
    using MedUA.Helpers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass()]
    public class SearchStringSpliterTests
    {
        private Mock<ISurnameNamePatronimicRetriever> searchStringSplitter;

        private string MedicalCode = "123456789123456789";

        private PatientUser user;

        private Mock<IUserComparable> snpMock;

        [TestInitialize]
        public void Init()
        {
            this.searchStringSplitter = new Mock<ISurnameNamePatronimicRetriever>();
            this.snpMock = new Mock<IUserComparable>();
            this.snpMock.Setup(x => x.Compare(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            this.searchStringSplitter.Setup(x => x.RetrieveFunc(It.IsAny<string>())).Returns((IUserComparable)null);
            this.user = new PatientUser() { Code = this.MedicalCode };
        }

        [TestMethod]
        public void Split_MedicalCode_ReturnTrue()
        {
            var sut = this.CreateSut();
            Func<PatientUser, bool> func = null;
            string medicalCode = null;

            this.searchStringSplitter.Setup(x => x.RetrieveFunc(It.IsAny<string>())).Returns(this.snpMock.Object);
            var splitted = sut.Split(this.MedicalCode, out func, out medicalCode);

            Assert.IsTrue(splitted);
            Assert.AreEqual(this.MedicalCode, this.MedicalCode);
            Assert.IsTrue(func.Invoke(this.user));
        }

        [TestMethod]
        public void Split_SearchStringSplitterReturnNull_ReturnTrue()
        {
            var sut = this.CreateSut();
            Func<PatientUser, bool> func = null;
            string medicalCode = null;
            
            var splitted = sut.Split(this.MedicalCode, out func, out medicalCode);

            Assert.IsTrue(splitted);
            Assert.AreEqual(this.MedicalCode, this.MedicalCode);
            Assert.IsTrue(func.Invoke(this.user));
        }

        [TestMethod]
        public void Split_MedicalCodeSpaces_ReturnTrue()
        {
            var sut = this.CreateSut();
            Func<PatientUser, bool> func = null;
            string medicalCode = null;

            this.searchStringSplitter.Setup(x => x.RetrieveFunc(It.IsAny<string>())).Returns(this.snpMock.Object);
            var splitted = sut.Split($"   {this.MedicalCode}   ", out func, out medicalCode);

            Assert.IsTrue(splitted);
            Assert.AreEqual(this.MedicalCode, this.MedicalCode);
            Assert.IsTrue(func.Invoke(this.user));
        }
        
        [TestMethod]
        public void Split_MedicalCodeLessThan18_ReturnFalse()
        {
            var sut = this.CreateSut();
            Func<PatientUser, bool> func = null;
            string medicalCode = null;
            
            var splitted = sut.Split("2907", out func, out medicalCode);
            
            Assert.IsFalse(splitted);
            Assert.IsNull(medicalCode);
            Assert.IsNull(func);
        }

        [TestMethod]
        public void Split_MedicalCodeMoreThan18_ReturnFalse()
        {
            var sut = this.CreateSut();
            Func<PatientUser, bool> func = null;
            string medicalCode = null;
            
            var splitted = sut.Split(this.MedicalCode + 14, out func, out medicalCode);

            Assert.IsFalse(splitted);
            Assert.IsNull(medicalCode);
            Assert.IsNull(func);
        }

        [TestMethod]
        public void Split_MedicalIsNotDigital_ReturnFalse()
        {
            var sut = this.CreateSut();
            Func<PatientUser, bool> func = null;
            string medicalCode = null;

            var splitted = sut.Split(this.MedicalCode.Replace(this.MedicalCode[0], 'a'), out func, out medicalCode);

            Assert.IsFalse(splitted);
            Assert.IsNull(medicalCode);
            Assert.IsNull(func);
        }

        [TestMethod]
        public void Split_MedicalCodeTwice_ReturnFalse()
        {
            var sut = this.CreateSut();
            Func<PatientUser, bool> func = null;
            string medicalCode = null;
            
            var splitted = sut.Split($"{this.MedicalCode} {this.MedicalCode}", out func, out medicalCode);

            Assert.IsFalse(splitted);
            Assert.IsNull(medicalCode);
            Assert.IsNull(func);
        }

        private SearchStringSpliter CreateSut()
        {
            return new SearchStringSpliter(this.searchStringSplitter.Object);
        }
    }
}