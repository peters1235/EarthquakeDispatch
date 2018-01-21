using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisasterModel
{
    class FireFighterReportWriter:ReportWriter
    {
        internal override string GetTemplateDocName()
        {
            return "消防员配送方案模板.docx";
        }

        protected override void WriteTheText(Dispatcher dispatcher, ExportToWord exportToWord)
        {
            WriteSummary(dispatcher, exportToWord);
        }

        protected override string GetSiteType()
        {
            return "着火点";
        }

        protected override string GetDispatchFormat()
        {
            return "    {0}调配消防队伍({1})的消防人员{2}名至着火点{3}\r\n";
        }

        protected override string ConclusionType(int p)
        {
            switch (p)
            {
                case 0:
                    return "经过消防队伍调配，消防人员正好满足救灾需求，因此不再需要调配消防队伍。";
                case 1:
                    return "经过消防队伍调配，目前所有的队伍已经到达着火点，还剩余{0}支队伍{1}人待命";
                case -1:
                    return "经过消防队伍调配，目前所有的队伍已经到达着火点，由于灾情严重，灾区还需要{0}名消防人员参与灭火工作。";
            }
            return "";
        }
    }
}
