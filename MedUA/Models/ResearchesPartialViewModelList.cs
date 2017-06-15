
namespace MedUA.Models
{
    using System.Collections.Generic;

    public class ResearchesPartialViewModelList : List<ResearchesPartialViewModel>
    {
        public ResearchesPartialViewModelList()
        {

        }

        public ResearchesPartialViewModelList(IEnumerable<ResearchesPartialViewModel> values) : base(values)
        {

        }
        public string PatientId { get; set; }
    }
}