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
        Task<IEnumerable<Message>> GetPrivateChatHistory(string fromUser, string toUser);
        //Task<bool> DoesUserExist<T>(string userName) where T : BaseEntity;
        
    }
}
