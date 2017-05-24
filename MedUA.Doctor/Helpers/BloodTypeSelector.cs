namespace MedUA.Helpers
{
    using System;

    using MedUA.DAL;
    //TODO::Localization
    public static class BloodTypeSelector
    {
        public static string GetName(BloodType bloodType)
        {
            switch (bloodType)
            {
                case DAL.BloodType.OPlus:
                    return "I+";
                case DAL.BloodType.APlus:
                    return "II+";
                case DAL.BloodType.BPlus:
                    return "III+";
                case DAL.BloodType.ABPlus:
                    return "VI+";
                case DAL.BloodType.OMinus:
                    return "I-";
                case DAL.BloodType.AMinus:
                    return "II-";
                case DAL.BloodType.BMinus:
                    return "III-";
                case DAL.BloodType.ABMinus:
                    return "VI-";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}