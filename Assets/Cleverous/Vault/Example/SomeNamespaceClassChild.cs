using UnityEngine;

namespace Oblivion
{
    public class SomeNamespaceClassChild : SomeNamespaceClass
    {
        [Header("Child")]
        public int ChildField;

        protected override void Reset()
        {
            base.Title = "Spaced Child";
            ChildField = 42;
        }
    }
}