﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.ADF;

namespace DisasterModel
{
    public abstract class RefugeeSiteCol
    {
        protected List<RefugeeSite> _refugeeSites = null;
        protected IFeatureClass _fc;
        private int GetInsertIndex(List<RefugeeSite> col, RefugeeSite site)
        {
            int index = 0;
            for (int i = 0; i < col.Count; i++)
            {
                RefugeeSite s = col[i];
                if (site.Priority < s.Priority)
                {
                    index++;
                }
            }
            return index;
        }

        public List<RefugeeSite> Sites { get { return _refugeeSites; } }

        private int _totalResourceNeeds = 0;
        public int TotalResourceNeeds
        {
            get { return _totalResourceNeeds; }
        }

        internal IQueryFilter SiteFilter(RefugeeSite site)
        {
            IQueryFilter filter = new QueryFilterClass();
            filter.WhereClause = _fc.OIDFieldName + " = " + site.OID.ToString();
            return filter;
        }

        /// <summary>
        /// 分配完成后，总的还缺少多少资源
        /// </summary>
        /// <returns></returns>
        public int GetResourceInShort()
        {
            int result = 0;
            for (int i = 0; i < _refugeeSites.Count; i++)
            {
                result += _refugeeSites[i].ResourceInNeed;                
            }
            return result;
        }

        protected List<RefugeeSite> GetRefugeeSites()
        {
            _totalResourceNeeds = 0;
            List<RefugeeSite> results = new List<RefugeeSite>();
            IFeatureCursor cursor = null;
            try
            {
                cursor = _fc.Search(null, true);
                IFeature feature = cursor.NextFeature();

                while (feature != null)
                {
                    RefugeeSite site = CreateSite(feature);
                    if (site == null)
                    {
                        feature = cursor.NextFeature();
                        continue;
                    }

                    int insertIndex = GetInsertIndex(results, site);
                    results.Insert(insertIndex, site);
                    feature = cursor.NextFeature();

                    _totalResourceNeeds += site.ResourceInNeed;
                }
                return results;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
            finally
            {
                ComReleaser.ReleaseCOMObject(cursor);
            }
        }

        protected abstract RefugeeSite CreateSite(IFeature feature);

        public IFeatureClass FeatureClass { get { return _fc; } }

        //internal void ReplenishResource(RefugeeSite site, int amount)
        //{
        //    site.ResourceInNeed -= amount;
        //}

        internal string RescuePriority(string siteType)
        {
            string result = "";
            foreach (var item in _refugeeSites)
            {
                result += siteType+item.OID.ToString() + ",";
            }
            return result.TrimEnd(',');
        }
    }
}
