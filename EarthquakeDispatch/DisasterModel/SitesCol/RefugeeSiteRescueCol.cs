using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace DisasterModel.Forms
{
    class RefugeeSiteRescueCol : RefugeeSiteCol
    {
        public static string PopuField = "埋压人数";
        public static string DensityField = "人口密度";
        public static string IntensityField = "地震烈度";
        private int _idxPop = -1, _idxDensity = -1, _idxIntensity = -1;

        protected override RefugeeSite CreateSite(ESRI.ArcGIS.Geodatabase.IFeature feature)
        {
            int pop = int.Parse(feature.get_Value(_idxPop).ToString());
            double density = double.Parse(feature.get_Value(_idxDensity).ToString());
            int intensity = int.Parse(feature.get_Value(_idxIntensity).ToString());

            RefugeeSiteRescue site = new RefugeeSiteRescue()
            {
                OID = feature.OID,
                Location = feature.ShapeCopy as IPoint,
            };

            site.Priority = P(density) + M(pop) + I(intensity);

            site.ResourceInNeed = pop * 20;
            return site;
        }

        private int I(int intensity)
        {
            if (intensity == 6 || intensity == 7)
            {
                return 1;
            }

            if (intensity == 8)
            {
                return 2;
            }
            if (intensity == 9)
            {
                return 3;
            }
            if (intensity >= 10)
            {
                return 4;
            }
            return 0;
        }

        private int M(int pop)
        {
            if (pop <= 0)
            {
                return 1;
            }
            else
            {
                return 4;
            }
        }

        private int P(double density)
        {
            if (density < 0.5)
            {
                return 1;
            }
            else if ( density < 1)
            {
                return 2;
            }
            else if ( density < 2)
            {
                return 3;
            }
            else // if (density >= 2)
            {
                return 4;
            }
        }

        internal void Setup(Dispatcher dispatcher)
        {
            this._fc = dispatcher.SiteFeatureClass;
            _idxPop = _fc.FindField(PopuField);
            _idxDensity = _fc.FindField(DensityField);
            _idxIntensity = _fc.FindField(IntensityField);

            _refugeeSites = GetRefugeeSites();
        }
    }
}
