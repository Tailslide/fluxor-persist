using System;

namespace Fluxor.Persist.Storage
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SkipPersistState: Attribute
    {
        
    }
}
