using System;

namespace EdiTools.Tests
{
    public enum TransactionSetIdentifierCode
    {
        MotorCarrierFreight = 210,
        OceanFreightReceiptAndInvoice = 310
    }
    public enum PaymentMethod
    {
        CC,
        PP,
        TP
    }
    public enum WeightUnit
    {
        K, //Kilograms
        L,  //pounds
        E, //Metric Ton
        M // Measurement Ton
    }
    public enum WeighQualifier
    {
        A1,// Dimensional Weight
        B,//Billed Weight
        F,//Deficit Weight
        FR,// Freight Weight
        G,//Gross Weight
        Z, //Mutually Defined
        N  //Acrtual Net Weight
    }
    public enum PackagingForm
    {
        BAG,// Bag
        BBL,// Barrel
        BOX,// Box
        CTN,// Carton
        DRM,// Drum
        PCS,// Pieces
        PKG,// Package
        PLT,// Pallet
        SKD,// Skid
        TNK// Tank
    }
    public enum BilledRateQualifier
    {
        BA, // Barrels
        BX, // Box
        DM, // Miles
        EA, // Each
        FR, // Flat Rate
        GL, // Gallon
        KG, // Kilogram
        LB, // Pound
        LR, // Liter
        NC  // Car
    }
    public enum FreightRateQualifier
    {
        FR, //Flat Rate
        KG, //Per Kilograms
        LB, //Per Pound
        PC, //Per Car
        PF, //Per Cubic Foot
        PG, //Per Gallon
        PM, //Per Mile
        PU, //Per Unit
        PH, // on sample not sure if valid?
        AV // on sample not sure if valid?
    }
    public enum CurrencyCode
    {
        CAD,
        CHF,
        CZK,
        DKK,
        EUR,
        GBP,
        HUF,
        NOK,
        PLN,
        SEK,
        SGD,
        USD,
        ZAR,
    }
    public enum TransportationMode
    {
        E,
        FL,
        J,
        LT,
        M,
        MP,
        SC,
        TA,
        TT,
        X
    }
    public enum EntityIdentifierCode
    {
        CA,
        SE,
        BY,
        SF,
        ST
    }
    public enum LocationQualifier
    {
        UN,
        ZZ
    }
    public enum LocationIdentifier
    {
        KE, //Port of Embarkation
        KL, //Port of Loading
        PA, //Port of Arrival
        PB, //Port of Discharge
        PE //Port of Entry
    }
    public enum PortOrTerminalCode
    {
        O, //Origin (Operational) Shipper’s facility at which shipment begins its movement at cargo’s expense.
        J, //Bill of Lading Port of Loading (Contractual) Place at which cargo is loaded on board the means of transport
        L, //Port of Loading (Operational) Port at which cargo is loaded on vessel.
        D, //Port of Discharge (Operational) Port at which cargo is unloaded from vessel.
        E //Place of Delivery (Contractual)Place at which cargo leaves its care and custody of carrier.
    }
    public enum DateQualifier
    {
        PickupDate = 86,
        DeliveredDate = 35
    }
    public enum IdentificationCodeQualifier
    {
        GoodShip = 2,
        DexterAxel = 92,
        FA //Other Party
    }
    public enum ReferenceIndicationInboundOutbound
    {
        I, //Inbound
        O // Outbound
    }
    public enum TransportationMethodOrTypeCode
    {
        O, //Containerized Ocean (Default, if not provided)
        S, //Straight-up Ocean
        C // Consolidation
    }
    public enum DateTimeQualifier
    {
        Estimated = 139,
        Actual = 140
    }
    public enum RoutingSequenceCode
    {
        B  //Origin/Delivery Carrier (Any Mode)
    }
    public enum TypeOfServiceCode
    {
        DD, // Door to Door
        DR, // Door to Ramp
        HP, // House-to-pier
        PH, // Pier-to-house
        PP, // Pier-to-pier
        RR  // Roll-on Roll-off
    }
    public enum VolumeUnitQualifier
    {
        E, //Cubic Feet
        G, //Gallons
        M, //Cubic Decimeters
        V, //Liter
        X //Cubic Meters
    }
    public class EdiValueN0_3_8 : EdiValue
    {
        public override string Value { get; set; }
        public EdiValueN0_3_8(decimal value)
        {
            string formatted = Math.Abs(value).ToString("f0").Replace(".", string.Empty).TrimStart('0');
            if (formatted == string.Empty || formatted.Length > 8)
                throw new ArgumentException(nameof(value));
            formatted = formatted.PadLeft(3, '0');
            Value = (value < 0) ? "-" + formatted : formatted;
        }
        public override string ToString() => Value;
    }
    public class EdiValueR_1_8
    {
        public string Value { get; set; }
        public EdiValueR_1_8(decimal value)
        {
            Value = EdiValue.Real(value);
            if (Value == string.Empty || Value.Length > 8 || Value.Length < 1)
                throw new ArgumentException(nameof(value));
        }
        public override string ToString() => Value;
    }
    public class EdiValueR_1_10
    {
        public string Value { get; set; }
        public EdiValueR_1_10(decimal value)
        {
            Value = EdiValue.Real(value);
            if (Value == string.Empty || Value.Length > 10 || Value.Length < 1)
                throw new ArgumentException(nameof(value));
        }
        public override string ToString() => Value;
    }
    public class EquipmentType
    {
        private readonly string _equipmentType;
        public EquipmentType(string equipmentType)
        {
            if (string.IsNullOrEmpty(equipmentType))
            {
                throw new ArgumentNullException(nameof(equipmentType));
            }
            string type = string.Empty;
            if (equipmentType.Contains("20"))
                type= "20OC";
            if (equipmentType.Contains("40"))
                type = "40OC";
            if (equipmentType.Contains("45"))
                type = "45OC";

            if (equipmentType.Length > 1 || equipmentType.Length < 4)
            {
                throw new ArgumentException(nameof(equipmentType));
            }
            _equipmentType = equipmentType;
        }
        public override string ToString() => _equipmentType;
    }
    public class EquipmentInitial
    {
        private readonly string _equipmentInitial;
        public EquipmentInitial(string equipmentInitial)
        {
            if (string.IsNullOrEmpty(equipmentInitial))
            {
                throw new ArgumentNullException(nameof(equipmentInitial));
            }
            if (equipmentInitial.Length > 1 || equipmentInitial.Length < 4)
            {
                throw new ArgumentException(nameof(equipmentInitial));
            }
            _equipmentInitial = equipmentInitial;
        }
        public override string ToString() => _equipmentInitial;
    }
    public class EquipmentNumber
    {
        private readonly string _equipmentNumber;
        public EquipmentNumber(string equipmentNumber)
        {
            if (string.IsNullOrEmpty(equipmentNumber))
            {
                throw new ArgumentNullException(nameof(equipmentNumber));
            }
            if (equipmentNumber.Length > 1 || equipmentNumber.Length < 10)
            {
                throw new ArgumentException(nameof(equipmentNumber));
            }
            _equipmentNumber = equipmentNumber;
        }
        public override string ToString() => _equipmentNumber;
    }
    public class PortName
    {
        private readonly string _portName;
        public PortName(string portName)
        {
            if (string.IsNullOrEmpty(portName))
            {
                throw new ArgumentNullException(nameof(portName));
            }
            if (portName.Length > 24 || portName.Length < 2)
            {
                throw new ArgumentException(nameof(portName));
            }
            _portName = portName;
        }
        public override string ToString() => _portName;
    }
    public class TerminalName
    {
        private readonly string _terminalName;
        public TerminalName(string terminalName)
        {
            if (string.IsNullOrEmpty(terminalName))
            {
                throw new ArgumentNullException(nameof(terminalName));
            }
            if (terminalName.Length > 30 || terminalName.Length < 2)
            {
                throw new ArgumentException(nameof(terminalName));
            }
            _terminalName = terminalName;
        }
        public override string ToString() => _terminalName;
    }
    public class VesselCode
    {
        private readonly string _vesselCode;
        public VesselCode(string vesselCode)
        {
            if (string.IsNullOrEmpty(vesselCode))
            {
                throw new ArgumentNullException(nameof(vesselCode));
            }
            if (vesselCode.Length > 8)
            {
                throw new ArgumentException(nameof(vesselCode));
            }
            _vesselCode = vesselCode;
        }
        public override string ToString() => _vesselCode;
    }
    public class OceanCarrierCode
    {
        private readonly string _oceanCarrierCode;
        public OceanCarrierCode(string oceanCarrierCode)
        {
            if (string.IsNullOrEmpty(oceanCarrierCode))
            {
                throw new ArgumentNullException(nameof(oceanCarrierCode));
            }
            if (oceanCarrierCode.Length > 8)
            {
                throw new ArgumentException(nameof(oceanCarrierCode));
            }
            _oceanCarrierCode = oceanCarrierCode;
        }
        public override string ToString() => _oceanCarrierCode;
    }
    public class VesselName
    {
        private readonly string _vesselName;
        public VesselName(string vesselName)
        {
            if (string.IsNullOrEmpty(vesselName))
            {
                throw new ArgumentNullException(nameof(vesselName));
            }
            if (vesselName.Length < 2 || vesselName.Length > 28)
            {
                throw new ArgumentException(nameof(vesselName));
            }
            _vesselName = vesselName;
        }
        public override string ToString() => _vesselName;
    }
    public class FlightOrVoyageNumber
    {
        private readonly string _number;
        public FlightOrVoyageNumber(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                throw new ArgumentNullException(nameof(number));
            }
            if (number.Length < 2 || number.Length > 10)
            {
                throw new ArgumentException(nameof(number));
            }
            _number = number;
        }
        public override string ToString() => _number;
    }
    public class ReferenceIdentification
    {
        private readonly string _referenceIdentification;
        public ReferenceIdentification(string referenceIdentification)
        {
            if (referenceIdentification == null)
            {
                throw new ArgumentNullException(nameof(referenceIdentification));
            }
            if (referenceIdentification.Length > 30)
            {
                throw new ArgumentException(nameof(referenceIdentification));
            }
            _referenceIdentification = referenceIdentification;
        }
        public override string ToString() => _referenceIdentification;
    }
    public class ReferenceIdentificationQualifier
    {
        private readonly string _referenceIdentificationQualifier;
        public ReferenceIdentificationQualifier(string referenceIdentificationQualifier)
        {
            if (referenceIdentificationQualifier == null)
            {
                throw new ArgumentNullException(nameof(referenceIdentificationQualifier));
            }
            if (referenceIdentificationQualifier != "3Z"
                && referenceIdentificationQualifier != "CN"
                && referenceIdentificationQualifier != "4C")
            {
                throw new ArgumentException(nameof(referenceIdentificationQualifier));
            }
            if (referenceIdentificationQualifier.Length != 2)
            {
                throw new ArgumentException(nameof(referenceIdentificationQualifier));
            }
            _referenceIdentificationQualifier = referenceIdentificationQualifier;
        }
        public override string ToString() => _referenceIdentificationQualifier;
    }
    public class OrganizationName
    {
        private readonly string _organizationName;
        public OrganizationName(string organizationName)
        {
            if (organizationName == null)
            {
                throw new ArgumentNullException(nameof(organizationName));
            }
            if (organizationName.Length > 60)
            {
                throw new ArgumentException(nameof(organizationName));
            }
            _organizationName = organizationName;
        }
        public override string ToString() => _organizationName;
    }
    public class CityName
    {
        private readonly string _cityName;
        public CityName(string cityName)
        {
            if (cityName == null)
            {
                throw new ArgumentNullException(nameof(cityName));
            }
            if (cityName.Length > 30 || cityName.Length < 2)
            {
                throw new ArgumentException(nameof(cityName));
            }
            _cityName = cityName;
        }
        public override string ToString() => _cityName;
    }
    public class StateOrProvince
    {
        private readonly string _stateOrProvince;
        public StateOrProvince(string stateOrProvince)
        {
            if (stateOrProvince == null)
            {
                throw new ArgumentNullException(nameof(stateOrProvince));
            }
            if (stateOrProvince.Length != 2)
            {
                throw new ArgumentException(nameof(stateOrProvince));
            }
            _stateOrProvince = stateOrProvince;
        }
        public override string ToString() => _stateOrProvince;
    }
    public class PostalCode
    {
        private readonly string _postalCode;
        public PostalCode(string postalCode)
        {
            if (postalCode == null)
            {
                throw new ArgumentNullException(nameof(postalCode));
            }
            if (postalCode.Length < 3 || postalCode.Length > 15)
            {
                throw new ArgumentException(nameof(postalCode));
            }
            _postalCode = postalCode;
        }
        public override string ToString() => _postalCode;
    }
    public class CountryCode
    {
        private readonly string _countryCode;
        public CountryCode(string countryCode)
        {
            if (countryCode == null)
            {
                throw new ArgumentNullException(nameof(countryCode));
            }
            if (countryCode.Length < 2 || countryCode.Length > 3)
            {
                throw new ArgumentException(nameof(countryCode));
            }
            _countryCode = countryCode;
        }
        public override string ToString() => _countryCode;
    }
    public class LadingDescription
    {
        private readonly string _ladingDescription;
        public LadingDescription(string ladingDescription)
        {
            if (ladingDescription == null)
            {
                throw new ArgumentNullException(nameof(ladingDescription));
            }
            if (ladingDescription.Length > 50)
            {
                throw new ArgumentException(nameof(ladingDescription));
            }
            _ladingDescription = ladingDescription;
        }
        public override string ToString() => _ladingDescription;
    }
    public class AddressInformation
    {
        private readonly string _addressInformation;
        public AddressInformation(string addressInformation)
        {
            if (addressInformation == null)
            {
                throw new ArgumentNullException(nameof(addressInformation));
            }
            if (addressInformation.Length > 35)
            {
                throw new ArgumentException(nameof(addressInformation));
            }
            _addressInformation = addressInformation;
        }
        public override string ToString() => _addressInformation;
    }
    public class SpecialChargeAllowanceCode
    {
        private readonly string _specialChargeAllowanceCode;
        public SpecialChargeAllowanceCode(string specialChargeAllowanceCode)
        {
            if (specialChargeAllowanceCode == null)
            {
                throw new ArgumentNullException(nameof(specialChargeAllowanceCode));
            }
            if (specialChargeAllowanceCode != "DSC"
                && specialChargeAllowanceCode != "405"
                && specialChargeAllowanceCode != "400")
            {
                throw new ArgumentException(nameof(specialChargeAllowanceCode));
            }
            _specialChargeAllowanceCode = specialChargeAllowanceCode;
        }
        public override string ToString() => _specialChargeAllowanceCode;

    }
    public class IdentificationCode
    {
        private readonly string _identificationCode;
        public IdentificationCode(string identificationCode)
        {
            if (identificationCode == null)
            {
                throw new ArgumentNullException(nameof(identificationCode));
            }
            if (identificationCode.Length < 2 || identificationCode.Length > 80)
            {
                throw new ArgumentException(nameof(identificationCode));
            }
            _identificationCode = identificationCode;
        }
        public override string ToString() => _identificationCode;
    }
    public class InvoiceNumber
    {
        private readonly string _invoiceNumber;
        public InvoiceNumber(string invoiceNumber)
        {
            if (invoiceNumber == null)
            {
                throw new ArgumentNullException(nameof(invoiceNumber));
            }
            if (invoiceNumber.Length > 22)
            {
                throw new ArgumentException("Length cannot exceed 22 digits", nameof(invoiceNumber));
            }
            _invoiceNumber = invoiceNumber;
        }
        public override string ToString() => _invoiceNumber;
    }
    public class BillOfLadingNumber
    {
        private readonly string _billOfLadingNumber;
        public BillOfLadingNumber(string billOfLadingNumber)
        {
            if (billOfLadingNumber == null)
            {
                throw new ArgumentNullException(nameof(billOfLadingNumber));
            }
            if (billOfLadingNumber.Length > 30)
            {
                throw new ArgumentException("Length cannot exceed 30 digits", nameof(billOfLadingNumber));
            }
            _billOfLadingNumber = billOfLadingNumber;
        }
        public override string ToString() => _billOfLadingNumber;
    }
    public class StandardCarrierCode
    {
        private readonly string _standardCarrierCode;
        public StandardCarrierCode(string standardCarrierCode)
        {
            if (standardCarrierCode == null)
            {
                throw new ArgumentNullException(nameof(standardCarrierCode));
            }
            if (standardCarrierCode.Length < 2 || standardCarrierCode.Length > 4)
            {
                throw new ArgumentException(nameof(standardCarrierCode));
            }
            _standardCarrierCode = standardCarrierCode;
        }
        public override string ToString() => _standardCarrierCode;
    }


}
