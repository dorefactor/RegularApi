namespace RegularApi.RabbitMq.Listener
{
    public interface IMessageListener
    {
        void OnMessage(string message);
        string GetConsumerTag(); 
    }
}