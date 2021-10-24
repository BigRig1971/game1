using UnityEngine;

public class NoNamespaceClassChild : NoNamespaceClass
{
    [Header("Lower Class")]
    public int PotatoCount;

    protected override void Reset()
    {
        PotatoCount = 420000;
    }
}