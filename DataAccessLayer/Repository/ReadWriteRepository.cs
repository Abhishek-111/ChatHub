using DataAccessLayer.Data;
using DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class ReadWriteRepository : IReadWriteRepository
    {
        private readonly ChatContext _context = null;
       // private readonly DbSet<T> entities;

        public ReadWriteRepository(ChatContext context)
        {
            _context = context;
           // entities = _context.Set<T>();
        }

        public async Task<int> Add<T>(T model) where T : BaseEntity
        {
            
            await _context.AddAsync<T>(model).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return model.Id;
        }

      


        // Returns the list of all users
        public async Task<IEnumerable<T>> GetAllData<T>() where T : BaseEntity
        {

            return await _context.Set<T>().ToListAsync().ConfigureAwait(false);
        }
       
        // Fetch the private chat history between two users
        public async Task<IEnumerable<Message>> GetPrivateChatHistory(string fromUser, string toUser)
        {
            return await _context.Messages
                .Where(msg => (msg.FromUserName == fromUser && msg.ToUserName == toUser) ||
                (msg.FromUserName == toUser && msg.ToUserName == fromUser)).OrderBy(msg => msg.TimeStamp).ToListAsync();
         
        }

    }
}
