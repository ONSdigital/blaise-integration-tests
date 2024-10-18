using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using System;
using System.Collections.Generic;

namespace Blaise.Tests.Models.Case
{
    public class CaseModel : IEquatable<CaseModel>
    {
        public Dictionary<string, string> PrimaryKeyValues { get; }
        public string OutcomeCode { get; }
        public string TelephoneNo { get; }

        public CaseModel(Dictionary<string, string> primaryKeyValues, string outcomeCode, string telephoneNo)
        {
            PrimaryKeyValues = primaryKeyValues;
            OutcomeCode = outcomeCode;
            TelephoneNo = telephoneNo;
        }
        public bool Equals(CaseModel other)
        {
            if (other is null)
                return false;

            return PrimaryKeyValues == other.PrimaryKeyValues && OutcomeCode == other.OutcomeCode; //&& TelephoneNo == other.TelephoneNo;
        }

        public Dictionary<string, string> FieldData()
        {
            return new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), OutcomeCode},
                {FieldNameType.TelNo.FullName(), TelephoneNo}
            };
        }

        public override bool Equals(object obj) => Equals(obj as CaseModel);
        public override int GetHashCode() => (PrimaryKeyValues, OutcomeCode, TelephoneNo).GetHashCode();

    }
}
