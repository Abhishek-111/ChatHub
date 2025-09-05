using BusinessLogicLayer.Helper;
using DataAccessLayer.Entity;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Global.Modals.Dtos;
//using DataAccessLayer.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class ChatService : IChatService
    {
        private static readonly Dictionary<string, string> OnlineUsers = new Dictionary<string, string>();
        private readonly IReadWriteRepository _readWriteRepository;
        

        public ChatService(IReadWriteRepository readWriteRepository)
        {
            _readWriteRepository = readWriteRepository;
        }
        public async Task<int> AddUser(UserDTO userDto)
        {

            // if user already online list return 0
            if (OnlineUsers.ContainsKey(userDto.Name))
            {
                return 0;
            }

            OnlineUsers.Add(userDto.Name, null);

            // First Check user exist in db or not
            int userId = await DoesUserExist(userDto.Name).ConfigureAwait(false) ?? -1;
            if (userId != -1)
            {
                return userId;
            }

            return await AddNewUserToDatabase(userDto, userDto.Name);
        }

        public async Task<int> AddNewUserToDatabase(UserDTO userDto, string name)
        {
            var registeredUserId = await _readWriteRepository.Add<User>(ModelConverter.UserDtoToModel(userDto));
            if (registeredUserId > 0)
            {
                return registeredUserId;
            }
            return 0;
        }

        public async Task<bool> SavePrivateChat(MessageDTO message)
        {
            var result = await _readWriteRepository.Add<Message>(ModelConverter.MessageDtoToModel(message));
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<MessageDTO>> GetPrivateChatHistory(GroupDTO group)
        {
            var privateChatHistory = new List<MessageDTO>();
            var chatHistory = await _readWriteRepository.GetChatHistory(group.From, group.To, group.MessageFrom, group.MessageTo);
            if (chatHistory?.Any() == true)
            {
                foreach (var msg in chatHistory)
                {
                    privateChatHistory.Add(ModelConverter.MessageModelToDto(msg));
                }
                return privateChatHistory;
            }
            return null;
        }

        //public async Task<IEnumerable<MessageDTO>> GetPrivateChatHistory(GroupDTO group)
        //{
        //    var privateChatHistory = new List<MessageDTO>();
        //    var chatHistory = await _readWriteRepository.GetPrivateChatHistory(group.MessageFrom, group.MessageTo);
        //    if (chatHistory?.Any() == true)
        //    {
        //        foreach (var msg in chatHistory)
        //        {
        //            privateChatHistory.Add(ModelConverter.MessageModelToDto(msg));
        //        }
        //        return privateChatHistory;
        //    }
        //    return null;
        //}

        // Get all online users
        public string[] GetOnlineUsers()
        {
            lock (OnlineUsers)
            {
                return OnlineUsers.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }
        }

        public bool IsUserOnline(string userName)
        {
            //lock (OnlineUsers)
            //{
            foreach (var user in OnlineUsers)
            {
                if (user.Key.ToLower() == userName.ToLower())
                {
                    return true;
                }
            }
            return false;
            //}
        }

        public void AddUserConnectionId(string user, string connectionId)
        {
            lock (OnlineUsers)
            {
                if (OnlineUsers.ContainsKey(user))
                {
                    OnlineUsers[user] = connectionId;
                }
            }
        }

        public string GetUserByConnectionId(string connectionId)
        {
            //lock (OnlineUsers)
            //{
            return OnlineUsers.Where(x => x.Value == connectionId).Select(x => x.Key).FirstOrDefault();
            //}
        }

        public string GetConnectionIdByUser(string user)
        {
            lock (OnlineUsers)
            {
                return OnlineUsers.Where(x => x.Key == user).Select(x => x.Value).FirstOrDefault();
            }
        }

        // Fetch all saved users from the database
        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            var allUsers = new List<UserDTO>();
            var result = await _readWriteRepository.GetAllData<User>();
            if (result?.Any() == true)
            {
                foreach (var user in result)
                {
                    allUsers.Add(ModelConverter.UserModelToDto(user));
                }
                return allUsers;
            }
            return null;
        }

        // Check Whether User exists for not 
        private async Task<int?> DoesUserExist(string userName)
        {
            var allUsers = await GetAllUsers().ConfigureAwait(false);
            var existingUser = allUsers?.FirstOrDefault(user => user.Name == userName);
            return existingUser?.Id;
        }

        // File upload 
        public async Task<int> FileUpload(MessageDTO fileModel)
        {
            var fileBytesArray = new FileByteArray();
            var fileToAdd = new DataAccessLayer.Entity.File();
            var addedFileId = 0;

            if (fileModel != null && !string.IsNullOrEmpty(fileModel.FileContent))
            {
                string base64Content = fileModel.FileContent.Split(',')[1];
                byte[] fileBytes = Convert.FromBase64String(base64Content);

                fileToAdd.FileType = fileModel.FileType;
                fileToAdd.FileName = fileModel.FileName;
                fileToAdd.UploadedBy = fileModel.From;
                fileToAdd.DateUploaded = DateTime.Now;

                var fileId = await _readWriteRepository.Add<DataAccessLayer.Entity.File>(fileToAdd);
                
                // Add to FileBytesArray
                fileBytesArray.Content = fileBytes;
                fileBytesArray.FileId = fileId;

                addedFileId = await _readWriteRepository.Add<FileByteArray>(fileBytesArray);

                // update message table
                var message = new Message()
                {
                    FromUserName = fileModel.From,
                    ToUserName = fileModel.To,
                    Content = fileModel.Content,
                    TimeStamp = DateTime.Now,
                    FileId = fileId
                };
                var messageId = await _readWriteRepository.Add<Message>(message);
                return fileId;

            }
            return -1;

        }

        // Download file based on fileId
        public async Task<FileDTO> DownloadFile(int fileId)
        {
            try
            {
                var fileByteArray = await _readWriteRepository.GetFileByteArray(fileId);
                if (fileByteArray != null)
                {
                    
                    string fileExtension = await _readWriteRepository.GetFileExtension(fileId);

                    string base64Content = Convert.ToBase64String(fileByteArray.Content);

                    var response = new FileDTO
                    {

                        FileContent = $"data:{fileExtension};base64,{base64Content}",
                        FileType = $"{fileExtension}"

                    };
                    return response;
                   
                }
                
            }
            catch(Exception ex)
            {
               
            }
            return null;

        }

        public async Task<IEnumerable<DateTime>> GetTop2Dates(DateDTO date)
        {
            var list = await _readWriteRepository.GetTop2Dates(date.timeStamp, date.from, date.to, date.passed);
            return list;
        }

    }
}
