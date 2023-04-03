using StudentMarket.Common.Entities;
using StudentMarket.DL.LocationDL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.BL.LocationBL
{
    public class LocationBL : BaseBL<Location>, ILocationBL
    {
        #region field

        private ILocationDL _locationDL;

        #endregion

        #region contructor

        public LocationBL(ILocationDL locationDL) : base(locationDL)
        {
            _locationDL = locationDL;
        }

        #endregion

        #region Method

        #endregion
    }
}
