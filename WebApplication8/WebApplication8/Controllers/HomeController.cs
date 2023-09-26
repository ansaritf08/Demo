using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Microsoft.Azure.Cosmos;
using StackExchange.Redis;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNet.SignalR.Redis;

namespace WebApplication8.Controllers
{
    [Route("[controller]")]
   [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]

    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private const string EndpointUrl = "https://dev-azure-account.documents.azure.com:443/";
        private const string AuthorizationKey = "1NKx9PN2ib0F847j1bz5zfJ72R1MY7Kda54URA3em0FWzaTKurQuVs4d0XeOR8oXKX1TjQziM11OACDbQFOXBQ==";
        private const string DatabaseId = "UserManagement";
        private const string ContainerId = "User";
       
        private const string ConnectionString = @"AccountEndpoint=https://dev-azure-account.documents.azure.com:443/;AccountKey=1NKx9PN2ib0F847j1bz5zfJ72R1MY7Kda54URA3em0FWzaTKurQuVs4d0XeOR8oXKX1TjQziM11OACDbQFOXBQ==;";
        private static Task<RedisConnection> _redisConnectionFactory = RedisConnection.InitializeAsync(connectionString: ConfigurationManager.AppSettings["CacheConnection"].ToString()););

        [Authorize(Roles ="Manager")]
        [HttpGet("ManagerRoleApi")]
        [HttpGet]
        public IActionResult GetManagerRoleApi()
        {
            return Ok(new { Suceess = "Success" , Role ="Manager" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("AmdinRoleApi")]
        public IActionResult GetAmdinRoleApi()
        {
            return Ok(new { Suceess = "Success", Role = "Admin" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("CreateUser")]
        public async  Task<IActionResult> CreateUser([FromBody]User user)
        {

            List<string> result = new List<string>() {
                _configuration["ContainerId"],
                _configuration["DatabaseId"],
                _configuration["AuthorizationKey"],
                _configuration["EndpointUrl"],
                _configuration["EventHubConnectionString"]
           
            };
            CosmosClient cosmosClient = new CosmosClient(result[3], result[2]);
            Container container = cosmosClient.GetContainer(result[1], result[0]);
            if (user != null)
            {
                user.id = Guid.NewGuid().ToString();
                var item = await container.CreateItemAsync<User>(user);
                if (item.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return Ok(new { Suceess = "Success", Message = "User is added successfully" });
                }
                //if(item.StatusCode==)
            }
            return Ok(new { Failure = "Failure", Role = "User is added successfully" });
        }
    }
}
