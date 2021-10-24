﻿// (c) Copyright Cleverous 2020. All rights reserved.

using UnityEngine;
using UnityEngine.UI;

namespace Cleverous.VaultInventory
{
    /// <summary>
    /// The Tooltip will hook into Inventory UI to display information on Items in the Inventory.
    /// </summary>
    public class UiTooltip : MonoBehaviour
    {
        public static UiTooltip Instance;
        public Text Title;
        public Text Description;

        // TODO force tooltip window to always be on screen, using edges as bumpers

        private RectTransform m_tooltipRect;

        protected virtual void Awake()
        {
            m_tooltipRect = GetComponent<RectTransform>();
            Instance = this;
            Hide();
        }

        public virtual void Show(RectTransform rt, string title, string description)
        {
            if (rt == null) return;

            Title.text = title;
            Description.text = description;
            transform.position = rt.position - new Vector3
            {
                x = rt.rect.width / 2,
                y = rt.rect.height / 2
            };

            gameObject.SetActive(true);
        }

        public virtual void Show(ItemUiPlug plug)
        {
            m_tooltipRect.SetAsLastSibling();
            if (plug == null || plug.ReferenceVaultItemData == null) return;
            RectTransform rt = plug.GetComponent<RectTransform>();
            Show(rt, plug.ReferenceVaultItemData.GetUiTitle(), plug.ReferenceVaultItemData.GetDescriptionComplex());
        }

        public virtual void Hide()
        {
            Title.text = "";
            Description.text = "";
            gameObject.SetActive(false);
        }
    }
}