// (c) Copyright Cleverous 2020. All rights reserved.

using UnityEngine;
using UnityEngine.UI;

namespace Cleverous.VaultInventory
{
    /// <summary>
    /// Used to communicate with the Merchant class.
    /// </summary>
    public class MerchantListItem : MonoBehaviour
    {
        public Image ItemIcon;
        public Image CurrencyIcon;

        public Text ItemName;
        public Text ItemPrice;
        public Button BuyButton;

        private RootItem m_referenceItem;
        private MerchantUi m_controller;

        public void Setup(Merchant merchant, MerchantUi controller, int index)
        {
            m_controller = controller;
            m_referenceItem = merchant.ItemsForSale[index];
            ItemIcon.sprite = m_referenceItem.UiIcon;
            ItemName.text = m_referenceItem.Title;
            const char cent = '\u00A2';
            ItemPrice.text = $"{cent}{m_referenceItem.Value}";
            CurrencyIcon.sprite = merchant.AcceptedCurrency.UiIcon;
            BuyButton.onClick.AddListener(() => Buy(index, 1));
        }

        public void Buy(int index, int count)
        {
            m_controller.ClientBuy(index, count);
        }
    }
}