using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace BankWebApp.Infrastructure
{
    // Logs all AdminController actions (CRUD) with timing and sanitized args
    public sealed class AdminCrudLoggingFilter : IAsyncActionFilter
    {
        private readonly ILogger<AdminCrudLoggingFilter> _logger;
        public AdminCrudLoggingFilter(ILogger<AdminCrudLoggingFilter> logger) => _logger = logger;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var http = context.HttpContext;
            var method = http.Request.Method;          // GET/POST/PUT/DELETE
            var path = http.Request.Path.Value ?? string.Empty;
            var action = context.ActionDescriptor.DisplayName ?? string.Empty;

            // Build a sanitized args object (avoid logging passwords)
            var args = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            foreach (var kv in context.ActionArguments)
            {
                if (kv.Key.Contains("password", StringComparison.OrdinalIgnoreCase))
                {
                    args[kv.Key] = "***"; // redact
                }
                else
                {
                    // For complex types, avoid dumping the whole object; log type name
                    var val = kv.Value;
                    if (val is null || val.GetType().IsPrimitive || val is string or decimal)
                        args[kv.Key] = val;
                    else
                        args[kv.Key] = val.GetType().Name;
                }
            }

            var sw = Stopwatch.StartNew();
            _logger.LogInformation("ADMIN START {Method} {Path} | Action={Action} | Args={@Args}", method, path, action, args);

            try
            {
                var executed = await next();
                sw.Stop();
                var status = executed.HttpContext.Response.StatusCode;
                _logger.LogInformation("ADMIN END {Method} {Path} -> {Status} in {ElapsedMs}ms | Action={Action}", method, path, status, sw.ElapsedMilliseconds, action);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "ADMIN ERROR {Method} {Path} after {ElapsedMs}ms | Action={Action}", method, path, sw.ElapsedMilliseconds, action);
                throw;
            }
        }
    }
}
