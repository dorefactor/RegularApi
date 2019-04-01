namespace RegularApi.RabbitMq.Listeners
{
    public interface IRabbitMqMessageListener
    {
        void OnMessage(string message);
    }
}