using DataAccessLayer.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        Task<FileDTO> DownloadFile(int fileId);
        Task<int> FileUpload(MessageDTO fileModel);
        Task<IEnumerable<DateTime>> GetTop2Dates(DateDTO date);

    }
}
