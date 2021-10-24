// (c) Copyright Cleverous 2020. All rights reserved.

using System.Collections.Generic;
using UnityEngine;

namespace Cleverous.VaultInventory
{
    /// <summary>
    /// Merchant UI controller. Requires being setup with a Merchant and Player reference. 
    /// Will manage actions between shop ui components and the merchant.
    /// </summary>
    public class MerchantUi : MonoBehaviour
    {
        public static MerchantUi Instance;
        [Header("References")]
        public RectTransform ContentContainer;
        public GameObject ListItemPrefab;

        private Merchant m_merchant;
        private List<GameObject> m_uiItems;

        public void Awake()
        {
            m_uiItems = new List<GameObject>();
            Instance = this;
            gameObject.SetActive(false);
        }

        public virtual void Open(Merchant merchant)
        {
            m_merchant = merchant;
            gameObject.SetActive(true);

            for (int i = 0; i < merchant.ItemsForSale.Length; i++)
            {
                GameObject go = Instantiate(ListItemPrefab, ContentContainer);
                m_uiItems.Add(go);

                MerchantListItem row = go.GetComponent<MerchantListItem>();
                row.Setup(m_merchant, this, i);
            }
        }

        public virtual void Close()
        {
            m_merchant = null;
            foreach (GameObject x in m_uiItems)
            {
                Destroy(x);
            }
            gameObject.SetActive(false);
        }

        public virtual void ClientBuy(int index, int count)
        {
            if (m_merchant == null) return;
            m_merchant.ClientBuy(index, count);
        }

        public virtual void ClientSell(int index)
        {
            m_merchant.ClientSell(index);
        }
    }
}