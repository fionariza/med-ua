namespace MedUA.Helpers
{
    using System;

    using MedUA.DAL;
    public static class BloodTypeSelector
    {
        public static string GetName(BloodType bloodType)
        {
            switch (bloodType)
            {
                case DAL.BloodType.OPlus:
                    return BloodTypeSelectorNames.OPlus;
                case DAL.BloodType.APlus:
                    return BloodTypeSelectorNames.APlus;
                case DAL.BloodType.BPlus:
                    return BloodTypeSelectorNames.BPlus;
                case DAL.BloodType.ABPlus:
                    return BloodTypeSelectorNames.ABPlus;
                case DAL.BloodType.OMinus:
                    return BloodTypeSelectorNames.OMinus;
                case DAL.BloodType.AMinus:
                    return BloodTypeSelectorNames.AMinus;
                case DAL.BloodType.BMinus:
                    return BloodTypeSelectorNames.BMinus;
                case DAL.BloodType.ABMinus:
                    return BloodTypeSelectorNames.ABMinus;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}