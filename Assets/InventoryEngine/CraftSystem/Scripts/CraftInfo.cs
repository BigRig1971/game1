using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftInfo : MonoBehaviour
{
    CraftManager manager;

    Button craftButton;
    Text craftText;

    void Awake()
    {
        manager = GameObject.Find("CraftSystem").GetComponent<CraftManager>();
        if(manager != null) manager.CraftInfo = gameObject;
        else Debug.LogError("[CraftSystem Error]: Can't find CraftSystem");

        if(!transform.Find("CraftButton")){
            Debug.LogError("[CraftSystem Error]: Can't find CraftButton");
        }else if(!transform.Find("Text")){
            Debug.LogError("[CraftSystem Error]: Can't find Text in CraftInfo");
        }else {
            craftButton = transform.Find("CraftButton").GetComponent<Button>();
            craftText = transform.Find("Text").GetComponent<Text>();

            craftButton.onClick.AddListener(delegate { manager.CraftItem(); });
            SetInfoText();
        }
    }

    //Set text in craft info panel
    public void SetInfoText(string text = "Press Item") {
        craftText.text = text;
    }

    //Activate craft panel
    public void Activate(string text = ""){
        SetInfoText(text);
        craftButton.interactable = true;        
    }

    //Deactivate craft panel
    public void Deactivate(){
        SetInfoText();
        craftButton.interactable = false;
    }

}
