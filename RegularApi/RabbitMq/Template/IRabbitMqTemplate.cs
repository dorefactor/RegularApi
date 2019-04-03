namespace RegularApi.RabbitMq.Templates
{
    public interface IRabbitMqTemplate
    {
        void SendMessage(string message);
    }
}