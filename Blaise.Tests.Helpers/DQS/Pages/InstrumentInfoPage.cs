using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class InstrumentInfoPage : BasePage
    {
        public string ToStartDatePath = "//*[@id=\"main-content\"]/div[1]/div/table/tbody/tr/td[2]";
        public string AddToStartDatePath = "//*[@id=\"main-content\"]/div[1]/div/table/tbody/tr/td[3]/a";
        

        public InstrumentInfoPage() : base(DqsConfigurationHelper.DqsUrl)
        {
        }

        public string GetToStartDate()
        {
            return GetElementTextByPath(ToStartDatePath);
        }

        public void AddToStartDate()
        {
            ClickButtonByXPath(AddToStartDatePath);
        }
    }
}
