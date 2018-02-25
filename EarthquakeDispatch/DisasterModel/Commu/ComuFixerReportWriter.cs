using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisasterModel
{
    class ComuFixerReportWriter : ReportWriter
    {
        internal override string GetTemplateDocName()
        {
            return "通讯基站抢修人员配送方案模板.docx"; 
        }

        protected override void WriteTheText(Dispatcher dispatcher, ExportToWord exportToWord)
        {
            WriteSummary(dispatcher, exportToWord);
        }

        protected override string GetSiteType()
        {
            return "受损基站";
        }

        protected override string GetDispatchFormat()
        {
            return "    {0}调配基站抢修人员点({1})的抢修人员{2}名至受损基站{3}\r\n";
        }

        protected override string ConclusionType(int p)
        {
            switch (p)
            {
                case 0:
                    return "经过调配，抢修人员正好满足救灾需求，因此不再需要调配基站抢修人员。";
                case 1:
                    return "经过调配，目前所有的队伍已经到达受损基站，还剩余{0}支队伍{1}人待命";
                case -1:
                    return "经过调配，目前所有的队伍已经到达受损基站，由于灾情严重，灾区还需要{0}名基站抢修人员参与抢修工作。";
            }
            return "";
        }
    }
}
