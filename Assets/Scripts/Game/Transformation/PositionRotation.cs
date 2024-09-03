using System;
using UnityEngine;

namespace Game.Utils
{
    [Serializable]
    public struct PositionRotation
    {
        public Vector3 Position => position;
        public Vector3 Rotation => rotation;
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 rotation;
    }

}
