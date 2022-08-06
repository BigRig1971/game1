using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class QuitGame : MonoBehaviour
{
    [SerializeField] GameObject _quitGameUI;
    
    bool quit = false;
   public bool aboutToQuit = false;


    private void Start()
    {
        _quitGameUI = Instantiate(_quitGameUI);
        _quitGameUI.SetActive(false);
       
    }
    void Update()
    {
#if UNITY_EDITOR
   if(quit) EditorApplication.isPlaying = false;
#else
        if (quit) Application.Quit();
#endif
        if (Input.GetKey(KeyCode.Y))
        {
            
            StartCoroutine(OnQuit());

        }
        if (Input.GetKey("escape"))
        {

            if (!_quitGameUI.activeSelf)
            {
                _quitGameUI.SetActive(true);
            }


        }
        else
                if (Input.GetKey(KeyCode.N))
        {
            if (_quitGameUI.activeSelf) _quitGameUI.SetActive(false);          
        }
    }
    IEnumerator OnQuit()
    {
        aboutToQuit = true;
        yield return new WaitForSeconds(.3f);
        quit = true;
    }
}
