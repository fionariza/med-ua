namespace MedUA.Helpers
{
    public interface IUserComparable
    {
        bool Compare(string surname, string name, string patronimic, string placeOfBirth);
    }
}