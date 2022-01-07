using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class QuitGame : MonoBehaviour
{
    private TextMeshProUGUI Quit;
	private void Start()
	{
        Quit = FindObjectOfType<TextMeshProUGUI>();
        Quit.enabled = false;

	}
	void Update()
    {
        if (Input.GetKey(KeyCode.Y) && Quit.enabled)
        {
            Application.Quit();         
        }
        if (Input.GetKey("escape"))
        {
            Quit.enabled = true;     
        }
        else
                if (Input.GetKey(KeyCode.N))
        {
            Quit.enabled = false;
        }
    }
}
