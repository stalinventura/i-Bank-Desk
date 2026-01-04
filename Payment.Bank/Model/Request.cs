using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Bank.Model.Request
{
    public class AmountDetails
    {
        public string totalAmount { get; set; }
        public string currency { get; set; }
    }

    public class BillTo
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address1 { get; set; }
        public string postalCode { get; set; }
        public string locality { get; set; }
        public string administrativeArea { get; set; }
        public string country { get; set; }
        public string phoneNumber { get; set; }
        public string company { get; set; }
        public string email { get; set; }
    }

    public class Card
    {
        public string expirationYear { get; set; }
        public string number { get; set; }
        public string securityCode { get; set; }
        public string expirationMonth { get; set; }
    }

    public class ClientReferenceInformation
    {
        public string code { get; set; }
    }

    public class OrderInformation
    {
        public BillTo billTo { get; set; }
        public AmountDetails amountDetails { get; set; }
    }

    public class PaymentInformation
    {
        public Card card { get; set; }
    }

    public class ProcessingInformation
    {
        public string commerceIndicator { get; set; }
    }

    public class Root
    {
        public ClientReferenceInformation clientReferenceInformation { get; set; }
        public ProcessingInformation processingInformation { get; set; }
        public OrderInformation orderInformation { get; set; }
        public PaymentInformation paymentInformation { get; set; }
    }




}
