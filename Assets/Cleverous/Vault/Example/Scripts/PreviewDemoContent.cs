using Cleverous.VaultSystem;
using UnityEngine;

namespace VaultExample
{
    public class PreviewDemoContent : DataEntity
    {
        [Header("Content")]
        public Color TextColor;
        [SpritePreview]
        public Sprite Icon;
        public int Number;
        public string Verbiage;

        protected override void Reset()
        {
            base.Reset();
            TextColor = Color.cyan;
            Icon = null;
            Number = 42;
            Verbiage = "All your base are belong to us.";
        }

        public string GetMyCoolString()
        {
            return $"{Verbiage}\n... Secret: {Number}.\n...Squared Secret: {Number * Number}";
        }
    }
}