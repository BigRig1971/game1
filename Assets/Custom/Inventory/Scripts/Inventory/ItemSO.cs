using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StupidHumanGames
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class ItemSO : ScriptableObject
    {
        [Tooltip("String that appears in tooltips.")]
        public string tooltip;
        [Tooltip("How many of this item can be placed in a single slot.")]
        public int stackLimit = 10;
        public Sprite itemSprite;
        public Type type;
        [Tooltip("Color of the item slot border.")]
        public Color itemBorderColor = new Color(1, 1, 1, 1);
        [Tooltip("If this is an equiipable item, this is what GameObject will spawn when held/equipped.")]
        [SerializeField]
        public GameObject equipPrefab;
        public GameObject buildPreview;
        public GameObject spawnPrefab;
        public ItemStatSO[] stat;

        
        

        public enum Type
        {
            All,
            Head,
            Torso,
            Legs,
            Gloves,
            HandRight,
            HandLeft,
            Feet,
            Buildable,       
        }
    }
}
