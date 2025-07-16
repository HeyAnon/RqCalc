using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;

using Framework.Core;

namespace Rq_Calc.ServiceFacade
{
    public class JsonFacade : Facade, IJsonFacade
    {
        public JsonFacade(ServiceFacadeEnvironment environment)
            : base(environment)
        {
        }


        public Stream GetImage(string type, int id)
        {
            var image = this.Environment.ImageSourceService.GetImageSource(type).GetById(id).FromMaybe(() => new ArgumentException($"{type} with id = {id} not found"));

            return new MemoryStream(image.Data);
        }
        

        protected override FaultException GetFaultException(Exception ex)
        {
            return new WebFaultException<string>(ex.ToString(), HttpStatusCode.Conflict);
        }
    }
}