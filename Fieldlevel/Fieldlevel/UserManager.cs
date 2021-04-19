using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LazyCache;

namespace Fieldlevel
{
    public interface IUserManager
    {
        Task<IEnumerable<Post>> GetMostRecentPost();
    }

    public class CachedUserManager : UserManager, IUserManager
    {
        private readonly IAppCache _cache;

        public CachedUserManager(IPostsService postsService, IAppCache cache)
            : base(postsService)
        {
            _cache = cache;
        }

        public override async Task<IEnumerable<Post>> GetMostRecentPost()
        {
            var result = await _cache.GetOrAddAsync("RecentPosts_v1", base.GetMostRecentPost, DateTime.Now.AddMinutes(1));
            return result;
        }
    }

    public class UserManager : IUserManager
    {
        private readonly IPostsService _postsService;

        public UserManager(IPostsService postsService)
        {
            _postsService = postsService;
        }

        public virtual async Task<IEnumerable<Post>> GetMostRecentPost()
        {
            var allPosts  = await _postsService.GetPosts();

            var mostRecentPostForUsers = 
                from p in allPosts
                group p by p.UserId into rp
                select rp.OrderByDescending(op => op.Id).First();

            return mostRecentPostForUsers;
        }
    }
}
