namespace BackendService.Application.RabbitMq
{
    public interface IProducer
    {
        public void SendDetailMessage<T>(T message);
    }
}
