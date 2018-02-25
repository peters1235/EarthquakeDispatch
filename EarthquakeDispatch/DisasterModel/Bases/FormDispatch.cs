using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DisasterModel.SitesCol;

namespace DisasterModel
{
    public abstract partial class FormDispatch : Form
    {
        public FormDispatch()
        {
            InitializeComponent();
#if DEBUG
             ForTest();
#endif
            _ucParas = GetUC();
            _ucParas.Dock = DockStyle.Fill;
            paraPanel.Controls.Add(_ucParas);
        }

        protected virtual void ForTest()
        {
            txtEarthquakeName.Text = "he";
            txtFacilityLoc.Text = @"E:\17\private\Disaster\Data\added\燃气维修人员.shp";
            txtIncidentLoc.Text = @"E:\17\private\Disaster\Data\added\燃气破坏点.shp";
            txtOutputFolder.Text = GetNonExist();
        }

        protected abstract UCParas GetUC();

        protected void ParaPanelVibility(bool visible)
        {
            this.paraPanel.Visible = visible;
        }

        protected void SetSiteLabel(string label)
        {
            lblSite.Text = label;
        }

        protected void SetRepoLablel(string label)
        {
            lblRepo.Text = label;
        }

        protected UCParas _ucParas = null;
        protected Dispatcher _dispatcher = null;
        public Dispatcher Dispatcher { get { return _dispatcher; } }

        protected string GetFile(OpenFileDialog openFileDialog1)
        {
            openFileDialog1.Filter = "Shp文件|*.shp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                return path;
            }
            return null;
        }

        protected string GetFolder(FolderBrowserDialog folderBrowserDialog1)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return folderBrowserDialog1.SelectedPath;
            }
            return "";
        }

        private string GetNonExist()
        {
            string dir = @"E:\17\private\Disaster\Data\output\o";
            while (System.IO.Directory.Exists(dir))
            {
                dir += "1";
            }
            return dir;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            Dispatch();
        }

        private void Dispatch()
        {
            try
            {
                Earthquake quake = new Earthquake()
                {
                    DateTime = dtEarthquakeDate.Value,
                    Name = txtEarthquakeName.Text
                };

                string incidentData = txtIncidentLoc.Text;
                string facilityData = txtFacilityLoc.Text;
                string outputFolder = txtOutputFolder.Text;

                DispatchResource(quake, incidentData, facilityData, outputFolder);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        protected virtual void DispatchResource(Earthquake quake, string incidentData, string facilityData, string outputFolder)
        {
            _dispatcher = new Dispatcher();
            _dispatcher.OutputFolder = outputFolder;
            if (_dispatcher.Setup(quake, facilityData, incidentData))
            {
                RefugeeSiteCol siteCol = GetSiteCol();
                _dispatcher.SetReportName(siteCol);
                RepositoryCol repoCol = GetRepoCol();

                SupplyNetwork supplyNetwork = new SupplyNetwork();
                supplyNetwork.SetLocations(siteCol, repoCol, _dispatcher.RoadNetwork);
                supplyNetwork.Init(_dispatcher.GetRoutesClass());

                foreach (var site in siteCol.Sites)
                {
                    supplyNetwork.SupplyResource(site);
                }
                _dispatcher.StoreResult(supplyNetwork.Routes, repoCol, siteCol);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

      
        protected abstract RepositoryCol GetRepoCol();

        protected abstract RefugeeSiteCol GetSiteCol();

        private void txtFacilityLoc_Click(object sender, EventArgs e)
        {
            string file = GetFile(openFileDialog1);
            if (file != null)
            {
                txtFacilityLoc.Text = file;
            }
        }

        private void txtIncidentLoc_Click(object sender, EventArgs e)
        {
            string file = GetFile(openFileDialog1);
            if (file != null)
            {
                txtIncidentLoc.Text = file; 
            }
        }

        private void txtOutputFolder_Click(object sender, EventArgs e)
        {
            txtOutputFolder.Text = GetFolder(folderBrowserDialog1);
        }           
    }
}
