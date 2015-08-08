using System.Threading.Tasks;

namespace Falcor.Server
{
    public interface IResponseSerializer
    {
        string Serialize(Response response);
    }
}