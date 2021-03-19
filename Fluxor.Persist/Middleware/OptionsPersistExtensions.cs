using Fluxor.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fluxor.Persist.Middleware
{
	public static class OptionsPersistExtensions
	{
		public static Options UsePersist(this Options options, Action<PersistMiddlewareOptions> persistMiddlewareOptions = null)
		{
			var middlewareOptions = new PersistMiddlewareOptions();
			persistMiddlewareOptions?.Invoke(middlewareOptions);
			options.AddMiddleware<PersistMiddleware>();
			options.Services.AddScoped(_ => middlewareOptions);
			return options;
		}
	}
}
