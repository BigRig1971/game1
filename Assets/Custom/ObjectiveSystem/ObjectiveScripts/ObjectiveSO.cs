using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StupidHumanGames
{
    [CreateAssetMenu(fileName = "New Objective", menuName = "ObjectiveSo/New Objective")]
    public class ObjectiveSO : ScriptableObject
    {
        public string _objectiveName;
        public bool _isActive = false;
        [TextArea(5,10)] public string _objectiveDetail;
        public enum ObjectiveType
        {
            gather, kill
        }
        public ObjectiveType objectiveType;
        [System.Serializable]
        public class Item
        {
            public ItemSO _item;
            public int _itemAmount = 1;
        }
        public Item[] _listOfItems;
        

    }

}
