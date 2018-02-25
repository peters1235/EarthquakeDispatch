using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace DisasterModel
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RefugeeSite
    {
        public int OID { get; set; }
        public IPoint Location { get; set; }
        public double Priority
        {
            get;
            set;
        }

        internal abstract string ResourceName();

        internal  int  ResourceInNeed{get;set;}

        internal abstract string ResourceUnit();

        public void ReplenishResource(int amount)
        {
            ResourceInNeed -= amount;
        }
    }

    public class RefugeeSiteWater : RefugeeSite
    {
        internal override string ResourceName()
        {
            return "饮用水";
        }
        internal override string ResourceUnit()
        {
            return "L";
        }
      
    }

    public class RefugeeSiteFood : RefugeeSite
    {
        internal override string ResourceName()
        {
            return "方便食品";
        }
        internal override string ResourceUnit()
        {
            return "包";
        }

    }

    public class RefugeeSiteTent : RefugeeSite
    {
        public int PeopleNeedTent { get; set; }

        internal override string ResourceName()
        {
            return "帐篷";
        }
        internal override string ResourceUnit()
        {
            return "顶";
        }
    }

    public class RSElectricity : RefugeeSite
    {
        internal override string ResourceName()
        {
            return "电力抢修人员";
        }

        internal override string ResourceUnit()
        {
            return "名";
        }
    }

    public class RefugeeSiteRescue : RefugeeSite
    {
        internal override string ResourceName()
        {
            return "救援人员";
        }

        internal override string ResourceUnit()
        {
            return "名";
        }
    }

    class RefugeeSiteFireFighter : RefugeeSite
    {
        internal override string ResourceName()
        {
            return "消防员";
        }

        internal override string ResourceUnit()
        {
            return "名";
        }
    }

    class RefugeeSiteWaterFixer : RefugeeSite
    {
        internal override string ResourceName()
        {
            return "供水维修人员";
        }

        internal override string ResourceUnit()
        {
            return "名";
        }
    }

    class RefugeeSiteCommuFixer : RefugeeSite
    {
        internal override string ResourceName()
        {
            return "通讯基站抢修人员";
        }

        internal override string ResourceUnit()
        {
            return "名";
        }
    }
}
