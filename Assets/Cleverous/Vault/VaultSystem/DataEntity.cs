// (c) Copyright Cleverous 2020. All rights reserved.

using UnityEngine;

namespace Cleverous.VaultSystem
{
    public abstract class DataEntity : ScriptableObject
    {
        public string Title;
        [TextArea]
        public string Description;

        protected virtual void Reset()
        {
            Title = $"UNASSIGNED.{System.DateTime.Now.TimeOfDay.TotalMilliseconds}";
            Description = "";
        }
    }
}