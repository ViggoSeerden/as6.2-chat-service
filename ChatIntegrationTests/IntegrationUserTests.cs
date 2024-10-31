using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ChatIntegrationTests
{
    [TestClass]
    public class IntegrationUserTests
    {
        private readonly HttpClient _client;

        public IntegrationUserTests()
        {
            var factory = new MockFactory();
            using (factory.Services.CreateScope())
            {
                // var provider = scope.ServiceProvider;
                // using (var APIContext = provider.GetRequiredService<APIContext>())
                // {
                //     APIContext.Database.EnsureCreatedAsync();
                //
                //     APIContext.Users.AddAsync(new User { Username = "oggiVAdmin", Id = 1 });
                //     APIContext.Users.AddAsync(new User { Username = "oggiVUser", Id = 2 });
                //     APIContext.SaveChangesAsync();
                // }
                _client = factory.CreateClient();
            }
        }

        [TestMethod]
        public async Task GetSkeleton()
        {
            var response = await _client.GetAsync("/chats/skeleton");
            var value = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.IsTrue(value.Contains("This is the Chat Service Skeleton endpoint."));
        }
    }
}