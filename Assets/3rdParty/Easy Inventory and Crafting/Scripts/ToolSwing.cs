using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

namespace EZInventory
{
    /// <summary>
    /// Spin while mouse button is held down
    /// </summary>
    public class ToolSwing : MonoBehaviour
    {
        public float swingSpeed = 800;
        public float power = 1;

        private void OnEnable()
        {
            PlayerMining.SetPower(power);
        }

        private void OnDisable()
        {
            PlayerMining.SetPower(1);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                transform.Rotate(swingSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
}
