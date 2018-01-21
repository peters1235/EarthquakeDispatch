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

namespace EarthquakeDispatch
{
    public sealed partial class MainForm : Form
    {
        #region class private members
        private IMapControl3 m_mapControl = null;
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

            //disable the Save menu (since there is no document yet)
            menuSaveDoc.Enabled = false;

            Setup();
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
            command.OnCreate(m_mapControl.Object);
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
                statusBarXY.Text = Path.GetFileName(m_mapDocumentName);
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

        private void ����ˮ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDispatch input = new FormDispatchWater();
            DispatchResource(input);
            if (_dispatcher != null)
            {
                _dispatcher.ResourceType = EnumResource.Water;
            }
        }

        private void ����עToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_dispatcher.CreateReport(axPageLayoutControl1.ActiveView))
            {
                MessageBox.Show("�������");
            }
            else
            {
                MessageBox.Show("����ʧ�ܣ���������Ϊ�����Ѵ򿪣�������־��");
            }
        }

        private void ������עToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _dispatcher.ToggleLabel(axPageLayoutControl1.ActiveView.FocusMap, false);
            if (_dispatcher.CreateReport(axMapControl1.ActiveView))
            {
                MessageBox.Show("�������");
            }
            else
            {
                MessageBox.Show("����ʧ�ܣ���������Ϊ�����Ѵ򿪣�������־��");
            }
            _dispatcher.ToggleLabel(axMapControl1.Map, true);
        }

        private void ���Ե�������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ����עToolStripMenuItem.Enabled = _dispatcher != null;
            ������עToolStripMenuItem.Enabled = _dispatcher != null;
        }

        private void ����ʳƷ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDispatch input = new FormDispatchFood();
            DispatchResource(input);
            if (_dispatcher != null)
            {
                _dispatcher.ResourceType = EnumResource.Food;
            }
        }

        private void �������񹩸�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDispatch input = new FormDispatchTents();
            DispatchResource(input);
            if (_dispatcher != null)
            {
                _dispatcher.ResourceType = EnumResource.Tent;
            }
        }

        private void ����Ա����ToolStripMenuItem_Click(object sender, EventArgs e)
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
            }
        }

        private void ����������Ա����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DispatchResource(new FormDispatchElectricity());
            if (_dispatcher != null)
            {
                _dispatcher.ResourceType = EnumResource.Electricity;
            }
        }

        private void ��Ԯ�������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DispatchResource(new FormDispatchRescue());
            if (_dispatcher != null)
            {
                _dispatcher.ResourceType = EnumResource.Rescue;
            }
        }
    }
}