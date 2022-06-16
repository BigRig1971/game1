using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace StupidHumanGames
{

    public class MouseOverDetect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string _objectiveName;
        public string _objectiveDetail;

        public void OnPointerDown(PointerEventData eventData) 
        { 
            Debug.Log("Mouse Down: " + eventData.pointerCurrentRaycast.gameObject.name);
        }


        public void OnPointerEnter(PointerEventData pointerEventData)
        {

            //var go = pointerEventData.pointerClick.GetComponent<ObjectiveSO>();
            var go = GameObject.FindGameObjectWithTag("ObjectiveDetail");
            go.GetComponentInChildren<TMP_Text>().text = _objectiveDetail;

        }
        public void OnPointerExit(PointerEventData pointerEventData)
        {

        }
        private void Start()
        {
            var on = GetComponentInChildren<TMP_Text>();
            on.text = _objectiveName;
        }
    }

}
