namespace MedUA.Helpers
{
    using System;
    using System.Linq;

    using MedUA.DAL;

    public class SurnameNamePatronimicRetriever
    {
        public Func<PatientUser, bool> RetrieveFunc(string searchString)
        {
            var splits = searchString.Split(' ').ToList();
            splits.RemoveAll(p => string.IsNullOrEmpty(p.Trim()));
            if (splits.Count < 3)
            {
                return null;
            }
            return
                user =>
                    GetComparer(user.Surname, splits[0]) 
                    && GetComparer(user.Name, splits[1]) 
                    && GetComparer(user.Partonimic, splits[2])
                    && (splits.Count!= 4 || GetComparer(user.PlaceOfBirth, splits[3]));
        }

        private static bool GetComparer(string user, string split)
        {
            return string.Compare(user, split.Trim(), StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}