using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fluxor.Persist.Sample.Store.CounterUseCase
{
	public record CounterState( int ClickCount);
}
