using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisasterModel
{
    class RescuerReportWriter:ReportWriter
    {
        internal override string GetTemplateDocName()
        {
            return "救援队伍配送方案模板.docx";
        }

        protected override void WriteTheText(Dispatcher dispatcher, ExportToWord exportToWord)
        {
            WriteSummary(dispatcher, exportToWord);
        }

        protected override string GetSiteType()
        {
          return "埋压点";
        }

        protected override string GetDispatchFormat()
        {
            return "    {0}调配救援队伍({1})的救援人员{2}名至埋压点{3}\r\n";
        }

        protected override string ConclusionType(int p)
        {
            switch (p)
            {
                case 0:
                    return "经过救援队伍调配，救援人员正好满足救灾需求，因此不再需要调配救援队伍。";
                case 1:
                    return "经过救援队伍调配，目前所有的队伍已经到达埋压点，还剩余{0}支队伍{1}人待命";
                case -1:
                    return "经过救援队伍调配，目前所有的队伍已经到达埋压点，由于灾情严重，埋压人员较多，灾区还需要{0}名救援人员参与救援。";
            }
            return "";
        }
    }
}
