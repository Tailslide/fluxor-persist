using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fluxor.Persist.Middleware
{
    using System.Text.Json.Serialization;

    public record PersistMiddlewareState (InitializationStatus Status, ResetToDefaultStatus DefaultStatus, string ResetToDefaultError);

}
