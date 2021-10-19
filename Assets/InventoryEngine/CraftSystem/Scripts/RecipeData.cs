using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecipeData : MonoBehaviour, IPointerClickHandler
{
    public Recipe recipe;//data of recipe

    private CraftManager manager;

    void Awake() {
        manager = GameObject.Find("CraftSystem").GetComponent<CraftManager>();
        if(manager == null) Debug.LogError("[CraftSystem Error]: Can't find CraftSystem");
    }

    //Set recipe info in CraftInfo panel when we click on recipe
    public void OnPointerClick(PointerEventData eventData)
    {
        manager.SetCraftInfo(recipe);
    }
}
