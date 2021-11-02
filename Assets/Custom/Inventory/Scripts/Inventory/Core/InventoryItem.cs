using UnityEngine;
using UnityEngine.Events;

namespace InventorySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Inventory/InventoryItem")]
    public class InventoryItem : CompositeScriptableObject
    {
        public string Name;
        public Sprite Sprite;
        public InventoryItemType ItemType;	
	}
   
}
