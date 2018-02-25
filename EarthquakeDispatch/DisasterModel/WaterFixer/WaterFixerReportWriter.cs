using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisasterModel
{
    class WaterFixerReportWriter: ReportWriter
    {
        internal override string GetTemplateDocName()
        {
            return "供水设备抢修组配送方案模板.docx"; 
        }

        protected override void WriteTheText(Dispatcher dispatcher, ExportToWord exportToWord)
        {
            WriteSummary(dispatcher, exportToWord);
        }

        protected override string GetSiteType()
        {
            return "供水设备破坏点";
        }

        protected override string GetDispatchFormat()
        {
            return "    {0}调配供水设备抢修组({1})的抢修人员{2}名至供水设备破坏点{3}\r\n";
        }

        protected override string ConclusionType(int p)
        {
            switch (p)
            {
                case 0:
                    return "经过供水设备抢修组调配，抢修人员正好满足救灾需求，因此不再需要调配电力抢修队伍。";
                case 1:
                    return "经过供水设备抢修组调配，目前所有的队伍已经到达供水设备破坏点，还剩余{0}支队伍{1}人待命";
                case -1:
                    return "经过供水设备抢修组调配，目前所有的队伍已经到达供水设备破坏点，由于灾情严重，灾区还需要{0}名供水设备抢修人员参与抢修工作。";
            }
            return "";
        }
    }
}
