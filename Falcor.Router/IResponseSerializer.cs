using System.Threading.Tasks;

namespace Falcor.Router
{
    public interface IResponseSerializer
    {
        string Serialize(Response response);
    }
}