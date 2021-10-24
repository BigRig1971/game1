using Cleverous.VaultSystem;
using UnityEngine;

public class NoNamespaceClass : DataEntity
{
    [Header("Superior")]
    public int SuperiorData;

    protected override void Reset()
    {
        SuperiorData = int.MaxValue;
    }
}
