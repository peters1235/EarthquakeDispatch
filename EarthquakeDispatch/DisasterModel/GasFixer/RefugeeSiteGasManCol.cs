using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using DisasterModel.SitesCol;

namespace DisasterModel.Forms
{
    public class RefugeeSiteGasManCol : RefugeeSiteCol
    {
        public static string SrField = "Sr";
        public static string LrField = "Lr";
        public static string DrField = "Dr";

        private int _idxSr = -1, _idxLr = -1, _idxDr = -1;
        internal void Setup(Dispatcher dispatcher)
        {
            this._fc = dispatcher.SiteFeatureClass;
            _idxDr = _fc.FindField(DrField);
            _idxLr = _fc.FindField(LrField);
            _idxSr = _fc.FindField(SrField);

            _refugeeSites = GetRefugeeSites();
        }

        protected override RefugeeSite CreateSite(ESRI.ArcGIS.Geodatabase.IFeature feature)
        {
            double sr = double.Parse(feature.get_Value(_idxSr).ToString());
            double lr = double.Parse(feature.get_Value(_idxLr).ToString());
            double dr = double.Parse(feature.get_Value(_idxDr).ToString());

            RefugeeSiteGasMan site = new RefugeeSiteGasMan()
            {
                OID = feature.OID,
                Location = feature.ShapeCopy as IPoint,
            };

            if (dr == 0 || dr == 1)
            {
                site.Priority = 0;
                return null;
            }
            else
            {
                double alpha = 0.62, beta = 0.33, gama = 0.05;
                site.Priority = sr * alpha + lr * beta + dr * gama;
                int menPerSite = 6;
                site.ResourceInNeed = menPerSite;
                return site;
            }
        }
    }
}
