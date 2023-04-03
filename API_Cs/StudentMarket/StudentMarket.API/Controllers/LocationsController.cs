using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentMarket.BL.LocationBL;
using StudentMarket.Common.Entities;

namespace StudentMarket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LocationsController : BaseController<Location>
    {
        #region Field
        private ILocationBL _locationBL;
        #endregion

        public LocationsController(ILocationBL locationBL) : base(locationBL)
        {
            _locationBL = locationBL;
        }
    }
}
