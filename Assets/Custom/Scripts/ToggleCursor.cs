using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using Cameras;
using StarterAssets;

namespace StarterAssets
{
    public class ToggleCursor : MonoBehaviour
    {
        bool state = false;
		private ThirdPersonController _tpc;
		private void Start()
		{
			_tpc = GetComponent<ThirdPersonController>();
		}
		private void Update()
		{
			if (_tpc._input.cursor)
			{
				_tpc._input.cursor = false;
				state = !state;
				if (state)
				{					
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
					_tpc._input.cursorInputForLook = false;
				}
				else
				{
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;
					_tpc._input.cursorInputForLook = true;				
				}
			}
		}
	}
}
