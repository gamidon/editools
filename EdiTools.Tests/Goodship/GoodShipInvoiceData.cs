using System;
using System.Collections.Generic;
using System.Linq;

namespace EdiTools.Tests
{
    public class GoodShipInvoiceData
    {
        public DateTime EdiDateTime { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime NetAmountDate { get; internal set; }
        public DateTime InvoiceDate { get; set; }
        public InvoiceNumber InvoiceNumber { get; set; }
        public BillOfLadingNumber ShipmentNumber { get; set; }
        public decimal NetAmountDue { get; internal set; }
        public CurrencyCode CurrencyCode { get; set; }  //Always USD for Goodship?
        public PaymentMethod PaymentMethod { get; set; }
        public WeightUnit WeightUnit { get; set; }
        public ReferenceIdentificationQualifier ReferenceIdentificationQualifier { get; set; }
        public ReferenceIdentification ReferenceIdentification { get; set; }
        public TransportationMode TransportationMode { get; set; }
        public OrganizationName SellingPartyNameCarrier { get; set; }
        public IdentificationCodeQualifier SellingPartyIdentificationCodeQualifier { get; set; }
        public OrganizationName BuyingPartyNameShipper { get; set; }
        public IdentificationCodeQualifier BuyingPartyIdentificationCodeQualifier { get; set; }
        public Organization OrgSE { get; set; }
        public Organization OrgBY { get; set; }
        public Organization OrgSF { get; set; }
        public Organization OrgST { get; set; }
        public List<InvoiceElements> InvoiceItems { get; set; }
        public decimal TotalChargeSum => InvoiceItems.Sum(s => s.Charge);
        public decimal TotalWeightSum => InvoiceItems.Sum(s => s.Weight); //What if weights are different L vs KG... Need to normalize
        public WeighQualifier WeightQualifier { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal TotalCharge { get; set; }
        public class InvoiceElements
        {
            public LadingDescription LadingDescription { get; set; } //L5
            public decimal Weight { get; set; } //L0
            public WeighQualifier WeighQualifier { get; set; } //L0
            public int Quantity { get; set; } //L0
            public PackagingForm PackagingForm { get; set; } //L0
            public decimal BilledRatedAsQuantity { get; set; } //L0
            public BilledRateQualifier BilledRateQualifier { get; set; } //L0
            public decimal FreightRate { get; set; } //L1
            public FreightRateQualifier FreightRateQualifier { get; set; } //L1
            public decimal Charge { get; set; } //L1
            public SpecialChargeAllowanceCode SpecialChargeAllowanceCode { get; set; } //L1
        }
    }
}