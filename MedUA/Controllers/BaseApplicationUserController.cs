namespace MedUA.Controllers
{
    using System.Web;

    using MedUA.Data;
    using MedUA.DAL;

    using Microsoft.AspNet.Identity.Owin;

    public abstract class BaseApplicationUserController : BaseController
    {
        protected BaseApplicationUserController() :base()
        {
        }

        protected BaseApplicationUserController(ApplicationUserManager userManager, SignInManager signInManager, IApplicationDataProvider dataProvider)
            :base(userManager, signInManager)
        {
            DataProvider = dataProvider;
        }

        private IApplicationDataProvider _dataProvider;

        public IApplicationDataProvider DataProvider
        {
            get
            {
                return _dataProvider ?? HttpContext.GetOwinContext().Get<IApplicationDataProvider>();
            }
            private set
            {
                _dataProvider = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._dataProvider != null)
                {
                    this._dataProvider.Dispose();
                    this._dataProvider = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}