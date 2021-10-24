// (c) Copyright Cleverous 2020. All rights reserved.

using System.Collections.Generic;
using UnityEngine;

namespace Cleverous.VaultSystem
{
    [CreateAssetMenu(fileName = "New Database", menuName = "Cleverous/Vault/Database", order = 0)]
    public class Database : ScriptableObject
    {
        public List<DataEntity> Items;
    }
}