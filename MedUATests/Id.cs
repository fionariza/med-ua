namespace MedUATests
{
    using System.Threading;

    public static class Id
    {
        private static int index = -1;
        private static object lockObject = new object();
        public static int GetUnique()
        {
            lock (lockObject)
            {
                index++;
            }
            return index;
        }
    }
}
