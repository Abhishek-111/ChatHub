using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.SignalR;
using Shared.Global.Modals.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.HubConfig
{
    public class MyHub : Hub
    {
        private readonly IChatService _chatService;
        public MyHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        // Executes when user logs
        public override async Task OnConnectedAsync()
        {
            // Creates a group "OnlineChat"
            await Groups.AddToGroupAsync(Context.ConnectionId, "OnlineChat");
            await Clients.Caller.SendAsync("UserConnected");
        }
        public async Task AddUserConnectionId(string name)
        {
            _chatService.AddUserConnectionId(name, Context.ConnectionId);
            await AllOnlineUsers();
        }

        // Displays all online users (from the dictonary in ChatServer)
        private async Task AllOnlineUsers()
        {
            var onlineUsers = _chatService.GetOnlineUsers();
            await Clients.Groups("OnlineChat").SendAsync("OnlineChat", onlineUsers);
        }

        public async Task CreatePrivateChat(MessageDTO message)
        {
            // check wheter "to" user is online or not!
            var IsOnline = _chatService.IsUserOnline(message.To);
            if (IsOnline)
            {
                // save private chat
                var result = await _chatService.SavePrivateChat(message);

                string privateGroupName = GetPrivateGroupName(message.From, message.To);
                await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName); // current user connectionId
                var toConnectionId = _chatService.GetConnectionIdByUser(message.To);
                await Groups.AddToGroupAsync(toConnectionId, privateGroupName);

                // [Aman-Naman] sending private message to the another user 'Naman'
                await Clients.Client(toConnectionId).SendAsync("OpenPrivateChat", message);
            }
            else
            {
                // If To usr is not online - chats will go in database
                var result = await _chatService.SavePrivateChat(message);
                // send msg to the same user.
                await Clients.Client(Context.ConnectionId).SendAsync("MessageOfShelf",message);
            }

        }

        public async Task ReceivePrivateMessage(MessageDTO message)
        {
            // save private chat
            var result = await _chatService.SavePrivateChat(message);
            string privateGroupName = GetPrivateGroupName(message.From, message.To);
            await Clients.Group(privateGroupName).SendAsync("NewPrivateMessage", message);
        }

        private string GetPrivateGroupName(string from, string to)
        {
            var stringCompare = string.CompareOrdinal(from, to) < 0;
            return stringCompare ? $"{from}-{to}" : $"{to}-{from}";
        }

        


    }
}
