using System;
using System.Collections.Generic;
using BraspagApiDotNetSdk.Contracts.Enum;

namespace BraspagApiDotNetSdk.Contracts.Antifraud
{
    public class FraudAnalysis
    {
        public AntifraudSequenceEnum Sequence { get; set; }

        public AntifraudSequenceCriteriaEnum SequenceCriteria { get; set; }

        public string FingerPrintId { get; set; }

        public List<MerchantDefinedField> MerchantDefinedFields { get; set; }

        public CartData Cart { get; set; }

        public TravelData Travel { get; set; }

        public BrowserData Browser { get; set; }

        public ShippingData Shipping { get; set; }

        public Guid? Id { get; set; }

        public byte Status { get; set; }

        public int? FraudAnalysisReasonCode { get; set; }

        public ReplyData ReplyData { get; set; }

        public List<string> InvalidFields { get; set; }

        public bool? CaptureOnLowRisk { get; set; }

        public bool? VoidOnHighRisk { get; set; }
    }
}
