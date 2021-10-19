using UnityEngine;
using UnityEngine.InputSystem;

namespace Cameras
{
    public class CustomCinemachineInputProvider : Cinemachine.CinemachineInputProvider
    {

        public InputActionReference toggleMouse;
        public override float GetAxisValue(int axis)
        {
            if (!toggleMouse)
			{
                return 0;             
            }             
            return base.GetAxisValue(axis);
        }
        
    }
}