using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedUA.Helpers
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class CultureAwareControllerActivator : IControllerActivator
    {
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            //Get the {language} parameter in the RouteData
            string language = requestContext.RouteData.Values["language"]?.ToString() ?? "uk";

            //Get the culture info of the language code
            CultureInfo culture = CultureInfo.GetCultureInfo(language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            return DependencyResolver.Current.GetService(controllerType) as IController;
        }
    }
}