using System.Diagnostics;
using UnityEngine;

namespace Features.Shared
{
    [Conditional("UNITY_EDITOR")]
    public class NotEditableAttribute : PropertyAttribute
    {
    }
}