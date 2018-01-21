using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using DisasterModel.SitesCol;

namespace DisasterModel.Forms
{
    public class RefugeeSiteFireFighterCol : RefugeeSiteCol
    {
        public static string SfField = "Sf";
        public static string LfField = "Lf";
        public static string FireAreaField = "着火面积";

        private int _idxSf = -1, _idxLf = -1, _idxFireArea = -1;
        internal void Setup(Dispatcher dispatcher)
        {
            this._fc = dispatcher.SiteFeatureClass;
            _idxFireArea = _fc.FindField(FireAreaField);
            _idxLf = _fc.FindField(LfField);
            _idxSf = _fc.FindField(SfField);

            _refugeeSites = GetRefugeeSites();
        }

        protected override RefugeeSite CreateSite(ESRI.ArcGIS.Geodatabase.IFeature feature)
        {
            double sf = double.Parse(feature.get_Value(_idxSf).ToString());
            double lf = double.Parse(feature.get_Value(_idxLf).ToString());
            int fireArea = int.Parse(feature.get_Value(_idxFireArea).ToString());

            RefugeeSiteFireFighter site = new RefugeeSiteFireFighter()
            {
                OID = feature.OID,
                Location = feature.ShapeCopy as IPoint,
            };

            site.Priority = 0.6 * lf + 0.4 * lf;

            site.ResourceInNeed = 6 * fireArea / 100;
            return site;
        }
    }
}
