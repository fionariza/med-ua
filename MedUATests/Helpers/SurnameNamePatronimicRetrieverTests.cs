namespace MedUATests.Helpers
{
    using MedUA.Helpers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class SurnameNamePatronimicRetrieverTests
    {
        private const string Name = "Name";

        private const string Surname = "Surname";

        private const string PlaceOfBirth = "PlaceOfBirth";

        private const string Patronimic = "Patronimic";
        

        [TestMethod()]
        public void RetrieveFuncTest_SurnameNamePatronimic_ReturnTrue()
        {
            var fun = this.CreateSut().RetrieveFunc($"{Surname} {Name} {Patronimic}");

            Assert.IsTrue(this.CheckSnp(fun));
        }

        [TestMethod()]
        public void RetrieveFuncTest_SurnameNamePatronimicPlaceOfBirth_ReturnTrue()
        {
            var fun = this.CreateSut().RetrieveFunc($"{Surname} {Name} {Patronimic} {PlaceOfBirth}");

            Assert.IsTrue(this.CheckSnp(fun));
        }


        [TestMethod()]
        public void RetrieveFuncTest_SurnameNamePatronimicSpacing_ReturnTrue()
        {
            var fun = this.CreateSut().RetrieveFunc($"     {Surname}    {Name}    {Patronimic}    ");

            Assert.IsTrue(this.CheckSnp(fun));
        }

        [TestMethod()]
        public void RetrieveFuncTest_SurnameNamePatronimicPlaceOfBirthSpacing_ReturnTrue()
        {
            var fun = this.CreateSut().RetrieveFunc($"   {Surname}     {Name}    {Patronimic}   {PlaceOfBirth}   ");

            Assert.IsTrue(this.CheckSnp(fun));
        }

        [TestMethod()]
        public void RetrieveFuncTest_IncorrectSurname_ReturnFalse()
        {
            var fun = this.CreateSut().RetrieveFunc($"{Surname}a {Name} {Patronimic}");

            Assert.IsFalse(this.CheckSnp(fun));
        }

        [TestMethod()]
        public void RetrieveFuncTest_IncorrectName_ReturnFalse()
        {
            var fun = this.CreateSut().RetrieveFunc($"{Surname} {Name}a {Patronimic}");

            Assert.IsFalse(this.CheckSnp(fun));
        }

        [TestMethod()]
        public void RetrieveFuncTest_IncorrectPatronimic_ReturnFalse()
        {
            var fun = this.CreateSut().RetrieveFunc($"{Surname} {Name} {Patronimic}a");

            Assert.IsFalse(this.CheckSnp(fun));
        }

        [TestMethod()]
        public void RetrieveFuncTest_IncorrectPlaceOfBirth_ReturnFalse()
        {
            var fun = this.CreateSut().RetrieveFunc($"{Surname} {Name} {Patronimic} {PlaceOfBirth}a");

            Assert.IsFalse(this.CheckSnp(fun));
        }



        [TestMethod()]
        public void RetrieveFuncTest_NameFirst_ReturnFalse()
        {
            var fun = this.CreateSut().RetrieveFunc($"{Name} {Surname} {Patronimic}");

            Assert.IsFalse(this.CheckSnp(fun));
        }

        [TestMethod()]
        public void RetrieveFuncTest_PatronimicFirst_ReturnFalse()
        {
            var fun = this.CreateSut().RetrieveFunc($"{Patronimic} {Surname} {Name}");

            Assert.IsFalse(this.CheckSnp(fun));
        }

        [TestMethod()]
        public void RetrieveFuncTest_PlaceOfBirthFirst_ReturnFalse()
        {
            var fun = this.CreateSut().RetrieveFunc($"{PlaceOfBirth} {Surname} {Name}");

            Assert.IsFalse(this.CheckSnp(fun));
        }

        [TestMethod()]

        public void RetrieveFuncTest_JustSurname_ReturnNull()
        {
            var fun = this.CreateSut().RetrieveFunc($"{Surname}");
            Assert.IsNull(fun);
        }

        [TestMethod()]
        public void RetrieveFuncTest_JustSurnameName_ReturnNull()
        {
            var fun = this.CreateSut().RetrieveFunc($"{Surname} {Name}");
            Assert.IsNull(fun);
        }

        private bool CheckSnp(IUserComparable userComparable)
        {
            return userComparable.Compare(Surname, Name, Patronimic, PlaceOfBirth);
        }

        private SurnameNamePatronimicRetriever CreateSut()
        {
            return new SurnameNamePatronimicRetriever();
        }
    }
}