using System;
using UnityEngine;

namespace Digger
{
    public struct ModificationParameters
    {
        public Vector3 Position;
        public BrushType Brush;
        public ActionType Action;
        public int TextureIndex;
        public float Opacity;
        public float Size;
        public bool RemoveDetails;
        public bool RemoveTreesInSphere;
        public float StalagmiteHeight;
        public bool StalagmiteUpsideDown;
        public bool OpacityIsTarget;
        public Action Callback;

        public static ModificationParameters Default => new ModificationParameters
        {
            Brush = BrushType.Sphere,
            Position = default,
            Action = ActionType.Dig,
            TextureIndex = 0,
            Opacity = 0.5f,
            Size = 4f,
            RemoveDetails = true,
            RemoveTreesInSphere = true,
            StalagmiteHeight = 8f,
            StalagmiteUpsideDown = false,
            OpacityIsTarget = false,
            Callback = null
        };
    }
}