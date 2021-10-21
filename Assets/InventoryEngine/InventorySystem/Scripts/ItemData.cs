using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int slotID;
    public int amount;
    public Item item;

    private Vector2 offset;
    private Transform originalParent;
    private string originalParentName = "Canvas";
    private Text amountText;

    private InventoryManager manager;

    void Awake() {
        manager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>();
        if(manager == null) Debug.LogError("[InventorySystem Error]: Can't find InventorySystem");
    }

    void Start()
    {
        amountText = transform.GetChild(0).GetComponent<Text>();
    }

    void Update () {
        if(amountText.text != amount.ToString()){
            amountText.text = amount.ToString();//updating amount of items text
        }
    }

    //allow to Drag item
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            originalParent = GameObject.Find(originalParentName).transform; 

            if(originalParent != null){
                this.transform.SetParent(originalParent);
                this.transform.position = eventData.position;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
            }else {
                Debug.LogError("[InventorySystem Error]: Can't find GameObject with the name \"" + originalParentName + "\"");
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            this.transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(slotID != -1){
            //set new item's slot
            Transform newSlot = manager.GetSlotByID(slotID).transform;

            this.transform.SetParent(newSlot);
            this.transform.position = newSlot.position;

            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }else {
            Destroy(gameObject);//destroy item if it has slotID == -1
        }
    }

    //show tip only in inventory or Equip slots
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(manager.TipPanel != null){
            manager.TipPanel.GetComponent<InventoryItemTip>().Activate(item);
        }
    }

    //deactivate tip panel
    public void OnPointerExit(PointerEventData eventData)
    {
        if(manager.TipPanel != null){
            manager.TipPanel.GetComponent<InventoryItemTip>().Deactivate();
        }
    }

    //activate split items panel
    public void OnPointerClick(PointerEventData eventData)
    {
        if(manager.SplitPanel != null){
            manager.SplitPanel.GetComponent<InventoryItemSplit>().Activate(this);
        }
    }

    //deactivate split items panel
    void OnDisable()
    {
        if(manager.TipPanel != null){
            manager.TipPanel.GetComponent<InventoryItemTip>().Deactivate();
        }
    }
}
