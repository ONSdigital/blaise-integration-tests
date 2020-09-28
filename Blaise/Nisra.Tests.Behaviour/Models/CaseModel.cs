﻿using BlaiseNisraCaseProcessor.Tests.Behaviour.Enums;

namespace BlaiseNisraCaseProcessor.Tests.Behaviour.Models
{
    public class CaseModel
    {
        public CaseModel(string primaryKey, string outcome, ModeType mode, string caseId = null)
        {
            PrimaryKey = primaryKey;
            Outcome = outcome;
            Mode = mode;
            CaseId = caseId;
        }

        public string PrimaryKey { get; set; }

        public string Outcome { get; set; }

        public ModeType Mode { get; set; }

        public string CaseId { get; set; }
    }
}
