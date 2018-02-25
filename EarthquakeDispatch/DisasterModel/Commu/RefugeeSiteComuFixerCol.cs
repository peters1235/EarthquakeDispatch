using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using DisasterModel.SitesCol;

namespace DisasterModel.Forms
{
    public class RefugeeSiteComuFixerCol : RefugeeSiteCol
    {
        public static string XField = "x";
        public static string AlphaField = "Sc";
        public static string BetaField = "Lc";
        public static string DField = "Dc";

        private int _idxSc = -1, _idxLc = -1, _idxDc = -1, _idxX = -1;
        internal void Setup(Dispatcher dispatcher)
        {
            this._fc = dispatcher.SiteFeatureClass;
            _idxDc = _fc.FindField(DField);
            _idxLc = _fc.FindField(BetaField);
            _idxSc = _fc.FindField(AlphaField);
           _idxX = _fc.FindField(XField);

            _refugeeSites = GetRefugeeSites();
        }

        protected override RefugeeSite CreateSite(ESRI.ArcGIS.Geodatabase.IFeature feature)
        {
            double sc = double.Parse(feature.get_Value(_idxSc).ToString());
            double lc = double.Parse(feature.get_Value(_idxLc).ToString());
            double dc = double.Parse(feature.get_Value(_idxDc).ToString());
            int x = int.Parse(feature.get_Value(_idxX).ToString());

            RefugeeSiteCommuFixer site = new RefugeeSiteCommuFixer()
            {
                OID = feature.OID,
                Location = feature.ShapeCopy as IPoint,
            };

            if (dc == 0 || dc == 1)
            {
                site.Priority = 0;
                return null;
            }
            else
            {
                double alpha = 0.67, beta = 0.27, gama = 0.06;
                site.Priority = sc * alpha + lc * beta + dc * gama;

                site.ResourceInNeed =(int) Math.Ceiling( Math.Exp(0.833 * Math.Log(x) +2.0299));
                return site;
            }
        }
    }
}
