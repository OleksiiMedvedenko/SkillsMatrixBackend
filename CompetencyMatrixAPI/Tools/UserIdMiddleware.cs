namespace CompetencyMatrixAPI.Tools
{
    public class UserIdMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var userId = GetUserIdFromRequest(context.Request);

            context.Items["userId"] = userId;

            await _next(context);
        }

        private int GetUserIdFromRequest(HttpRequest request)
        {
            if (request.Query.TryGetValue("userId", out var userIdFromRoute))
            {
                if (int.TryParse(userIdFromRoute, out var userId)) 
                {
                    return userId;
                }
            }
            return 0; 
        }
    }
}
