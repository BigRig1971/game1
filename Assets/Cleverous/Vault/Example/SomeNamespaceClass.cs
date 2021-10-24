using Cleverous.VaultSystem;
using UnityEngine;

namespace Oblivion
{
    public class SomeNamespaceClass : DataEntity
    {
        [Header("Parent")]
        public int ParentField;

        protected override void Reset()
        {
            base.Title = "Spaced Parent";
            ParentField = 21;
        }
    }
}