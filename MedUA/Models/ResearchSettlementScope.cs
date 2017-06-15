namespace MedUA.Helpers
{
    using System.ComponentModel.DataAnnotations;

    using MedUA.Resources;

    public enum ResearchSettlementScope
    {
        [Display(ResourceType = typeof(Resource), Name = ("SettlementScope"))]
        Settlement,
        [Display(ResourceType = typeof(Resource), Name = ("RegionScope"))]
        Region,
        [Display(ResourceType = typeof(Resource), Name = ("OblastScope"))]
        Oblast,
        [Display(ResourceType = typeof(Resource), Name = ("CountryScope"))]
        Country
    }
}