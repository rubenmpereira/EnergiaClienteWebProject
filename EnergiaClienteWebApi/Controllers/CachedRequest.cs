using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

public class CachedRequest(IMemoryCache _cache) : ActionFilterAttribute
{
    private readonly IMemoryCache cache = _cache;
    private string Key { get; set; } = "";

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var request = filterContext.HttpContext.Request;
        var header = request.Headers.FirstOrDefault(h => h.Key.Equals("habitation"));
        int habitation = int.Parse(header.Value.ToString());

        var routeinfo = filterContext.ActionDescriptor.AttributeRouteInfo;
        if (routeinfo != null)
            Key = routeinfo.Name + "_" + habitation + filterContext.HttpContext.Request.QueryString;

        if (cache.TryGetValue(Key, out var cachedResult))
        {
            if (cachedResult != null)
            {
                filterContext.Result = (IActionResult)cachedResult;
            }
        }

        base.OnActionExecuting(filterContext);
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        if (string.IsNullOrEmpty(Key))
            base.OnActionExecuted(filterContext);

        var options = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromSeconds(60))
        .SetAbsoluteExpiration(TimeSpan.FromSeconds(600))
        .SetPriority(CacheItemPriority.Normal);

        cache.Set(Key, filterContext.Result, options);

        base.OnActionExecuted(filterContext);
    }
}