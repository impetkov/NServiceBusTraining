using System;
using System.Threading.Tasks;
using NServiceBus;
using static System.Console;

namespace Sales
{
    public class Program
    {
        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Title = "Sales";

            var endpointConfiguration = new EndpointConfiguration("Sales");
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();

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

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(immediate => immediate.NumberOfRetries(0));
            recoverability.Delayed(delayed => delayed.NumberOfRetries(3));
            recoverability.Delayed(delayed => delayed.TimeIncrease(TimeSpan.FromSeconds(3)));

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
            Clear();
            WriteLine("Perss Enter to exit.");
            ReadLine();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
