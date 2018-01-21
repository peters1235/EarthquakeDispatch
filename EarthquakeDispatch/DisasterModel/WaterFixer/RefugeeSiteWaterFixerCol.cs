using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using DisasterModel.SitesCol;

namespace DisasterModel.Forms
{
    public class RefugeeSiteWaterFixerCol : RefugeeSiteCol
    {
        public static string SwField = "Sw";
        public static string LwField = "Lw";
        public static string DwField = "Dw";

        private int _idxSw = -1, _idxLw = -1, _idxDw = -1;
        internal void Setup(Dispatcher dispatcher)
        {
            this._fc = dispatcher.SiteFeatureClass;
            _idxDw = _fc.FindField(DwField);
            _idxLw = _fc.FindField(LwField);
            _idxSw = _fc.FindField(SwField);

            _refugeeSites = GetRefugeeSites();
        }

        protected override RefugeeSite CreateSite(ESRI.ArcGIS.Geodatabase.IFeature feature)
        {
            double sw = double.Parse(feature.get_Value(_idxSw).ToString());
            double lw = double.Parse(feature.get_Value(_idxLw).ToString());
            double dw = double.Parse(feature.get_Value(_idxDw).ToString());

            RefugeeSiteWaterFixer site = new RefugeeSiteWaterFixer()
            {
                OID = feature.OID,
                Location = feature.ShapeCopy as IPoint,
            };

            if (dw == 0 || dw == 1)
            {
                site.Priority = 0;
                return null;
            }
            else
            {
                double alpha = 0.67, beta = 0.28, gama = 0.05;
                site.Priority = sw * alpha + lw * beta + dw * gama;
                int personPerSite = 8;
                site.ResourceInNeed = personPerSite;
                return site;
            }
        }
    }
}
