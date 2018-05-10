using System;
using System.Collections.Generic;

namespace EdiTools.Tests
{
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
        public void AddRange(IEnumerable<EdiSegment> segments)
        {
            foreach (var segment in segments)
            {
                Add(segment);
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
}