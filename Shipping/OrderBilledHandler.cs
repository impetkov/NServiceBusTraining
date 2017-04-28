using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Shipping
{
    public class OrderBilledHandler : IHandleMessages<OrderBilled>
    {
        private static ILog logger = LogManager.GetLogger<OrderBilledHandler>();

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            logger.Info($"Order has been billed. OrderId: {message.OrderId}. Waiting for sales before shipping order.");

            return Task.CompletedTask;
        }
    }
}