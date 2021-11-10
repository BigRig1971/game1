using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZInventory;

namespace EZInventory
{
    /// <summary>
    /// Spawns Tooltip canvas when mouse hovers over items
    /// </summary>
    public class Tooltip : MonoBehaviour
    {
        public Transform tooltipHolder;
        public Text tooltipText;

        public void SetTooltip(string text, Vector3 position)
        {
            tooltipHolder.position = position;
            tooltipText.text = text;
        }
    }
}