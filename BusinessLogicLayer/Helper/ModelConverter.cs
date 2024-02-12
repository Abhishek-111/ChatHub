using DataAccessLayer.Entity;
using Shared.Global.Modals.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Helper
{
    public static class ModelConverter
    {
        public static User UserDtoToModel(UserDTO model)
        {
            return new User
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static UserDTO UserModelToDto(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name
            };
        }

        public static Message MessageDtoToModel(MessageDTO model)
        {
            return new Message
            {
                Id = model.Id,
                FromUserName = model.From,
                ToUserName = model.To,
                Content = model.Content,
                TimeStamp = DateTime.Now
            };
        }

        public static MessageDTO MessageModelToDto(Message message)
        {
            return new MessageDTO
            {
                Id = message.Id,
                From = message.FromUserName,
                To = message.ToUserName,
                Content = message.Content,
                TimeStamp = message.TimeStamp
                
            };
        }
    }
}
