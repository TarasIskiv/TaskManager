using System.Runtime.Serialization;

namespace TaskManager.Core.Enums;

public enum Status
{
    [EnumMember(Value = "New")]
    New = 1,
    [EnumMember(Value = "Ready")]
    Ready,
    [EnumMember(Value = "Active")]
    Active,
    [EnumMember(Value = "Review")]
    Review,
    [EnumMember(Value = "Closed")]
    Closed
}