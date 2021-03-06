﻿using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Sales.Handlers
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        private static ILog logger = LogManager.GetLogger<PlaceOrderHandler>();

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            logger.Info($"Receiver a PlaceOrder message with OrderId {message.OrderId}.");

            //throw new Exception("Dummy exception");

            var placedOrder = new OrderPlaced {OrderId = message.OrderId};

            return context.Publish(placedOrder);
        }
    }
}