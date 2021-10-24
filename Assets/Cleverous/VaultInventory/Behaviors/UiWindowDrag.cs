// (c) Copyright Cleverous 2020. All rights reserved.

using UnityEngine;
using UnityEngine.EventSystems;

namespace Cleverous.VaultInventory
{
    public class UiWindowDrag : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [Header("|| Target Panel Pivot must be center")]
        [Header("|| Target Panel Anchor must be Bottom Left")]
        public RectTransform TargetPanel;

        public void OnBeginDrag(PointerEventData eventData)
        {
            TargetPanel.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 moveDelta = eventData.delta / VaultInventory.GameCanvas.scaleFactor;
            float xMin = VaultInventory.GameCanvas.pixelRect.xMin / VaultInventory.GameCanvas.scaleFactor;
            float xMax = VaultInventory.GameCanvas.pixelRect.xMax / VaultInventory.GameCanvas.scaleFactor;
            float yMin = VaultInventory.GameCanvas.pixelRect.yMin / VaultInventory.GameCanvas.scaleFactor;
            float yMax = VaultInventory.GameCanvas.pixelRect.yMax / VaultInventory.GameCanvas.scaleFactor;

            Vector2 result = new Vector2(
                Mathf.Clamp(
                    TargetPanel.anchoredPosition.x + moveDelta.x, 
                    xMin + TargetPanel.sizeDelta.x / 2, 
                    xMax - TargetPanel.sizeDelta.x / 2),
                Mathf.Clamp(
                    TargetPanel.anchoredPosition.y + moveDelta.y,
                    yMin + TargetPanel.sizeDelta.y / 2,
                    yMax - TargetPanel.sizeDelta.y / 2));

            TargetPanel.anchoredPosition = result;
        }
    }
}