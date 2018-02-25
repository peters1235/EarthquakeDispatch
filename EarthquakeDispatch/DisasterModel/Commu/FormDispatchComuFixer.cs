using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DisasterModel;
using DisasterModel.Forms;

namespace DisasterModel
{
    public class FormDispatchComuFixer : FormDispatch
    {

        public FormDispatchComuFixer()
        {
            this.Text = "通讯基站抢修人员配送方案";
            ParaPanelVibility(false);
            SetRepoLablel("基站抢修人员");
            SetSiteLabel("基站受损点");
        }

        protected override UCParas GetUC()
        {
            return new UCParas();
        }

        protected override RepositoryCol GetRepoCol()
        {
            RepositoryCol repoCol = new RepositoryCol();
            repoCol.Setup(_dispatcher.RepoFeatureClass, RepositoryCol.CommuFixerField);
            return repoCol;
        }

        protected override RefugeeSiteCol GetSiteCol()
        {
            RefugeeSiteComuFixerCol siteCol = new RefugeeSiteComuFixerCol();
           
            siteCol.Setup(_dispatcher);
            return siteCol;
        }
    }
}
