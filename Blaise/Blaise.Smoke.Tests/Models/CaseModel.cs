using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaise.Smoke.Tests.Models
{
    public class CaseModel : IEquatable<CaseModel>
    {
        public string PrimaryKey { get; set; }
        public string Outcome { get; set; }
        public string TelNo { get; set; }

        public bool Equals(CaseModel other)
        {
            if (other is null)
                return false;

            return this.PrimaryKey == other.PrimaryKey && this.Outcome == other.Outcome && this.TelNo == other.TelNo;
        }

        public Dictionary<string, string> ConvertToDictionary()
        {
            var dic = new Dictionary<string, string>();
            dic.Add("serial_number", this.PrimaryKey);
            dic.Add("QHAdmin.HOut", this.Outcome);
            dic.Add("QDataBag.TelNo", this.TelNo);
            return dic;
        }

        public override bool Equals(object obj) => Equals(obj as CaseModel);
        public override int GetHashCode() => (PrimaryKey, Outcome, TelNo).GetHashCode();

    }
}
