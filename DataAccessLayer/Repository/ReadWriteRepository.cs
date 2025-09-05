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
        public ReadWriteRepository(ChatContext context)
        {
            _context = context;
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
        //public async Task<IEnumerable<Message>> GetPrivateChatHistory(string fromUser, string toUser)
        //{

        //    var chatHistory = await _context.Messages
        //        .Where(msg => (msg.FromUserName == fromUser && msg.ToUserName == toUser) ||
        //        (msg.FromUserName == toUser && msg.ToUserName == fromUser)).OrderBy(msg => msg.TimeStamp)
        //        .Include(msg => msg.FileAttached)
        //        .ToListAsync();

        //    return chatHistory;

        //}

        // Fetches file Byte Array content from the database by file id
        public async Task<FileByteArray> GetFileByteArray(int fileId)
        {
            var fileByteArray = await _context.FileByteArrays
                .Include(fba => fba.File)
                .FirstOrDefaultAsync(fba => fba.FileId == fileId);

            return fileByteArray;
        }

        // Returns a file extension from Files table By file Id
        public async Task<string> GetFileExtension(int fileId)
        {
            return await _context.Files.Where(file => file.Id == fileId)
                .Select(file => file.FileType).FirstOrDefaultAsync();
        }

       // Fetches the top 2 dates of chat between tow users
        public async Task<IEnumerable<DateTime>> GetTop2Dates(DateTime date, string from, string to, bool passed)
        {
            if (!passed)
            {
                var list = await _context.Messages.Where(x => ((x.FromUserName == from && x.ToUserName == to) || (x.FromUserName == to && x.ToUserName == from)))
                         .Select(x => x.TimeStamp.Date).Distinct().OrderByDescending(x => x.Date).Take(2).ToListAsync();
                list = list.OrderBy(x => x.Date).Take(2).ToList();
                return list;
            }
            else
            {
                var list = await _context.Messages.Where(x => ((x.FromUserName == from && x.ToUserName == to) || (x.FromUserName == to && x.ToUserName == from)) && (x.TimeStamp.Date < date.Date))
                    .Select(x => x.TimeStamp.Date).Distinct().OrderByDescending(x => x.Date).Take(2).ToListAsync();
                list = list.OrderBy(x => x.Date).ToList();
                return list;
            }
        }

        // Fetch the private chat history between two users
        public async Task<IEnumerable<Message>> GetChatHistory(DateTime from, DateTime to, string fromUserName, string toUserName)
        {
            return await _context.Messages.Where(x => ((x.FromUserName == fromUserName && x.ToUserName == toUserName) || (x.FromUserName == toUserName && x.ToUserName == fromUserName)) && (x.TimeStamp.Date == from.Date || x.TimeStamp.Date == to.Date)).OrderBy(x => x.TimeStamp).Include(msg => msg.FileAttached).ToListAsync();

        }

    }
}
