namespace MedUA.Helpers
{
    using System;
    using System.Text.RegularExpressions;

    using MedUA.DAL;
    using MedUA.Models;

    public class SearchStringSpliter
    {
        private readonly ISurnameNamePatronimicRetriever patronimicRetriever;
        public const int CodeLength = 18;
        public SearchStringSpliter(ISurnameNamePatronimicRetriever patronimicRetriever)
        {
            this.patronimicRetriever = patronimicRetriever;
        }

        public bool Split(string patientSearch, out Func<PatientUser, bool> func, out string medicalCode)
        {
            func = null;
            medicalCode = null;
            try
            {
                var searchString = patientSearch?.Trim();
                if (string.IsNullOrEmpty(searchString))
                {
                    return false;
                }

                var intRegex = new Regex("^[0-9]+$");
                if (intRegex.IsMatch(searchString) && searchString.Length == CodeLength)
                {
                    func = user => user.Code == searchString;
                    medicalCode = searchString;
                    return true;
                }

                var sut = this.patronimicRetriever.RetrieveFunc(searchString);
                if (sut == null)
                {
                    return false;
                }
                func = user => sut.Compare(user.Surname, user.Name, user.Partonimic, user.PlaceOfBirth);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}