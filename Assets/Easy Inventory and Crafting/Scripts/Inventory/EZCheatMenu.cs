using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZInventory;

namespace EZInventory
{
    /// <summary>
    /// Cheater, cheater, pumpkin eater
    /// </summary>
    public class EZCheatMenu : MonoBehaviour
    {
        public GameObject mainGameObject;
        public ItemSO[] items;
        public Dropdown itemsDropdown;
        public Text warningText;
        int currentItem;
        int currentItemAmount = 99;

        // Start is called before the first frame update
        void Start()
        {
            mainGameObject.SetActive(true);
            itemsDropdown.ClearOptions();
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            foreach (ItemSO item in items)
            {
                options.Add(new Dropdown.OptionData(item.name));
            }
            itemsDropdown.AddOptions(options);

            DisableText();
            mainGameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                mainGameObject.SetActive(!mainGameObject.activeSelf);
            }
        }

        public void SetItem(int value)
        {
            currentItem = value;
        }

        public void SetAmount(string value)
        {
            try { currentItemAmount = int.Parse(value); }
            catch { warningText.text = "Invalid Amount!"; warningText.gameObject.SetActive(true); Invoke("DisableText", 2); }
        }

        void DisableText()
        {
            warningText.gameObject.SetActive(false);
        }

        public void GenerateItem()
        {
            if (InventoryManager.AddItemToInventory(items[currentItem], currentItemAmount) > 0)
            {
                warningText.text = "Ran out of room!";
                warningText.gameObject.SetActive(true);
                Invoke("DisableText", 2);
            }

        }
    }
}
