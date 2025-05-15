using StackExchange.Redis;

namespace AuthService
{
    public class AuthService
    {
        private readonly IDatabase _redisDb;

        // Constructor to inject Redis connection
        public AuthService()
        {
            var redis = ConnectionMultiplexer.Connect("http://localhost:6379/"); // Redis server address
            _redisDb = redis.GetDatabase();
        }

        // Method to store JWT token in Redis (Centralized session store)
        public void StoreJwtTokenInRedis(string userId, string jwtToken)
        {
            // Storing JWT with the userId as key and JWT token as value in Redis
            _redisDb.StringSet(userId, jwtToken, TimeSpan.FromHours(1));  // Expiry time of 1 hour
        }

        // Method to get JWT token from Redis
        public string GetJwtTokenFromRedis(string userId)
        {
            return _redisDb.StringGet(userId);  // Returns JWT token from Redis
        }
    }
}
