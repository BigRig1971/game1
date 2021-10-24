// (c) Copyright Cleverous 2020. All rights reserved.

using Cleverous.VaultSystem;
using UnityEngine;
using UnityEngine.UI;

namespace VaultExample
{
    /// <summary>
    /// This class will populate the target UI elements with the reference data.
    /// The dropdown for the Demo Content data references is from a special attribute that lets you choose items from the Vault Database.
    /// </summary>
    public class VaultUiPreview : MonoBehaviour
    {
        [Header("References")]
        public Image ImgObj;
        public Image HeaderBackground;
        public Text HeaderText;
        public Text DescriptionText;
        public Text MedialText;
        public Text TimeText;
        public float SwapTime;

        [Header("Array of Vault Datapoints")]
        [AssetDropdown(typeof(PreviewDemoContent))]
        public PreviewDemoContent[] ContentArray;

        private float m_timer;
        private int m_index;

        private void Update()
        {
            m_timer -= Time.deltaTime;
            TimeText.text = $"{m_timer:F1}";
            if (m_timer > 0) return;




            // Here's where we populate the UI with data:
            // Also note: You could inherit from that PreviewDemoContent class to easily add to, override or extend it!
            HeaderBackground.color  = ContentArray[m_index].TextColor;
            HeaderText.text         = ContentArray[m_index].Title;
            DescriptionText.text    = ContentArray[m_index].Description;
            MedialText.text         = ContentArray[m_index].GetMyCoolString(); // Notice! This is a method inside a data entity. Totally fine.
            ImgObj.sprite           = ContentArray[m_index].Icon;





            m_timer = SwapTime;
            m_index++;

            if (m_index >= ContentArray.Length) m_index = 0;
        }
    }
}