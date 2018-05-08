using System;
using System.Collections.Generic;

namespace EdiTools.Tests
{
    public class EdiSegmentB3 : EdiSegment
    {
        public EdiSegmentB3(InvoiceNumber invoiceNumber, BillOfLadingNumber shipmentIdNumber, PaymentMethod pm,
                            WeightUnit wu, DateTime invoiceDate, Decimal netAmountDue,
                            DateTime deliveryDate)
            : base("B3")
        {
            this[01] = "";
            this[02] = invoiceNumber.ToString();
            this[03] = shipmentIdNumber.ToString();
            this[04] = pm.ToString();
            this[05] = wu.ToString();
            this[06] = EdiValue.Date(8, invoiceDate);
            if (netAmountDue.ToString().Length > 12)
            {
                throw new ArgumentException(nameof(netAmountDue));
            }
            this[07] = EdiValue.Numeric(2, netAmountDue);
            this[08] = "";
            this[09] = EdiValue.Date(8, deliveryDate);
            this[10] = "035";   //Delivered Date
            this[11] = Info.GoodShipId;
        }
    }
    public class EdiSegmentC3 : EdiSegment
    {
        public EdiSegmentC3(CurrencyCode cc)
            : base("C3")
        {
            this[01] = cc.ToString();
        }
    }
    public class EdiSegmentDtm: EdiSegment
    {     
        public EdiSegmentDtm(DateTimeQualifier dateTimeQualifier, DateTime dateTime)
            : base("DTM")
        {
            this[01] = ((int)dateTimeQualifier).ToString();
            this[02] = EdiValue.Date(8, dateTime);
            this[03] = EdiValue.Time(4,dateTime);
        }
    }
    public class EdiSegmentG62 : EdiSegment
    {
        public EdiSegmentG62(DateQualifier dateQualifier, DateTime date)
            : base("G62")
        {
            int val = (int) dateQualifier;
            this[01] = val.ToString();
            this[02] = EdiValue.Date(8, date);
        }
    }
    public class EdiSegmentGe : EdiSegment
    {
        public EdiSegmentGe(int transactionSets, int groupControlNumber)
            : base("GE")
        {
            this[01] = transactionSets.ToString();
            this[02] = groupControlNumber.ToString().PadLeft(9, '0');
        }
    }
    public class EdiSegmentGs : EdiSegment
    {
        public EdiSegmentGs(DateTime dateTime, int groupControlNumber)
            : base("GS")
        {
            this[01] = "IM";
            this[02] = Info.GoodShipId;
            this[03] = Info.DexterAxelId;
            this[04] = EdiValue.Date(8, dateTime);
            this[05] = EdiValue.Time(6, dateTime);
            this[06] = groupControlNumber.ToString().PadLeft(9, '0');
            this[07] = "X";
            this[08] = "004010";
        }
    }
    public class EdiSegmentIsa : EdiSegment
    {
        public EdiSegmentIsa(DateTime dateTime, int controlNumber, bool requestAck, bool production = false)
            : base("ISA")
        {
            if (controlNumber.ToString().Length > 9)
            {
                throw new ArgumentException($"{nameof(controlNumber)} must be less than 10 digits", nameof(controlNumber));
            }
            this[01] = "00"; //Authorization Information Qualifier
            this[02] = Info.AuthInfo.PadRight(10);
            this[03] = "00"; //Security Information Qualifier
            this[04] = "".PadRight(10);
            this[05] = "02"; //Interchange ID Qualifier
            this[06] = Info.GoodShipId.PadRight(15);
            this[07] = "12"; //Interchange Receiver ID
            this[08] = Info.DexterAxelId.PadRight(15);
            this[09] = EdiValue.Date(6, dateTime);
            this[10] = EdiValue.Time(4, dateTime);
            this[11] = "U"; //Interchange Control Standards Identifier
            this[12] = "00401";  //Interchange Control Version Number
            this[13] = controlNumber.ToString().PadLeft(9, '0');
            this[14] = (requestAck) ? "1" : "0";
            this[15] = (production) ? "P" : "T";
            this[16] = "|";
        }
    }
    public class EdiSegmentIea : EdiSegment
    {
        public EdiSegmentIea(int functionalGroups, int interchangeControlNumber)
            : base("IEA")
        {
            this[01] = functionalGroups.ToString();
            this[02] = interchangeControlNumber.ToString().PadLeft(9, '0');
        }
    }
    public class EdiSegmentL0 : EdiSegment
    {
        public EdiSegmentL0(int assignedNumber, decimal weight, WeighQualifier wq,
                            int ladingQuantity, PackagingForm pf)
            : base("L0")
        {
            if (ladingQuantity.ToString().Length > 7)
            {
                throw new ArgumentException(nameof(ladingQuantity));
            }
            this[01] = assignedNumber.ToString();
            this[04] = EdiValue.Real(weight);
            this[05] = wq.ToString();
            this[08] = ladingQuantity.ToString();
            this[09] = pf.ToString();
        }
        public EdiSegmentL0(int assignedNumber, decimal billedRatedAsQuantity, BilledRateQualifier bq)
             : base("L0")
        {
            if (billedRatedAsQuantity.ToString().Length > 11 || billedRatedAsQuantity < 1)
            {
                throw new ArgumentException(nameof(billedRatedAsQuantity));
            }
            this[01] = assignedNumber.ToString();
            this[02] = EdiValue.Real(billedRatedAsQuantity);
            this[03] = bq.ToString();
        }
    }
    public class EdiSegmentL1 : EdiSegment
    {
        public EdiSegmentL1(int assignedNumber, decimal freightRate, FreightRateQualifier frq, decimal charge)
            : base("L1")
        {
            this[01] = assignedNumber.ToString();
            this[02] = EdiValue.Real(freightRate);
            this[03] = frq.ToString();
            this[04] = EdiValue.Numeric(2, charge);
        }
        public EdiSegmentL1(int assignedNumber, decimal freightRate, FreightRateQualifier frq, decimal charge, SpecialChargeAllowanceCode specialChargeAllowanceCode)
            : base("L1")
        {
            this[01] = assignedNumber.ToString();
            this[02] = EdiValue.Real(freightRate);
            this[03] = frq.ToString();
            this[04] = EdiValue.Numeric(2, charge);
            this[08] = specialChargeAllowanceCode.ToString();
        }
    }
    public class EdiSegmentL3 : EdiSegment
    {
        public EdiSegmentL3(decimal weight, WeighQualifier wq, decimal charge, WeightUnit weightUnit)
            : base("L3")
        {
            this[01] = EdiValue.Real(weight);
            this[02] = wq.ToString();
            this[05] = EdiValue.Numeric(2, charge);
            this[12] = weightUnit.ToString();
        }
    }
    public class EdiSegmentL5 : EdiSegment
    {
        public EdiSegmentL5(int assignedNumber, LadingDescription description)
            : base("L5")
        {
            this[01] = assignedNumber.ToString();
            this[02] = description.ToString();
        }
    }
    public class EdiSegmentLx : EdiSegment
    {
        public EdiSegmentLx(int assignedNumber)
            : base("LX")
        {
            this[01] = assignedNumber.ToString();
        }
    }
    public class LxSection
    {
        private readonly List<EdiSegment> _segments = new List<EdiSegment>();
        public LxSection(int sectionNumber, EdiSegmentL5 l5, EdiSegmentL0 l0, EdiSegmentL1 l1)
        {
            _segments.Add(new EdiSegmentLx(sectionNumber));
            _segments.Add(l5);
            _segments.Add(l0);
            _segments.Add(l1);
        }
        public List<EdiSegment> GetSegments => _segments;
    }
    public class EdiSegmentN1 : EdiSegment
    {
        public EdiSegmentN1(EntityIdentifierCode entityIdentifierCode, Organization organization)
            : base("N1")
        {
            this[01] = entityIdentifierCode.ToString();
            this[02] = organization.Name.ToString();
        }
        public EdiSegmentN1(EntityIdentifierCode entityIdentifierCode, Organization organization,
                            IdentificationCodeQualifier identificationCodeQualifier,
                            IdentificationCode identificationCode)
            : base("N1")
        {
            this[01] = entityIdentifierCode.ToString();
            this[02] = organization.Name.ToString();
            this[03] = ((int)identificationCodeQualifier).ToString();
            this[04] = identificationCode.ToString();
        }
    }
    public class EdiSegmentN3 : EdiSegment
    {
        public EdiSegmentN3(Organization organization)
            : base("N3")
        {
            this[01] = organization.AddressInformation1.ToString();
            this[02] = organization.AddressInformation2.ToString();
        }
    }
    public class EdiSegmentN4 : EdiSegment
    {
        public EdiSegmentN4(Organization organization)
            : base("N4")
        {
            this[01] = organization.City.ToString();
            this[02] = organization.StateOrProvince.ToString();
            this[03] = organization.PostalCode.ToString();
        }
 
    }
    public class EdiSegmentN7 : EdiSegment
    {
        public EdiSegmentN7(EquipmentInitial equipmentInitial, EquipmentNumber equipmentNumber, EdiValueR_1_10 weight,
                            WeighQualifier weighQualifier, EdiValueN0_3_8 tareWeight, EdiValueR_1_8 volume, 
                            VolumeUnitQualifier volumeUnitQualifier)
            : base("N7")
        {
            this[01] = equipmentInitial.ToString();
            this[02] = equipmentNumber.ToString();
            this[03] = weight.ToString();
            this[04] = weighQualifier.ToString();
            this[05] = tareWeight.ToString();
            this[08] = volume.ToString();
            this[12] = volumeUnitQualifier.ToString();
        }
    }
    public class EdiSegmentN9_4C : EdiSegment
    {
        public EdiSegmentN9_4C(ReferenceIndicationInboundOutbound referenceIdentification)
            : base("N9")
        {
            this[01] = "4C";
            this[02] = referenceIdentification.ToString();
        }
    }
    public class EdiSegmentR2 : EdiSegment
    {
        public EdiSegmentR2(StandardCarrierCode standardCarrierCode, RoutingSequenceCode routingSequenceCode,
                            TypeOfServiceCode typeOfServiceCode)
            : base("R2")
        {
            this[01] = standardCarrierCode.ToString();
            this[02] = routingSequenceCode.ToString();
            this[12] = typeOfServiceCode.ToString();
        }
    }
    public class EdiSegmentR3 : EdiSegment
    {
        public EdiSegmentR3(TransportationMode transportationMode)
            : base("R3")
        {
            this[01] = Info.GoodShipId;
            this[02] = "B";
            this[03] = "";
            this[04] = transportationMode.ToString();
        }
    }
    public class EdiSegmentR4 : EdiSegment
    {
        public EdiSegmentR4(PortOrTerminalCode portOrTerminalCode, LocationQualifier locationQualifier, 
                            LocationIdentifier locationIdentifier, PortName portName, CountryCode countryCode, 
                            TerminalName terminalName,StateOrProvince stateOrProvince)
            : base("R4")
        {
            this[01] = portOrTerminalCode.ToString();
            this[02] = locationIdentifier.ToString();
            this[03] = locationIdentifier.ToString();
            this[04] = portName.ToString();
            this[05] = countryCode.ToString();
            this[06] = terminalName.ToString();
            this[08] = stateOrProvince.ToString();
        }
    }
    public class EdiSegmentSt : EdiSegment
    {
        public EdiSegmentSt(int TransactionSetControlNumber, TransactionSetIdentifierCode TransactionSetIdentifierCode)
            : base("ST")
        {
            var controlNumber = TransactionSetControlNumber.ToString();
            if (controlNumber.Length < 4) { controlNumber = controlNumber.PadLeft(4, '0'); }
            if (controlNumber.Length > 9)
            {
                throw new ArgumentException("TransactionSetControlNumber must be less than 9 digits", nameof(TransactionSetControlNumber));
            }

            this[01] = ((int)TransactionSetIdentifierCode).ToString();
            this[02] = controlNumber;
        }
    }
    public class EdiSegmentSe : EdiSegment
    {
        public EdiSegmentSe(int TransactionSetControlNumber)
            : base("SE")
        {
            var controlNumber = TransactionSetControlNumber.ToString();
            if (controlNumber.Length < 4) { controlNumber = controlNumber.PadLeft(4, '0'); }
            if (controlNumber.Length > 9)
            {
                throw new ArgumentException("TransactionSetControlNumber must be less than 9 digits", nameof(TransactionSetControlNumber));
            }
            this[01] = "2";
            this[02] = controlNumber;
        }
        public int SetSegmentCount
        {
            set
            {
                if (value <= 1) //ST segment has to be there so it should be more than that 
                {
                    throw new ArgumentException("segmentCount has to be greater than 1", nameof(SetSegmentCount));
                }
                //Add one to segment count for this segment about to be added.
                var segments = value.ToString();
                if (segments.Length > 10)
                {
                    throw new ArgumentException("segmentCount must be less than 10 digits", nameof(SetSegmentCount));
                }
                this[01] = segments;
            }
        }
    }
    public class EdiSegmentV1 : EdiSegment
    {
        public EdiSegmentV1(VesselCode vesselCode, VesselName vesselName, CountryCode countryCode, FlightOrVoyageNumber flightOrVoyageNumber,TransportationMethodOrTypeCode typeCode = TransportationMethodOrTypeCode.O)
            : base("V1")
        {
            this[01] = vesselCode.ToString();
            this[02] = vesselName.ToString();
            this[03] = countryCode.ToString();
            this[04] = flightOrVoyageNumber.ToString();
            this[09] = typeCode.ToString();
        }
    }
}
