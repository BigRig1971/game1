using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemTip : MonoBehaviour
{
    InventoryManager manager;

    void Awake()
    {
        manager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>(); 
        if(manager != null) manager.TipPanel = gameObject;
        else Debug.LogError("[InventorySystem Error]: Can't find InventorySystem");
    }

    // Start is called before the first frame update
    void Start()
    {
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf){
            gameObject.transform.position = Input.mousePosition;
        }
    }

    //activate tip
    public void Activate(Item item)
    {
        //text in Tip FF5C34
        string data = "<color=#000000><b>" + item.Name + "</b></color>" 
        + "\n<color=#FF3300>" + "Power: " + item.Power.ToString() + "</color>"
        + "\n<color=#3737FF>" + "Price: " + item.Price.ToString() + "</color>";

        gameObject.transform.GetChild(0).GetComponent<Text>().text = data;
        gameObject.transform.position = Input.mousePosition;

        gameObject.SetActive(true);
    }

    //deactivate tip
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
