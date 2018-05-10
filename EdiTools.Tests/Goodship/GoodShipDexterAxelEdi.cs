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
    public static class ControlNumbers
    {
        public static int GetControlNumber() => 1;
        public static int GetGroupControlNumber() => 1;
    }
}