using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Billing
{
    public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
    {
        static ILog logger = LogManager.GetLogger<OrderPlacedHandler>();

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            logger.Info($"Received an OrderPlaced event. OrderId: {message.OrderId}. Charging credit card...");

            var billedOrder = new OrderBilled {OrderId = message.OrderId};

            return context.Publish(billedOrder);
        }
    }
}