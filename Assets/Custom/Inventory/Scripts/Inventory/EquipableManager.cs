using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StupidHumanGames
{
    public class EquipableManager : MonoBehaviour
    {
        public UnityEvent weaponStartDamage;
        public UnityEvent weaponEndDamage;
        public UnityEvent onWeaponSwing;
        [SerializeField]
        private GameObject equipableItemHolder;

        public List<GameObject> EquipableItems = new List<GameObject>();
        private Transform[] equipableItems;
        [SerializeField] private ScriptableObject[] itemSoList;
        private void Awake()
        {
            itemSoList = Resources.LoadAll<ScriptableObject>("EquipableItems");
            equipableItems = equipableItemHolder.GetComponentsInChildren<Transform>();
            foreach (Transform child in equipableItems)
            {
                foreach (ScriptableObject iso in itemSoList)
                {
                    if (child.name == iso.name)
                    {
                        if (!EquipableItems.Contains(child.gameObject))
                            EquipableItems.Add(child.gameObject);
                    }
                }
            }
        }
        private void Start()
        {
            if (weaponStartDamage == null)
                weaponStartDamage = new UnityEvent();
            if (weaponEndDamage == null)
                weaponEndDamage = new UnityEvent();

            if (onWeaponSwing == null)
                onWeaponSwing = new UnityEvent();
            DisableEquipped();
        }
        void DisableEquipped()
        {
            foreach (GameObject go in EquipableItems)
            {
                go.SetActive(false);

            }
        }
        public void OnWeaponStartDamage()
        {
            weaponStartDamage?.Invoke();
        }
        public void OnWeaponEndDamage()
        {
            weaponEndDamage?.Invoke();
        }
        public void ONWeaponSwing()
        {
            onWeaponSwing?.Invoke();
        }
    }
}
