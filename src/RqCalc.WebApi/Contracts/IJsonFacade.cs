using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

using Framework.ServiceModel.Json;

namespace Rq_Calc.ServiceFacade
{
    [ServiceContract]
    [AsJsonContract(BodyStyle = WebMessageBodyStyle.Bare)]
    public interface IJsonFacade : IFacade
    {
        [OperationContract]
        [WebGet(UriTemplate = "/GetImage?type={type}&id={id}")]
        [ContentType("image/png", "Cache-Control", "public, max-age=31536000")]
        Stream GetImage(string type, int id);
    }
}