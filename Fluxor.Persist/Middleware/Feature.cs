using Fluxor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fluxor.Persist.Middleware
{
	public class Feature : Feature<PersistMiddlewareState>
	{
		public override string GetName() => "PersistMiddleware";
		protected override PersistMiddlewareState GetInitialState() =>
			new PersistMiddlewareState( InitializationStatus.INITIALIZING, ResetToDefaultStatus.IDLE, "");
	}
}
