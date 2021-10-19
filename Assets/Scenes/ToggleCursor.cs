using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using Cameras;
public class ToggleCursor : MonoBehaviour
{
    bool state = false;
   
    public InputAction input;
    public CustomCinemachineInputProvider cinInput;

    private void Start()
	{
       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        
    }
	void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
		{
            state = !state;
            if (!state)
			{
                Cursor.lockState = CursorLockMode.None;            
                Cursor.visible = false;
                
                      
            }
			else
			{
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                
            }
		}
            
    }

}
