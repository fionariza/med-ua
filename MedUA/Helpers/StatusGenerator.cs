namespace MedUA.Helpers
{
    using System;

    using MedUA.DAL.EntityModel;

    public static class StatusGenerator
    {
        public static string GetStatus(Status status)
        {
            switch (status)
            {
                case Status.Upcoming:
                    return MedUA.Resources.Resource.UpcomingStatus;
                case Status.Cancelled:
                    return MedUA.Resources.Resource.CancelledStatus;
                case Status.Passed:
                    return MedUA.Resources.Resource.PassedStatus;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}