using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cameras;
using StupidHumanGames;



namespace StupidHumanGames
{
    public class ToggleCursor : MonoBehaviour
    {
		[SerializeField] string[] _elementsToDisable;
        bool state = false;
		private ThirdPersonController _tpc;
		private void Start()
		{
			_tpc = GetComponent<ThirdPersonController>();
		}
		/*private void Update()
		{
			if (_tpc._input.cursor)
			{
				_tpc._input.cursor = false;
				state = !state;
				if (state)
				{
					OnCursorVisable();
					
				}
				else
				{
					OnCursorHide();				
				}
			}
		}
		public void OnCursorVisable()
		{
			foreach(string element in _elementsToDisable)
			{
				(gameObject.GetComponent(element) as MonoBehaviour).enabled = false;
			}
			InventoryManager.OpenInventory();
			//Cursor.visible = true;
			//Cursor.lockState = CursorLockMode.None;
			//_tpc._input.cursorInputForLook = false;
			
		}
		public void OnCursorHide()
		{
			foreach (string element in _elementsToDisable)
			{
				(gameObject.GetComponent(element) as MonoBehaviour).enabled = true;
			}
			InventoryManager.CloseInventory();
			//Cursor.visible = false;
			//Cursor.lockState = CursorLockMode.Locked;
			//_tpc._input.cursorInputForLook = true;
		}*/
	}
}
