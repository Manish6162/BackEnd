using BackEnd.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Collections;
using System.Reflection;
using System.Resources;


namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly Container _usersContainer;

        public UsersController(CosmosDbContext cosmosDbContext)
        {
            _usersContainer = cosmosDbContext.UsersContainer;
        }



        [HttpGet("username")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                string username = GenerateRandomUsername();

              
                    BackEnd.Entities.User
                      newUser = new BackEnd.Entities.User
                      {
                        Username = username,
                         
                        };

                    await _usersContainer.CreateItemAsync(newUser); //, new PartitionKey(newUser.UserId)

                return Ok(new { Message = "User added successfully." });
                //else
                //{
                //    // User already exists in Cosmos DB
                //    return BadRequest(new { Message = "User already exists." });
                //}
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Exception: {ex}");

                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        private string GenerateRandomUsername()
        {
            string adjective = GetRandomResource("Strings_Adjectives");
            string noun = GetRandomResource("Strings_Nouns");
            string number = GenerateRandomNumber();

            return $"{adjective}_{noun}_{number}";
        }

        private string GetRandomResource(string resourceName)
        {
            try
            {
                string resourceKey = $"BackEnd.Resources.{resourceName}.resources";
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream(resourceKey))
                {
                    using (var reader = new ResourceReader(stream))
                    {
                        var resources = reader.OfType<DictionaryEntry>().ToList();
                        int randomIndex = new Random().Next(0, resources.Count);

                        return resources[randomIndex].Value.ToString();
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string GenerateRandomNumber()
        {
            return new Random().Next(100, 1000).ToString();
        }


    }
}
