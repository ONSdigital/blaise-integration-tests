using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;

namespace Blaise.Tests.Models.Case
{
    public class CaseModel : IEquatable<CaseModel>
    {
        public string PrimaryKey { get; }
        public string OutcomeCode { get; }
        public string TelephoneNo { get; }

        public CaseModel(string primaryKey, string outcomeCode, string telephoneNo)
        {
            PrimaryKey = primaryKey;
            OutcomeCode = outcomeCode;
            TelephoneNo = telephoneNo;
        }
        public bool Equals(CaseModel other)
        {
            if (other is null)
                return false;

            return PrimaryKey == other.PrimaryKey && OutcomeCode == other.OutcomeCode; //&& TelephoneNo == other.TelephoneNo;
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
        public override int GetHashCode() => (PrimaryKey, OutcomeCode, TelephoneNo).GetHashCode();

    }
}
