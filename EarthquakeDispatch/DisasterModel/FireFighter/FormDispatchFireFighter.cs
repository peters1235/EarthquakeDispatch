using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DisasterModel;
using DisasterModel.Forms;

namespace DisasterModel
{
    public class FormDispatchFireFighter : FormDispatch
    {
      
        public FormDispatchFireFighter()
        {
            this.Text = "消防员配送方案";
            ParaPanelVibility(false);
            SetRepoLablel("消防站");
            SetSiteLabel("着火点");
        }

        protected override UCParas GetUC()
        {
            return new UCParas();
        }

        protected override RepositoryCol GetRepoCol()
        {
            RepositoryCol repoCol = new RepositoryCol();
            repoCol.Setup(_dispatcher.RepoFeatureClass, RepositoryCol.FireFighterField);
            return repoCol;
        }

        protected override RefugeeSiteCol GetSiteCol()
        {
            RefugeeSiteFireFighterCol siteCol = new RefugeeSiteFireFighterCol();
           
            siteCol.Setup(_dispatcher);
            return siteCol;
        }
    }
}
