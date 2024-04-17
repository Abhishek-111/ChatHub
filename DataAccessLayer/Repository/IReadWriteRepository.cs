using DataAccessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public interface IReadWriteRepository
    {
        Task<IEnumerable<T>>GetAllData<T>()
             where T:BaseEntity;
        Task<int> Add<T>(T entity)
            where T:BaseEntity;
       // Task<IEnumerable<Message>> GetPrivateChatHistory(string fromUser, string toUser);
        Task<IEnumerable<Message>> GetChatHistory(DateTime from, DateTime to, string fromUserName, string toUserName);
        Task<IEnumerable<DateTime>> GetTop2Dates(DateTime date, string from, string to, bool passed);
        Task<FileByteArray> GetFileByteArray(int fileId);
        Task<string> GetFileExtension(int fileId);


    }
}
