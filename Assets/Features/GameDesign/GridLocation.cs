using System;
using UnityEngine;

namespace Features.GameDesign
{
    [Serializable]
    public struct GridLocation
    {
        [Range(0, 3)] public int Row;
        [Range(0, 2)] public int Col;
    }
}