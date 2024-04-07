namespace CodeDesign.Web.Handlers
{
    public class ExceptionHandlingMiddleware
    {
        private RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Exeption occurred {Message}", ex.Message);
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
        }
    }
}
