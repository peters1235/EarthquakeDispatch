using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DisasterModel.SitesCol;
using DisasterModel.Forms;


namespace DisasterModel
{
    public class FormDispatchRescue: FormDispatch
    {
        public FormDispatchRescue()
        {
            this.Text = "救援人员配送方案";
            ParaPanelVibility(false);
            SetRepoLablel("救援队伍");
            SetSiteLabel("埋压点");
        }

        protected override UCParas GetUC()
        {
            return new UCParas();
        }
        protected override RepositoryCol GetRepoCol()
        {
            RepositoryCol repoCol = new RepositoryCol();
            repoCol.Setup(_dispatcher.RepoFeatureClass, RepositoryCol.RescueField); ;
            return repoCol;
        }

        protected override RefugeeSiteCol GetSiteCol()
        {
            RefugeeSiteRescueCol siteCol = new RefugeeSiteRescueCol();
           
           siteCol.Setup(_dispatcher);
            return siteCol;
        }
    }
}
