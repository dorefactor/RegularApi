namespace RegularApi.RabbitMq.Templates
{
    public interface IRabbitMqTemplate
    {
        void SendMessage(string exchange, string queue, string message);
    }
}