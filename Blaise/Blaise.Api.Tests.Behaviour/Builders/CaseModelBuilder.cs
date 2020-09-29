using System.Collections.Generic;
using Blaise.Api.Tests.Behaviour.Models;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;

namespace Blaise.Api.Tests.Behaviour.Builders
{
    public class CaseModelBuilder
    {
        private readonly string _defaultPrimaryKey;
        
        private readonly Dictionary<string, string> _defaultFieldData;

        
        public CaseModelBuilder()
        {
            _defaultPrimaryKey = "900000";

            _defaultFieldData = BuildFieldData(FieldNameType.CaseId, "1");
        }

        public CaseModel BuildBasicCase()
        {
            return new CaseModel
            {
                PrimaryKey = _defaultPrimaryKey,
                FieldData = _defaultFieldData
            };
        }

        public CaseModel BuildCaseWithPrimaryKey(string primaryKey)
        {
            return new CaseModel
            {
                PrimaryKey = primaryKey,
                FieldData = _defaultFieldData
            };
        }
        public CaseModel BuildCaseWithData(Dictionary<string, string> fieldData)
        {
            return new CaseModel
            {
                PrimaryKey = _defaultPrimaryKey,
                FieldData = fieldData
            };
        }

        public CaseModel BuildCaseWithField(FieldNameType fieldName, string value)
        {
            return new CaseModel
            {
                PrimaryKey = _defaultPrimaryKey,
                FieldData = BuildFieldData(fieldName, value)
            };
        }

        private static Dictionary<string, string> BuildFieldData(FieldNameType fieldName, string value)
        {
            return new Dictionary<string, string>
            {
                {fieldName.FullName(), $"{value}"}
            };
        }
    }
}
