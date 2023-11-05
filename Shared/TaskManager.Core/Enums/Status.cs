using System.Runtime.Serialization;

namespace TaskManager.Core.Enums;

[Flags]
public enum Status
{
    New = 1,
    Ready = 2,
    Active = 3,
    Review = 4,
    Closed = 5
}