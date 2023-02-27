using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Domain;
using Tweetbook.Data;

namespace Tweetbook.Services
{
    public class TagService : ITagService
    {
        private readonly DataContext _dataContext;

        public TagService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> DeleteTagAsync(Guid tagId)
        {
            var tag = await GetTagByIdAsync(tagId);
            if (tag == null)
                return false;
            _dataContext.Tags.Remove(tag);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        private async Task<Tag> GetTagByIdAsync(Guid tagId)
        {
            return await _dataContext.Tags.SingleOrDefaultAsync(x => x.Id == tagId);
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            return await _dataContext.Tags.ToListAsync();
        }


    }
}
