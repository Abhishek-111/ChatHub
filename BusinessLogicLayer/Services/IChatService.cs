using DataAccessLayer.Entity;
using Shared.Global.Modals.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface IChatService
    {
        Task<int> AddUser(UserDTO userDto);
        Task<int> AddNewUserToDatabase(UserDTO userDto, string name);
        Task<bool> SavePrivateChat(MessageDTO message);
        Task<IEnumerable<MessageDTO>> GetPrivateChatHistory(GroupDTO group);
        string[] GetOnlineUsers();
        bool IsUserOnline(string userName);
        void AddUserConnectionId(string user, string connectionId);
        string GetUserByConnectionId(string connectionId);
        string GetConnectionIdByUser(string user);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        //Task<int?> DoesUserExist(string userName);
        //Task<int> AddMessages(MessageDTO text);

    }
}
