namespace MedUA.Helpers
{
    using System;
    using System.IO;
    using System.Linq;

    public class SurnameNamePatronimicRetriever : ISurnameNamePatronimicRetriever
    {
        public IUserComparable RetrieveFunc(string searchString)
        {
            var splits = searchString.Split(' ', '\t').ToList();
            splits.RemoveAll(p => string.IsNullOrEmpty(p.Trim()));
            return splits.Count < 3 ? null : new UserComparable() { Surname = splits[0], Name = splits[1], Patronimic = splits[2], PlaceOfBirth = splits.Count == 4 ? splits[3] : null };
        }


        public sealed class UserComparable : IUserComparable
        {
            public string Surname { get; set; }
            public string Name { get; set; }
            public string Patronimic { get; set; }
            public string PlaceOfBirth { get; set; }
            private bool GetComparer(string user, string split)
            {
                return string.Compare(user, split.Trim(), StringComparison.OrdinalIgnoreCase) == 0;
            }

            public bool Compare(string surname, string name, string patronimic, string placeOfBirth)
            {
               return GetComparer(Surname, surname) 
                    && GetComparer(Name, name)
                    && GetComparer(Patronimic, patronimic)
                    && (PlaceOfBirth == null || GetComparer(PlaceOfBirth, placeOfBirth));
            }
            
        }
    }
}