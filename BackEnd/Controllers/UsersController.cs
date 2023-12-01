using Backend.Entities;
using BackEnd.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Define arrays for adjectives and nouns
        private readonly string[] Adjectives = { "Adventurous", "Bright", "Charming", /* Add more adjectives as needed */ };
        private readonly string[] Nouns = { "Aurora", "Beach", "Bear", /* Add more nouns as needed */ };

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("generate-username")]
        public async Task<IActionResult> GenerateUsername()
        {
            string username = GenerateRandomUsername();
            _context.Users.Add(new User { UserName = username });

            try { await _context.SaveChangesAsync(); }
            catch(Exception ex) {};

            

            return Ok(new { Username = username });
        }

        private string GenerateRandomUsername()
        {
            string adjective = GetRandomArrayValue(Adjectives);
            string noun = GetRandomArrayValue(Nouns);
            string number = GenerateRandomNumber();

            return $"{adjective}_{noun}_{number}";
        }

        private string GetRandomArrayValue(string[] array)
        {
            if (array == null || array.Length == 0)
            {
                // Handle the case where the array is empty
                return "DefaultWord";
            }

            Random random = new Random();
            int randomIndex = random.Next(0, array.Length);

            return array[randomIndex];
        }

        private string GenerateRandomNumber()
        {
            // Generate a random 3-digit number
            Random random = new Random();
            return random.Next(100, 1000).ToString();
        }
    }
}
