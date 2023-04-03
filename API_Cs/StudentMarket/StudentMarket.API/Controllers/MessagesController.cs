using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentMarket.BL.MessageBL;
using StudentMarket.Common.Entities;

namespace StudentMarket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MessagesController : BasesController<Message>
    {
        #region Field
        private IMessageBL _messageBL;
        #endregion

        public MessagesController(IMessageBL messageBL) : base(messageBL)
        {
            _messageBL = messageBL;
        }
    }
}
