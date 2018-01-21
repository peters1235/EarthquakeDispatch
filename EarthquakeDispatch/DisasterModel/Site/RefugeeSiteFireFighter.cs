using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisasterModel.SitesCol
{
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
}
