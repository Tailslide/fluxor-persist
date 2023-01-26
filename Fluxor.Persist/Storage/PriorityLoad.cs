using System;

namespace Fluxor.Persist.Storage;

/// <summary>
/// Use this attribute to prioritize the loading of persistent features
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class PriorityLoad : Attribute
{
    /// <summary>
    /// The priority level of this state object (lower number gives greatest priority)
    /// </summary>
    public ushort Level { get; init; } = (ushort)PriorityLevel.Default;

    /// <summary>
    /// Sets the priority loading of this state object to default
    /// </summary>
    public PriorityLoad()
    {

    }

    /// <summary>
    /// Prioritize the loading of this state object
    /// </summary>
    /// <param name="level">Handy dandy priority levels</param>
    public PriorityLoad(PriorityLevel level)
    {
        Level = (ushort)level;
    }
}

public enum PriorityLevel
{
    Highest = 0,
    Higher = 1,
    High = 2,
    Default = 3,
    Low = 4,
    Lower = 5,
    Lowest = 6,
}