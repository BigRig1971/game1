using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZInventory;

namespace EZInventory
{
    /// <summary>
    /// This allows the player to mine item blocks
    /// </summary>
    public class PlayerMining : MonoBehaviour
    {
        [Tooltip("UI Image that fills up to show mining progress")]
        public Image miningRing;

        static float power = 1; //How fast the player mines
        Transform camTransform;

        // Start is called before the first frame update
        void Start()
        {
            camTransform = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            bool showRing = false;

            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, 12))
                {
                    Destructable destructable = hit.transform.GetComponent<Destructable>();
                    if (destructable)
                    {
                        destructable.Damage(power * Time.deltaTime);
                        miningRing.fillAmount = 1 - (destructable.health / destructable.maxHealth);
                        showRing = true;
                    }
                }
            }

            if (!showRing && miningRing) miningRing.fillAmount = 0;
        }

        public static void SetPower(float newPower)
        {
            power = newPower;
        }
    }
}
