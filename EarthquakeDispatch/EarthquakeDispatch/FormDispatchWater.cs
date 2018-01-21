using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EarthquakeDispatch;


namespace DisasterModel
{
    public class FormDispatchWater : FormDispatch
    {
        protected override UCParas GetUC()
        {
            return new UCWater();
        }
    }
}
