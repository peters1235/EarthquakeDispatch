using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DisasterModel;
using DisasterModel.Forms;

namespace DisasterModel
{
    public class FormDispatchGasFixer : FormDispatch
    {
      
        public FormDispatchGasFixer()
        {
            this.Text = "燃气设施抢修组配送方案";
            ParaPanelVibility(false);
            SetRepoLablel("抢修组");
            SetSiteLabel("燃气设施危险点");
        }

        protected override UCParas GetUC()
        {
            return new UCParas();
        }

        protected override void DispatchResource(Earthquake quake, string incidentData, string facilityData, string outputFolder)
        {
            _dispatcher = new Dispatcher();
            _dispatcher.OutputFolder = outputFolder;
            if (_dispatcher.Setup(quake, facilityData, incidentData))
            {
                _dispatcher.SetReportName("燃气抢修组");

                //RefugeeSiteCol siteCol = GetSiteCol();
                //RepositoryCol repoCol = GetRepoCol();

                SupplyNetwork supplyNetwork = new SupplyNetwork();
                supplyNetwork.Init(_dispatcher.GetRoutesClass());

                //车
                RepositoryCol repoCol = new RepositoryCol();
                repoCol.Setup(_dispatcher.RepoFeatureClass, RepositoryCol.GasCarField);
                RefugeeSiteGasCarCol siteCol = new RefugeeSiteGasCarCol();
                siteCol.Setup(_dispatcher);

                supplyNetwork.SetLocations(siteCol, repoCol, _dispatcher.RoadNetwork);

                foreach (var site in siteCol.Sites)
                {
                    supplyNetwork.SupplyResource(site);
                }

                //人
                repoCol = new RepositoryCol();
                repoCol.Setup(_dispatcher.RepoFeatureClass, RepositoryCol.GasManField);
                RefugeeSiteGasManCol siteManCol = new RefugeeSiteGasManCol();
                siteManCol.Setup(_dispatcher);

                supplyNetwork.SetLocations(siteManCol, repoCol, _dispatcher.RoadNetwork);

                try
                {
                    foreach (var site in siteCol.Sites)
                    {
                        supplyNetwork.SupplyResource(site);
                    }
                }
                catch (Exception ex)
                {

                }
                _dispatcher.StoreResult(supplyNetwork.Routes, repoCol, siteCol);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
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
