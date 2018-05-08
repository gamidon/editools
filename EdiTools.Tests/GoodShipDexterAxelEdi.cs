using System;
using System.Collections.Generic;
using System.Linq;

namespace EdiTools.Tests
{
    public class GoodShipDexterAxelEdi
    {
        private GoodShipDexterAxelEdi(DateTime dateTime, int controlNumber, int groupControlNumber, bool requestAck)
        {
            //Note ControlNumber and GroupControlNumber should be unique values...... 

            _ediDateTime = dateTime;
            _controlNumber = controlNumber;
            _groupControlNumber = groupControlNumber;
            _requestAck = requestAck;
        }
        public string AuthInfo => Info.AuthInfo;
        public string ReceiverId => Info.DexterAxelId;
        public string SenderId => Info.GoodShipId;
        private readonly int _controlNumber;
        private readonly int _groupControlNumber;
        private readonly bool _requestAck;
        private readonly DateTime _ediDateTime;

        public static GoodShipDexterAxelEdi EdiStart(DateTime dateTime, int controlNumber, int groupControlNumber, bool requestAck)
            => new GoodShipDexterAxelEdi(dateTime, controlNumber, groupControlNumber, requestAck)
            {
                Isa = new EdiSegmentIsa(dateTime, controlNumber, false),
                Gs = new EdiSegmentGs(dateTime, groupControlNumber)
            };
        public void EdiEnd()
        {
            Ge = new EdiSegmentGe(TransactionSets.Count, _groupControlNumber);
            Iea = new EdiSegmentIea(functionalGroups: 1, interchangeControlNumber: _controlNumber);
        }
        public EdiSegmentIsa Isa { get; set; }
        public EdiSegmentGs Gs { get; set; }
        public EdiSegmentGe Ge { get; set; }
        public EdiSegmentIea Iea { get; set; }
        public List<TransactionSet> TransactionSets { get; set; } = new List<TransactionSet>();
        public string ToEdi() => GenerateEdi().ToString();
        public void Save(string pathAndName) => GenerateEdi().Save(pathAndName);
        public int GetNextTransactionSetControlNumber => TransactionSets.Count + 1;
        private EdiDocument GenerateEdi()
        {

            if (
                TransactionSets.GroupBy(x => x.TransactionSetControlNumber).Any(g => g.Count() > 1)
                || TransactionSets.Any(x => x.TransactionSetControlNumber == 0))
            {
                throw new ArgumentException("Bad Control Numbers Detected", nameof(TransactionSets));
            }
            var ediOptions = new EdiOptions
            {
                AddLineBreaks = true,
                SegmentTerminator = '\r',
                ElementSeparator = '*'
            };
            var ediDocument = new EdiDocument(ediOptions);
            ediDocument.Segments.Add(Isa);
            ediDocument.Segments.Add(Gs);
            foreach (var seg in TransactionSets.SelectMany(s => s.ToEdi()))
            {
                ediDocument.Segments.Add(seg);
            }
            ediDocument.Segments.Add(Ge);
            ediDocument.Segments.Add(Iea);

            return ediDocument;
        }
    }
    public class TransactionSet
    {
        private readonly List<EdiSegment> _segments = new List<EdiSegment>();
        private readonly int _transactionSetControlNumber;
        private bool _listClosed = false;
        private TransactionSet(int transactionSetControlNumber)
        {
            _transactionSetControlNumber = transactionSetControlNumber;
        }
        public void Add(EdiSegment segment)
        {
            if (segment == null)
            {
                throw new ArgumentNullException(nameof(segment));
            }
            if (_listClosed)
            {
                throw new IndexOutOfRangeException("Segment already has a terminator SE");
            }
            //First Must be ST
            if (_segments.Count == 0)
            {
                var s = segment as EdiSegmentSt;
                if (s == null)
                {
                    throw new IndexOutOfRangeException("Segments must start with ST");
                }
            }
            _segments.Add(segment);
            if (segment is EdiSegmentSe seg)
            {
                seg.SetSegmentCount = _segments.Count;
                _listClosed = true;
            }
        }
        public void Add(LxSection lxSection)
        {
            foreach (var item in lxSection.GetSegments)
            {
                Add(item);
            }
        }
        public List<EdiSegment> ToEdi()
        {
            if (!_listClosed || _segments.Count < 3)
            {
                throw new IndexOutOfRangeException("Must be more than Just ST SE in Segments, Must Contain ST SE");
            }
            return _segments;
        }
        public int TransactionSetControlNumber => _transactionSetControlNumber;
        public static TransactionSet Start(int transactionSetControlNumber)
        {
            var transactionset = new TransactionSet(transactionSetControlNumber);
            transactionset.Add(new EdiSegmentSt(transactionSetControlNumber, TransactionSetIdentifierCode.OceanFreightReceiptAndInvoice));
            return transactionset;
        }
        public void End() => Add(new EdiSegmentSe(_transactionSetControlNumber));
    }
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
        public List<InvoiceElements> Invoices { get; set; }
        public decimal TotalChargeSum => Invoices.Sum(s => s.Charge);
        public decimal TotalWeightSum => Invoices.Sum(s => s.Weight); //What if weights are different L vs KG... Need to normalize
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
    public class Organization
    {
        public Organization(OrganizationName name, AddressInformation addressInformation1, AddressInformation addressInformation2, CityName city, StateOrProvince stateOrProvince, PostalCode postalCode, CountryCode countryCode)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            AddressInformation1 = addressInformation1 ?? throw new ArgumentNullException(nameof(addressInformation1));
            AddressInformation2 = addressInformation2 ?? throw new ArgumentNullException(nameof(addressInformation2));
            City = city ?? throw new ArgumentNullException(nameof(city));
            StateOrProvince = stateOrProvince ?? throw new ArgumentNullException(nameof(stateOrProvince));
            PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
            CountryCode = countryCode ?? throw new ArgumentNullException(nameof(countryCode));
        }
        public OrganizationName Name { get; }
        public AddressInformation AddressInformation1 { get; }
        public AddressInformation AddressInformation2 { get; }
        public CityName City { get; }
        public StateOrProvince StateOrProvince { get; }
        public PostalCode PostalCode { get; }
        public CountryCode CountryCode { get; }
    }
}