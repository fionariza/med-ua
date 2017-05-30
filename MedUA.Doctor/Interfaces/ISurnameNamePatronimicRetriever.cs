namespace MedUA.Helpers
{
    public interface ISurnameNamePatronimicRetriever
    {
        IUserComparable RetrieveFunc(string searchString);
    }
}