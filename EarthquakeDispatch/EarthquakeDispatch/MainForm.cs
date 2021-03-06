using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.SystemUI;
using DisasterModel;
using System.Configuration;
using ESRI.ArcGIS.Geometry;

namespace EarthquakeDispatch
{
    public sealed partial class MainForm : Form
    {
        #region class private members
        private IMapControl3 m_mapControl = null;
        private IPageLayoutControl3 m_pageLayoutControl = null;

        private string m_mapDocumentName = string.Empty;

        Dispatcher _dispatcher = null;
        #endregion

        #region class constructor
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            //get the MapControl
            m_mapControl = (IMapControl3)axMapControl1.Object;
            m_pageLayoutControl = (IPageLayoutControl3)axPageLayoutControl1.Object;

            //disable the Save menu (since there is no document yet)
            menuSaveDoc.Enabled = false;

            Setup();
            axPageLayoutControl1.OnMouseMove +=new IPageLayoutControlEvents_Ax_OnMouseMoveEventHandler          (axPageLayoutControl1_OnMouseMove);
            axPageLayoutControl1.Visible = false;
        }

        private void  axPageLayoutControl1_OnMouseMove(object sender, IPageLayoutControlEvents_OnMouseMoveEvent e)
        {
            statusBarXY.Text = string.Format("{0}, {1}  {2}",e.pageX.ToString("#######.##"), e.pageY.ToString("#######.##"), axMapControl1.MapUnits.ToString().Substring(4)); 
        }

        private void Setup()
        {
            this.Text = ConfigurationManager.AppSettings.Get("Title");
            SetMenu();
        }

        private void SetMenu()
        {
            this.mnuParent.Text = GetTextConfig("Parent");
            bool resourceVisible = GetBooleanConfig("ResourceVisible");
            this.mnuFood.Visible = resourceVisible;
            this.mnuWater.Visible = resourceVisible;
            this.mnuTent.Visible = resourceVisible;

            bool personVisible = GetBooleanConfig("PersonVisible");
            this.mnuRescue.Visible = personVisible;
            this.mnuElectricity.Visible = personVisible;
            this.mnuFireFighter.Visible = personVisible;
            this.mnuWaterFixer.Visible = personVisible;
            this.mnuCommuFixer.Visible = personVisible;
            this.mnuGasFixer.Visible = personVisible;
        }

        private bool GetBooleanConfig(string key)
        {
            return bool.Parse(ConfigurationManager.AppSettings.Get(key));
        }

        private string GetTextConfig(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        #region Main Menu event handlers
        private void menuNewDoc_Click(object sender, EventArgs e)
        {
            //execute New Document command
            ICommand command = new CreateNewDocument();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuOpenDoc_Click(object sender, EventArgs e)
        {
            //execute Open Document command
            ICommand command = new ControlsOpenDocCommandClass();
            command.OnCreate(axPageLayoutControl1.Object);
            command.OnClick();
        }

        private void menuSaveDoc_Click(object sender, EventArgs e)
        {
            //execute Save Document command
         
            if (m_mapControl.CheckMxFile(m_mapDocumentName))
            {
                //create a new instance of a MapDocument
                IMapDocument mapDoc = new MapDocumentClass();
                mapDoc.Open(m_mapDocumentName, string.Empty);

                //Make sure that the MapDocument is not readonly
                if (mapDoc.get_IsReadOnly(m_mapDocumentName))
                {
                    MessageBox.Show("Map document is read only!");
                    mapDoc.Close();
                    return;
                }

                //Replace its contents with the current map
                mapDoc.ReplaceContents((IMxdContents)m_mapControl.Map);

                //save the MapDocument in order to persist it
                mapDoc.Save(mapDoc.UsesRelativePaths, false);

                //close the MapDocument
                mapDoc.Close();
            }
        }

        private void menuSaveAs_Click(object sender, EventArgs e)
        {
            //execute SaveAs Document command
            ICommand command = new ControlsSaveAsDocCommandClass();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuExitApp_Click(object sender, EventArgs e)
        {
            //exit the application
            Application.Exit();
        }
        #endregion

        //listen to MapReplaced evant in order to update the statusbar and the Save menu
        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            //get the current document name from the MapControl
            m_mapDocumentName = m_mapControl.DocumentFilename;

            //if there is no MapDocument, diable the Save menu and clear the statusbar
            if (m_mapDocumentName == string.Empty)
            {
                menuSaveDoc.Enabled = false;
                statusBarXY.Text = string.Empty;
            }
            else
            {
                //enable the Save manu and write the doc name to the statusbar
                menuSaveDoc.Enabled = true;
                statusBarXY.Text = System.IO.Path.GetFileName(m_mapDocumentName);
            }
        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            statusBarXY.Text = string.Format("{0}, {1}  {2}", e.mapX.ToString("#######.##"), e.mapY.ToString("#######.##"), axMapControl1.MapUnits.ToString().Substring(4));
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Dispatcher dispatcher = new Dispatcher();
            //if (dispatcher.Setup())
            //{
            //    dispatcher.Dispatch();
            //    ShowResults(dispatcher);
            //    dispatcher.CreateReport(axMapControl1.ActiveView);
            //}

        }

        private void ShowResults(Dispatcher dispatcher)
        {
            axMapControl1.Map = dispatcher.GetMap();
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null,
                axMapControl1.ActiveView.Extent);

            axMapControl1.ActiveView.Extent.Expand(1.1, 1.1, true);

            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

        }

        private void 饮用水供给ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDispatch input = new FormDispatchWater();
            DispatchResource(input);
            if (_dispatcher != null)
            {
                _dispatcher.ResourceType = EnumResource.Water;
            }
        }

        private void 带标注ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_dispatcher.CreateReport(axPageLayoutControl1.ActiveView))
            {
                MessageBox.Show("导出完毕");
            }
            else
            {
                MessageBox.Show("导出失败，可能是因为报告已打开，请检查日志。");
            }
        }

        private void 不带标注ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _dispatcher.ToggleLabel(axPageLayoutControl1.ActiveView.FocusMap, false);
            if (_dispatcher.CreateReport(axMapControl1.ActiveView))
            {
                MessageBox.Show("导出完毕");
            }
            else
            {
                MessageBox.Show("导出失败，可能是因为报告已打开，请检查日志。");
            }
            _dispatcher.ToggleLabel(axMapControl1.Map, true);
        }

        private void 测试导出报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            带标注ToolStripMenuItem.Enabled = _dispatcher != null;
            不带标注ToolStripMenuItem.Enabled = _dispatcher != null;
        }

        private void 方便食品供给ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDispatch input = new FormDispatchFood();
            DispatchResource(input);
            if (_dispatcher != null)
            {
                _dispatcher.ResourceType = EnumResource.Food;
            }
        }

        private void 救灾帐篷供给ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDispatch input = new FormDispatchTents();
            DispatchResource(input);
            if (_dispatcher != null)
            {
                _dispatcher.ResourceType = EnumResource.Tent;
            }
        }

        private void 消防员分配ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DispatchResource(new FormDispatchFireFighter());
            if (_dispatcher != null)
            {
                _dispatcher.ResourceType = EnumResource.FireFighter;
            }
        }

        private void DispatchResource(FormDispatch input)
        {
            if (input.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _dispatcher = input.Dispatcher;
                //axMapControl1.Map = _dispatcher.GetMap();
               // axMapControl1.Map =
                _dispatcher.GetMap(axPageLayoutControl1);
                axPageLayoutControl1.Visible = true;
            }
        }

        private void 电力抢修人员分配ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DispatchResource(new FormDispatchElectricity());
            if (_dispatcher != null)
            {
                _dispatcher.ResourceType = EnumResource.Electricity;
            }
        }

        private void 救援队伍分配ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DispatchResource(new FormDispatchRescue());
            if (_dispatcher != null)
            {
                _dispatcher.ResourceType = EnumResource.Rescue;
            }
        }

        private void 供水设备抢修人员分配ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DispatchResource(new FormDispatchWaterFixer());
            if (_dispatcher != null)
            {
                _dispatcher.ResourceType = EnumResource.WaterFixer;
            }
        }

        private void showExtentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnvelope env = axPageLayoutControl1.Extent;
            string extent = string.Format("xmin ymin xmax ymax {0},{1},{2},{3}",
                env.XMin, env.YMin, env.XMax, env.YMax);
            MessageBox.Show(extent);

            IEnvelope e2 = new EnvelopeClass();
            e2.PutCoords(1478084, 4624822, 1521170, 4662624);
            (axPageLayoutControl1.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation
                .VisibleBounds = e2;
            axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            //axPageLayoutControl1.Extent = e2;
        }

        private void 通讯基站抢修人员分配ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DispatchResource(new FormDispatchComuFixer());
            if (_dispatcher != null)
            {
                _dispatcher.ResourceType = EnumResource.Communication;
            }
        }

        private void 燃气ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DispatchResource(new FormDispatchGasFixer());
            if (_dispatcher != null)
            {
                _dispatcher.ResourceType = EnumResource.Gas;
            }
        }
    }
}