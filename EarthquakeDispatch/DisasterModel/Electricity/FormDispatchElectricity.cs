using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DisasterModel.SitesCol;
using DisasterModel.Forms;


namespace DisasterModel
{
    public class FormDispatchElectricity : FormDispatch
    {
        public FormDispatchElectricity()
        {
            this.Text = "电力抢修人员配送方案";
            ParaPanelVibility(false);
            SetRepoLablel("抢修人员");
            SetSiteLabel("损毁点");
        }

        protected override UCParas GetUC()
        {
            return new UCParas();
        }
        protected override RepositoryCol GetRepoCol()
        {
            RepositoryCol repoCol = new RepositoryCol();
            repoCol.Setup(_dispatcher.RepoFeatureClass, RepositoryCol.ElectricityRepairerField);
            return repoCol;
        }

        protected override RefugeeSiteCol GetSiteCol()
        {
            RSElectricityCol siteCol = new RSElectricityCol();

            siteCol.Setup(_dispatcher);
            return siteCol;
        }
    }
}
