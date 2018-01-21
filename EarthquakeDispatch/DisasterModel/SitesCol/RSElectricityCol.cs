using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace DisasterModel.Forms
{
    public class RSElectricityCol : RefugeeSiteCol
    {
        public static string SeField = "Se";
        public static string LeField = "Le";
        public static string DeField = "De";
        public static string DamagedLengthField = "损毁长度";
        public static string CoeField = "地区系数";

        public static double alpha = 0.76, beta = 0.18, gama = 0.06;

        private int _idxSe = -1, _idxLe = -1, _idxDe = -1, _idxDamagedLen = -1, _idxCoe = -1;

        protected override RefugeeSite CreateSite(ESRI.ArcGIS.Geodatabase.IFeature feature)
        {
            double se = double.Parse(feature.get_Value(_idxSe).ToString());
            double le = double.Parse(feature.get_Value(_idxLe).ToString());
            double de = double.Parse(feature.get_Value(_idxDe).ToString());
            double dmgLen = double.Parse(feature.get_Value(_idxDamagedLen).ToString());
            double coe = double.Parse(feature.get_Value(_idxCoe).ToString());

            RSElectricity site = new RSElectricity()
            {
                OID = feature.OID,
                Location = feature.ShapeCopy as IPoint,
            };

            site.Priority = alpha * se + beta * le + gama * de;

            site.ResourceInNeed = (int) (coe * 1.5 * dmgLen); 
            return site;
        }

        internal void Setup(Dispatcher dispatcher)
        {
            this._fc = dispatcher.SiteFeatureClass;

            _idxSe = _fc.FindField(SeField);
            _idxLe = _fc.FindField(LeField);
            _idxDe = _fc.FindField(DeField);
            _idxDamagedLen = _fc.FindField(DamagedLengthField);
            _idxCoe = _fc.FindField(CoeField);

            _refugeeSites = GetRefugeeSites();

        }
    }
}
