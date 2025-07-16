using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Rq_Calc.ServiceFacade
{
    public class JsonFacadeFactory : ServiceHostFactory
    {
        private readonly Func<ServiceFacadeEnvironment> _getEnvironment;


        public JsonFacadeFactory()
            : this(() => ServiceFacadeEnvironment.Configuration)
        {

        }

        public JsonFacadeFactory(Func<ServiceFacadeEnvironment> getEnvironment)
        {
            if (getEnvironment == null) throw new ArgumentNullException(nameof(getEnvironment));

            this._getEnvironment = getEnvironment;
        }


        public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        {
            try
            {
                return new ServiceHost(new JsonFacade(this._getEnvironment()), baseAddresses);
            }
            catch
            {
                System.Web.HttpRuntime.UnloadAppDomain();
                throw;
            }
        }
    }
}