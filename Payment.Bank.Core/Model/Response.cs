using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Bank.Core.Model.Response
{
    public class AmountDetails
    {
        public string authorizedAmount { get; set; }
        public string currency { get; set; }
    }

    public class ErrorInformation
    {
        public string reason { get; set; }
        public string message { get; set; }
    }

    public class AuthReversal
    {
        public string method { get; set; }
        public string href { get; set; }
    }

    public class Avs
    {
        public string code { get; set; }
    }

    public class Capture
    {
        public string method { get; set; }
        public string href { get; set; }
    }

    public class Card
    {
        public string type { get; set; }
    }

    public class CardVerification
    {
        public string resultCode { get; set; }
    }

    public class ClientReferenceInformation
    {
        public string code { get; set; }
    }

    public class Links
    {
        public AuthReversal authReversal { get; set; }
        public Self self { get; set; }
        public Capture capture { get; set; }
    }

    public class OrderInformation
    {
        public AmountDetails amountDetails { get; set; }
    }

    public class PaymentAccountInformation
    {
        public Card card { get; set; }
    }

    public class PaymentInformation
    {
        public TokenizedCard tokenizedCard { get; set; }
        public Card card { get; set; }
    }

    public class PointOfSaleInformation
    {
        public string terminalId { get; set; }
    }

    public class ProcessorInformation
    {
        public string approvalCode { get; set; }
        public CardVerification cardVerification { get; set; }
        public string networkTransactionId { get; set; }
        public string transactionId { get; set; }
        public string responseCode { get; set; }
        public Avs avs { get; set; }
    }

    public class Root
    {
        public Links _links { get; set; }
        public ClientReferenceInformation clientReferenceInformation { get; set; }
        public string id { get; set; }
        public OrderInformation orderInformation { get; set; }
        public PaymentAccountInformation paymentAccountInformation { get; set; }
        public PaymentInformation paymentInformation { get; set; }
        public PointOfSaleInformation pointOfSaleInformation { get; set; }
        public ProcessorInformation processorInformation { get; set; }
        public string reconciliationId { get; set; }
        public string status { get; set; }
        public DateTime submitTimeUtc { get; set; }
        public ErrorInformation errorInformation { get; set; }

    }

    public class Self
    {
        public string method { get; set; }
        public string href { get; set; }
    }

    public class TokenizedCard
    {
        public string type { get; set; }
    }


}
