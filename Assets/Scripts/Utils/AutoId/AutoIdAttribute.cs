using System;
using UnityEngine;

namespace Utils.AutoId
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoIdAttribute : PropertyAttribute
    {
    }
}