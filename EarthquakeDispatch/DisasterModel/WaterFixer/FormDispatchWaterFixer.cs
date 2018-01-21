using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DisasterModel;
using DisasterModel.Forms;

namespace DisasterModel
{
    public class FormDispatchWaterFixer : FormDispatch
    {
      
        public FormDispatchWaterFixer()
        {
            this.Text = "供水设备抢修组配送方案";
            ParaPanelVibility(false);
            SetRepoLablel("抢修组");
            SetSiteLabel("供水设备破坏点");
        }

        protected override UCParas GetUC()
        {
            return new UCParas();
        }

        protected override RepositoryCol GetRepoCol()
        {
            RepositoryCol repoCol = new RepositoryCol();
            repoCol.Setup(_dispatcher.RepoFeatureClass, RepositoryCol.WaterFixerField);
            return repoCol;
        }

        protected override RefugeeSiteCol GetSiteCol()
        {
            RefugeeSiteWaterFixerCol siteCol = new RefugeeSiteWaterFixerCol();
           
            siteCol.Setup(_dispatcher);
            return siteCol;
        }
    }
}
