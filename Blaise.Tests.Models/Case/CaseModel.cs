using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blaise.Tests.Models.Case
{
    public class CaseModel : IEquatable<CaseModel>
    {
        public Dictionary<string, string> PrimaryKeyValues { get; }
        
        public string PrimaryKey { get; }
        public string OutcomeCode { get; }
        public string TelephoneNo { get; }

        public CaseModel(string primaryKey, string outcomeCode, string telephoneNo)
        {
            PrimaryKey = primaryKey;

            PrimaryKeyValues = new Dictionary<string, string>
            {
                ["QID.Serial_Number"] = primaryKey
            };

            OutcomeCode = outcomeCode;
            TelephoneNo = telephoneNo;
        }

        public CaseModel(Dictionary<string, string> primaryKeyValues, string outcomeCode, string telephoneNo)
        {
            PrimaryKeyValues = primaryKeyValues;

            PrimaryKey = PrimaryKeyValues.FirstOrDefault().Value;

            OutcomeCode = outcomeCode;
            TelephoneNo = telephoneNo;
        }

        public bool Equals(CaseModel other)
        {
            if (other is null)
                return false;

            return PrimaryKey == other.PrimaryKey && OutcomeCode == other.OutcomeCode; 
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
