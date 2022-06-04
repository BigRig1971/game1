using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StupidHumanGames
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 _move;
		public Vector2 _look;
		public bool _jump;
		public bool _sprint;
		public bool _roll;
		public bool _interact;
		public bool _inventory;
		public bool _cursor;
		public bool _attack;
		public bool _build;
		public bool _pickup;
		


		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
		public void OnRoll(InputValue value)
        {
			RollInput(value.isPressed);
        }
		public void OnInteract(InputValue value)
        {
			Interact(value.isPressed);
        }
		public void OnInventory(InputValue value)
        {
			Inventory(value.isPressed);
        }
		public void OnCursor(InputValue value)
        {
			Kursor(value.isPressed);
        }
		public void OnBuild(InputValue value)
		{
			Build(value.isPressed);
		}
		public void OnAttack(InputValue value)
		{
			Attack(value.isPressed);
		}
		public void OnPickup(InputValue value)
		{
			Pickup(value.isPressed);
		}
		
#endif


		public void MoveInput(Vector2 value)
		{
			_move = value;
		} 

		public void LookInput(Vector2 value)
		{
			_look = value;
		}

		public void JumpInput(bool value)
		{
			_jump = value;
		}

		public void SprintInput(bool value)
		{
			_sprint = value;
		}
		public void RollInput(bool value)
        {
			_roll = value;
        }
		public void Interact(bool value)
        {
			_interact = value;
        }
		public void Inventory(bool value)
        {
			_inventory = value;
        }
		public void Kursor(bool value)
        {
			_cursor = value;
        }
		public void Build(bool value)
		{
			_build = value;
		}
		public void Attack(bool value)
		{
			_attack = value;
		}
		public void Pickup(bool value)
		{
			_pickup = value;
		}
		


		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}