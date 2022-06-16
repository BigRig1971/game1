using Lowscope.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lowscope.Saving.Examples
{
    public class RandomizeSprite : MonoBehaviour, ISaveable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] sprites;

        private bool hasBeenSaved;
        private int selectedIndex = -1;

        private void Start()
        {
            if (!SaveMaster.IsComponentLoaded(this))
            {
                int getRandomIndex = Random.Range(0, sprites.Length);
                spriteRenderer.sprite = sprites[getRandomIndex];
            }
            else
            {
                hasBeenSaved = true;
            }
        }

        public void OnLoad(string data)
        {
            selectedIndex = int.Parse(data);

            if (selectedIndex > 0 && selectedIndex < sprites.Length)
            {
                spriteRenderer.sprite = sprites[selectedIndex];
            }
        }

        public string OnSave()
        {
            return selectedIndex.ToString();
        }

        public bool OnSaveCondition()
        {
            return !hasBeenSaved;
        }
    }
}