namespace MedUA.DAL.TestData
{
    public class NamesAndAliases
    {
        public NamesAndAliases(string alias, string name, string surname, string patronimic)
        {
            Alias = alias;
            Name = name;
            Surname = surname;
            Patronimic = patronimic;
        }

        public string Alias
        {
            get;
            set;
        }

        public string Name
        {
            get; set;
        }
        public string Surname
        {
            get; set;
        }

        public string Patronimic
        {
            get; set;
        }
    }
}
