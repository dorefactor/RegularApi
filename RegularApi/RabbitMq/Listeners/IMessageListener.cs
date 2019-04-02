namespace RegularApi.RabbitMq.Listeners
{
    public interface IMessageListener
    {
        void OnMessage(string message);
        string GetConsumerTag(); 
    }
}