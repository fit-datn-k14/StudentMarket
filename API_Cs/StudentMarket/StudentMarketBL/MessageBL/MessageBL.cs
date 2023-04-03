using StudentMarket.Common.Entities;
using StudentMarket.DL.MessageDL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.BL.MessageBL
{
    public class MessageBL : BaseBL<Message>, IMessageBL
    {
        #region field

        private IMessageDL _messageDL;

        #endregion

        #region contructor

        public MessageBL(IMessageDL messageDL) : base(messageDL)
        {
            _messageDL = messageDL;
        }

        #endregion

        #region Method

        #endregion
    }
}
