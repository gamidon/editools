using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdiTools.Tests
{
    [TestClass]
    public class ReadAndWriteEdiX12
    {
        //private const string path = @"C:\Users\Greg\Source\Repos\editools\EdiTools.Tests\bin\Debug\";
        private const string path = @"C:\Users\gamid\Source\Repos\editools\EdiTools.Tests\bin\Debug\";
        [TestMethod]
        public void ReadFile()
        {
            EdiDocument ediDocument = EdiDocument.Load($"{path}GIIT_DEXTER AXLE.txt");
            EdiTransactionSet transactionSet = ediDocument.TransactionSets[0];
            EdiMapping ediMapping = EdiMapping.Load($"{path}GoodShipEdi.xml");
            XDocument xml = ediMapping.Map(transactionSet.Segments);
            Console.WriteLine("Done");
        }
        [TestMethod]
        public void WriteFile()
        {
            var g = GoodShipDexterAxelEdi.EdiStart(new DateTime(2018, 05, 01, 18, 28, 52), controlNumber: 1, groupControlNumber: 1, requestAck: false);
            var transactionSet = TransactionSet.Start(g.GetNextTransactionSetControlNumber);
            transactionSet.Add(new EdiSegmentB3(new InvoiceNumber("3335371"), new BillOfLadingNumber("52787819"), PaymentMethod.PP, WeightUnit.L, new DateTime(2018, 04, 23), 1258.07m, new DateTime(2018, 04, 26)));
            transactionSet.Add(new EdiSegmentC3(CurrencyCode.USD));
            transactionSet.Add(new EdiSegmentN9_4C(ReferenceIndicationInboundOutbound.O));
            transactionSet.Add(new EdiSegmentG62(DateQualifier.PickupDate, new DateTime(2018, 04, 23)));
            transactionSet.Add(new EdiSegmentG62(DateQualifier.DeliveredDate, new DateTime(2018, 04, 26)));
            transactionSet.Add(new EdiSegmentR3(TransportationMode.M));
            var orgGS = new Organization(new OrganizationName("GOODSHIP"), new AddressInformation("699 Lively Blvd"), new AddressInformation(""), new CityName("Elk Grove Village"), new StateOrProvince("IL"), new PostalCode("60007"), new CountryCode("USA"), IdentificationCodeQualifier.GoodShip, new IdentificationCode(Info.GoodShipId));
            var orgDE = new Organization(new OrganizationName("DEXTER AXLE"), new AddressInformation("902 S. DIVISION ST"), new AddressInformation(""), new CityName("BRISTOL"), new StateOrProvince("IN"), new PostalCode("46507"), new CountryCode("USA"), IdentificationCodeQualifier.DexterAxel, new IdentificationCode(Info.DexterAxelId));
            var orgABC = new Organization(new OrganizationName("ABC COMPANY"), new AddressInformation("2686 COMMERCE ROAD"), new AddressInformation(""), new CityName("MacClenny"), new StateOrProvince("FL"), new PostalCode("32063"), new CountryCode("USA"), IdentificationCodeQualifier.UnKnown, new IdentificationCode("   "));
            transactionSet.Add(new EdiSegmentN1(EntityIdentifierCode.SE, orgGS));
            transactionSet.Add(new EdiSegmentN1(EntityIdentifierCode.BY, orgDE));
            transactionSet.Add(new EdiSegmentN1(EntityIdentifierCode.SF, orgDE));
            transactionSet.Add(new EdiSegmentN3(orgDE));
            transactionSet.Add(new EdiSegmentN4(orgDE));
            transactionSet.Add(new EdiSegmentN1(EntityIdentifierCode.ST, orgABC));
            transactionSet.Add(new EdiSegmentN3(orgABC));
            transactionSet.Add(new EdiSegmentN4(orgABC));
            var lxSection = new LxSection(1,
                                            new EdiSegmentL5(1, new LadingDescription("DOORS")),
                                            new EdiSegmentL0(1, 536m, WeighQualifier.N, 1, PackagingForm.SKD),
                                            new EdiSegmentL1(1, 967.67m, FreightRateQualifier.PH, 5186.71m));
            transactionSet.AddRange(lxSection.GetSegments);
            lxSection = new LxSection(2,
                                            new EdiSegmentL5(2, new LadingDescription("PERCENT DISCOUNT")),
                                            new EdiSegmentL0(2, 1, BilledRateQualifier.EA),
                                            new EdiSegmentL1(2, 93m, FreightRateQualifier.AV, -4823.64m, new SpecialChargeAllowanceCode("DSC")));
            transactionSet.AddRange(lxSection.GetSegments);
            lxSection = new LxSection(3,
                                            new EdiSegmentL5(3, new LadingDescription("FUEL SURCHARGE")),
                                            new EdiSegmentL0(3, 895m, BilledRateQualifier.DM),
                                            new EdiSegmentL1(3, 1.00m, FreightRateQualifier.PM, 890.00m, new SpecialChargeAllowanceCode("405")));
            transactionSet.AddRange(lxSection.GetSegments);
            transactionSet.Add(new EdiSegmentL3(536m, WeighQualifier.G, 1258.07m, WeightUnit.L));
            transactionSet.End();
            g.TransactionSets.Add(transactionSet);
            g.EdiEnd();
            g.Save($"{path}save.txt");
            var output = g.ToEdi();
        }
        [TestMethod]
        public void CreateFromInvoice()
        {
            var g = GoodShipDexterAxelEdi.EdiStart(new DateTime(2018, 05, 01, 18, 28, 52), ControlNumbers.GetControlNumber(), ControlNumbers.GetGroupControlNumber(), requestAck: false);
            var invoiceData = GetFirstExample();
            var transactionSet = TransactionSet.Start(g.GetNextTransactionSetControlNumber);
            transactionSet.Add(new EdiSegmentB3(invoiceData));
            transactionSet.Add(new EdiSegmentC3(invoiceData));
            transactionSet.Add(new EdiSegmentN9_4C(invoiceData));
            transactionSet.Add(new EdiSegmentG62(DateQualifier.PickupDate, invoiceData.PickupDate));
            transactionSet.Add(new EdiSegmentG62(DateQualifier.DeliveredDate, invoiceData.DeliveryDate));
            transactionSet.Add(new EdiSegmentR3(invoiceData));
            transactionSet.AddRange(new N1Section(invoiceData).GetSegments);
            var lxNumber = 1;
            foreach (var invoiceItem in invoiceData.InvoiceItems)
            {
                EdiSegmentL1 l1Segment = new EdiSegmentL1(lxNumber, invoiceItem.FreightRate, invoiceItem.FreightRateQualifier, invoiceItem.Charge);
                if (invoiceItem.FreightRateQualifier == FreightRateQualifier.AV)
                {
                    l1Segment.AddSpecialChargeAllowanceCode(new SpecialChargeAllowanceCode("DSC"));
                }
                if (invoiceItem.FreightRateQualifier == FreightRateQualifier.PM)
                {
                    l1Segment.AddSpecialChargeAllowanceCode(new SpecialChargeAllowanceCode("405"));
                }
                var lxSection = new LxSection(lxNumber,
                                                new EdiSegmentL5(lxNumber, invoiceItem.LadingDescription),
                                                new EdiSegmentL0(lxNumber, invoiceItem.Weight, invoiceItem.WeighQualifier, invoiceItem.Quantity, invoiceItem.PackagingForm),
                                                l1Segment);
                transactionSet.AddRange(lxSection.GetSegments);
                lxNumber++;
            }
            transactionSet.Add(new EdiSegmentL3(536m, WeighQualifier.G, 1258.07m, WeightUnit.L));
            transactionSet.End();
            g.TransactionSets.Add(transactionSet);
            g.EdiEnd();
            g.Save($"{path}saveFromInvoiceData.txt");
            var output = g.ToEdi();
        }
        private GoodShipInvoiceData GetFirstExample()
        => new GoodShipInvoiceData
        {
            InvoiceNumber = new InvoiceNumber("3335371"),
            ShipmentNumber = new BillOfLadingNumber("52787819"),
            PaymentMethod = PaymentMethod.PP,
            WeightUnit = WeightUnit.L,
            NetAmountDate = new DateTime(2018, 04, 23),
            NetAmountDue = 1258.07m,
            DeliveryDate = new DateTime(2018, 04, 26),
            PickupDate = new DateTime(2018, 04, 23),
            CurrencyCode = CurrencyCode.USD,
            ReferenceIdentificationQualifier = new ReferenceIdentificationQualifier("4C"),  //??? Fixed?
            ReferenceIdentification = new ReferenceIdentification("O"),  //??? Fixed?
            TransportationMode = TransportationMode.M,
            OrgSE = new Organization(new OrganizationName("GOODSHIP"), new AddressInformation("699 Lively Blvd"), new AddressInformation(""), new CityName("Elk Grove Village"), new StateOrProvince("IL"), new PostalCode("60007"), new CountryCode("USA"), IdentificationCodeQualifier.GoodShip, new IdentificationCode(Info.GoodShipId)),
            OrgBY = new Organization(new OrganizationName("DEXTER AXLE"), new AddressInformation("902 S. DIVISION ST"), new AddressInformation(""), new CityName("BRISTOL"), new StateOrProvince("IN"), new PostalCode("46507"), new CountryCode("USA"), IdentificationCodeQualifier.DexterAxel, new IdentificationCode(Info.DexterAxelId)),
            OrgSF = new Organization(new OrganizationName("DEXTER AXLE"), new AddressInformation("902 S. DIVISION ST"), new AddressInformation(""), new CityName("BRISTOL"), new StateOrProvince("IN"), new PostalCode("46507"), new CountryCode("USA"), IdentificationCodeQualifier.DexterAxel, new IdentificationCode(Info.DexterAxelId)),
            OrgST = new Organization(new OrganizationName("ABC COMPANY"), new AddressInformation("2686 COMMERCE ROAD"), new AddressInformation(""), new CityName("MacClenny"), new StateOrProvince("FL"), new PostalCode("32063"), new CountryCode("USA"), IdentificationCodeQualifier.UnKnown, new IdentificationCode("   ")),
            InvoiceItems = new List<GoodShipInvoiceData.InvoiceElements>()
                                        {
                                            new GoodShipInvoiceData.InvoiceElements()
                                            {
                                                LadingDescription = new LadingDescription("DOORS"),
                                                Weight = 536m,
                                                WeighQualifier = WeighQualifier.N,
                                                Quantity =1,
                                                PackagingForm=  PackagingForm.SKD,
                                                FreightRate =  967.67m,
                                                FreightRateQualifier = FreightRateQualifier.PH,
                                                Charge=  5186.71m
                                            },
                                            new GoodShipInvoiceData.InvoiceElements()
                                            {
                                                LadingDescription = new LadingDescription("PERCENT DISCOUNT"),
                                                BilledRatedAsQuantity = 1,
                                                BilledRateQualifier =  BilledRateQualifier.EA,
                                                FreightRate =  93m,
                                                FreightRateQualifier = FreightRateQualifier.AV,
                                                Charge=  -4823.64m,
                                                SpecialChargeAllowanceCode = new SpecialChargeAllowanceCode("DSC")
                                            },
                                            new GoodShipInvoiceData.InvoiceElements()
                                            {
                                                LadingDescription = new LadingDescription("FUEL SURCHARGE"),
                                                BilledRatedAsQuantity = 895m,
                                                BilledRateQualifier =  BilledRateQualifier.DM,
                                                FreightRate = 1.00m,
                                                FreightRateQualifier = FreightRateQualifier.PM,
                                                Charge=  890.00m,
                                                SpecialChargeAllowanceCode = new SpecialChargeAllowanceCode("405")
                                            }
                                        },

            WeightQualifier = WeighQualifier.G,
            TotalWeight = 536m,
            TotalCharge = 1258.07m
        };
    }
}