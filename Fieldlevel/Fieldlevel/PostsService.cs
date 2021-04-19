using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fieldlevel
{
    public interface IPostsService
    {
        Task<IEnumerable<Post>> GetPosts();
    }

    public class PostsService : IPostsService
    {
        private readonly HttpClient _httpClient;

        public PostsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var response = await _httpClient.GetAsync("/posts");
            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var posts = JsonSerializer.Deserialize<IEnumerable<Post>>(await response.Content.ReadAsStringAsync(), options);

            return posts;
        }
    }
}
