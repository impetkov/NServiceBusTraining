using System;
using static System.Console;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace ClientUI
{
    internal class Program
    {
        private static ILog logger = LogManager.GetLogger<Program>();

        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Title = "ClientUI";

            var endpointConfiguration = new EndpointConfiguration("ClientUI");

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();

            endpointConfiguration.License(
                @"<license id=""d0159c96-ab45-4a24-b289-639c0996958d"" expiration=""9999-12-31T23:59:59.9999999"" type=""Standard"" LicenseVersion=""3.3"" LicenseType=""Standard"" WorkerThreads=""Max"" MaxMessageThroughputPerSecond=""Max"" AllowedNumberOfWorkerNodes=""Max"" ProductType=""Perpetual"" AllowedCores=""4"">
  <name>Insurecom Ltd</name>
  <Signature xmlns=""http://www.w3.org/2000/09/xmldsig#"">
    <SignedInfo>
      <CanonicalizationMethod Algorithm=""http://www.w3.org/TR/2001/REC-xml-c14n-20010315"" />
      <SignatureMethod Algorithm=""http://www.w3.org/2000/09/xmldsig#rsa-sha1"" />
      <Reference URI="""">
        <Transforms>
          <Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" />
        </Transforms>
        <DigestMethod Algorithm=""http://www.w3.org/2000/09/xmldsig#sha1"" />
        <DigestValue>rHkJf3NAVlnAJQE9NcGsO5vQTZI=</DigestValue>
      </Reference>
    </SignedInfo>
    <SignatureValue>zJteJlapcGr5T7Wk2J6SaYXt+hIrxKzaLoI0iSWREUsr0LbCTNpQsTwGNj4zgsUpNqhhtl9i5TThQSTQGzB+k1hqfiFvIAYrbaptThhkZ5EBydb4I+UzeVJYXYVb/wAcyYF0ez1ihX0laJwzDfUbLLMnAJg6W87QpKhQYgUpawo=</SignatureValue>
  </Signature>
</license>");

            var endpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
            Clear();
            await RunLoop(endpoint);

            await endpoint.Stop().ConfigureAwait(false);
        }

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            while (true)
            {
                logger.Info("Press P to place an order ot Q to quit");
                var key = ReadKey();
                WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        var command = new PlaceOrder
                        {
                            OrderId = Guid.NewGuid().ToString()
                        };

                        logger.Info($"Sending PlaceOrder command with OrderId {command.OrderId}");

                        await endpointInstance.Send(command).ConfigureAwait(false);

                        break;
                    case ConsoleKey.Q:
                        return;
                    default:
                        logger.Info("Invalid input.");
                        break;
                }
            }
        }
    }
}
