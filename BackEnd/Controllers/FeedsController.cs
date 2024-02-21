using Azure.Storage.Blobs;
using BackEnd.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedsController : ControllerBase
    {
        private readonly Container _feedsContainer;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "mediacontainer";

        public FeedsController(CosmosDbContext cosmosDbContext, BlobServiceClient blobServiceClient)
        {
            _feedsContainer = cosmosDbContext.FeedsContainer;
            _blobServiceClient = blobServiceClient;
        }

        [HttpGet("{feedId}")]
        public async Task<IActionResult> GetFeed(string feedId)
        {
            try
            {
                var feedResponse = await _feedsContainer.ReadItemAsync<Feeds>(feedId, new PartitionKey(feedId));
                return Ok(feedResponse.Resource);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(new { Message = "Feed not found." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateFeedWithBlob([FromForm] IFormFile file, [FromForm] string uploaderUserName)
        {
            try
            {
                // Check if the request has a file
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                // Create a new feed object
                var feed = new Feeds
                {
                    FeedId = Guid.NewGuid().ToString(),
                    CurrentTime = DateTime.UtcNow,
                    UploaderUserId = uploaderUserName,
                    Likes = 0,
                    Comments = 0,
                    Url = string.Empty // Set the URL to empty for now
                };

                // Generate a unique file name for blob storage
                string fileName = $"{feed.FeedId}_{file.FileName}";

                // Get the Blob container client
                BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(_containerName);

                // Get the Blob client for the new file
                BlobClient blob = container.GetBlobClient(fileName);

                // Upload the file to Blob storage
                using (var stream = file.OpenReadStream())
                {
                    await blob.UploadAsync(stream);
                }

                // Set the feed URL to the Blob URI
                feed.Url = blob.Uri.ToString();

                // Add the new feed record to Cosmos DB
                await _feedsContainer.CreateItemAsync(feed);

                // Return success response
                return Ok(new { Message = "Feed created successfully.", FeedId = feed.FeedId });
            }
            catch (Exception ex)
            {
                // Handle errors
                return StatusCode(500, $"An error occurred while creating the feed: {ex.Message}");
            }
        }





        [HttpPut("{feedId}")]
        public async Task<IActionResult> UpdateFeed(string feedId, [FromBody] Feeds updatedFeed)
        {
            try
            {
                var feedResponse = await _feedsContainer.ReadItemAsync<Feeds>(feedId, new PartitionKey(feedId));
                var existingFeed = feedResponse.Resource;

                if (existingFeed == null)
                {
                    return NotFound(new { Message = "Feed not found." });
                }

                existingFeed.Likes = updatedFeed.Likes;
                existingFeed.Comments = updatedFeed.Comments;

                await _feedsContainer.ReplaceItemAsync(existingFeed, existingFeed.FeedId);

                return Ok(new { Message = "Feed updated successfully." });
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(new { Message = "Feed not found." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpDelete("{feedId}")]
        public async Task<IActionResult> DeleteFeed(string feedId)
        {
            try
            {
                await _feedsContainer.DeleteItemAsync<Feeds>(feedId, new PartitionKey(feedId));
                return Ok(new { Message = "Feed deleted successfully." });
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(new { Message = "Feed not found." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllFeeds()
        {
            try
            {
                var queryDefinition = new QueryDefinition("SELECT * FROM c ORDER BY c.currentTime DESC");
                var feedIterator = _feedsContainer.GetItemQueryIterator<Feeds>(queryDefinition);
                var orderedFeeds = new List<Feeds>();

                while (feedIterator.HasMoreResults)
                {
                    var feedResponse = await feedIterator.ReadNextAsync();
                    orderedFeeds.AddRange(feedResponse.ToList());
                }

                var orderedFeedsWithUrls = orderedFeeds.Select(feed => new { url = feed.Url, feedId = feed.FeedId });

                return Ok(new
                {
                    OrderedFeeds = orderedFeedsWithUrls
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }



    }
}
