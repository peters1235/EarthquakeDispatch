using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace DisasterModel
{
    class ReportWriter
    {
        internal virtual string GetTemplateDocName()
        {
            return DisasterModel.Properties.Resources.DocName;
        }

        internal void FillTheReport(Dispatcher dispatcher,IActiveView view, ExportToWord exportToWord)
        {
            exportToWord.WriteWord("EarthquakeName", dispatcher.Earthquake.Name, false);

            WriteTheText(dispatcher, exportToWord);

            ExportActiveView export = new ExportActiveView();
            string imgName = System.IO.Path.GetTempFileName();
            export.ExportActiveViewParameterized(view, 300, 1, "JPEG", imgName, false);
            exportToWord.WriteWord("DispatchRoutes", imgName, true);

            exportToWord.Finish();
        }

        protected void WriteSummary(Dispatcher dispatcher, ExportToWord exportToWord)
        {
            exportToWord.WriteWord("TeamCount", dispatcher.ReposRemain.RepoCount.ToString(), false);
            exportToWord.WriteWord("TeamMemberCount", dispatcher.ReposRemain.TotalResource.ToString(), false);

            exportToWord.WriteWord("RefugeSiteCount", dispatcher.SitesRemain.Sites.Count.ToString(), false);
            exportToWord.WriteWord("RescuerInNeed", dispatcher.SitesRemain.TotalResourceNeeds.ToString(), false);

            exportToWord.WriteWord("RescuePriority", dispatcher.SitesRemain.RescuePriority(GetSiteType()), false);

            exportToWord.WriteWord("RescueSchema", GetDispatchSchema(dispatcher), false);

            exportToWord.WriteWord("Conclusion", GetConclusion(dispatcher), false);
        }

        protected virtual string GetSiteType()
        {
            throw new NotImplementedException();
        }

        protected virtual void WriteTheText(Dispatcher dispatcher, ExportToWord exportToWord)
        {
            exportToWord.WriteWord("DispatchSchema", dispatcher.GetDispatchSchema(), false);
        }

        protected string GetDispatchSchema(Dispatcher dispatcher)
        {
            string formatString = GetDispatchFormat() ;
            StringBuilder sb = new StringBuilder();
            int i = 1;
            foreach (var route in dispatcher.ResultRoutes)
            {
                sb.Append(string.Format(formatString, i++, route.RepoID, route.Amount, route.IncidentID));
            }
            return sb.ToString();
        }

        protected virtual string GetDispatchFormat()
        {
            throw new NotImplementedException();
        }

        protected string GetConclusion(Dispatcher dispatcher)
        {
            int resourceInShort = dispatcher.SitesRemain.TotalResourceNeeds -
                dispatcher.ReposRemain.TotalResource;
            if (resourceInShort == 0)
            {
                return ConclusionType(0);
            }
            else if (resourceInShort > 0)
            {
                return string.Format(ConclusionType(-1), resourceInShort);
            }
            else
            {
                int remainRepos = dispatcher.ReposRemain.GetRemainRepoCount();
                int remainResource = -resourceInShort;
                return string.Format(ConclusionType(1),
                    remainRepos,
                    remainResource
                    );
            }
        }

        protected virtual string ConclusionType(int p)
        {
            throw new NotImplementedException();
        }

    }
}
