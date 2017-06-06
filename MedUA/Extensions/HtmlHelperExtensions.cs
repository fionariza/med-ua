namespace MedUA.Helpers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    using MedUA.Models;

    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString PatientHistory(this HtmlHelper<IEnumerable<EntryHistoryViewModel>> htmlHelper, MvcHtmlString name, MvcHtmlString mvcHtmlString)
        {
            var value = "<div class='row'>" +
              "<div class='col-md-2'>" + name.ToHtmlString() + ":</div>" +
                 "<div class='col-md-10'>" + mvcHtmlString.ToHtmlString() +
               "</div></div>";
            return new MvcHtmlString(value);
        }

        public static MvcHtmlString PatientHistory(this HtmlHelper<EntryHistoryViewModel> htmlHelper, MvcHtmlString name, MvcHtmlString mvcHtmlString)
        {
            //var value = "<div class='form-group'>" +
            //  "<div class='col-md-2 control-label'>" + name.ToHtmlString() + ":</div>" +
            //      mvcHtmlString.ToHtmlString() +
            //   "</div>";
            var value = "<div class='form-group'>" + name.ToHtmlString() + mvcHtmlString.ToHtmlString() + "</div>";
            return new MvcHtmlString(value);
        }

        public static MvcHtmlString PatientHistoryWrapWithDiv(this HtmlHelper<EntryHistoryViewModel> htmlHelper, MvcHtmlString name, MvcHtmlString mvcHtmlString)
        {
            var value = "<div class='form-group'>" +
              "<div class='col-md-2 control-label'>" + name.ToHtmlString() + ":</div>" +
                   "<div class='col-md-10 multiline serialize-control'>" + mvcHtmlString.ToHtmlString() +
               "</div></div>";
            return new MvcHtmlString(value);
        }

        public static MvcHtmlString SearchPatientForm(this HtmlHelper htmlHelper, MvcHtmlString name, MvcHtmlString mvcHtmlString)
        {
            var value = "<div class='form-group'>" +
              "<div class='col-md-2'>" + name.ToHtmlString() + "</div>" +
                   "<div class='col-md-10'>" + mvcHtmlString.ToHtmlString() +
               "</div></div>";
            return new MvcHtmlString(value);
        }
    }
}