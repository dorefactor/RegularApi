using System.Threading.Tasks;

namespace RegularApi.RabbitMq.Listener
{
    public interface IMessageListener
    {
        Task OnMessageAsync(string message);
    }
}