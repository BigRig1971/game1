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
       
        if (Input.GetKeyDown("escape"))
        {

            if (!_quitGameUI.activeSelf)
            {
                _quitGameUI.SetActive(true);
            }
            else
            {
                _quitGameUI.SetActive(false);
            }

        }
        if (Input.GetKeyDown(KeyCode.Y) && _quitGameUI.activeSelf)
        {

            StartCoroutine(OnQuit());

        }

        if (Input.GetKeyDown(KeyCode.N) && _quitGameUI.activeSelf)
        {
             _quitGameUI.SetActive(false);          
        }
    }
    IEnumerator OnQuit()
    {
        aboutToQuit = true;
        yield return new WaitForSeconds(.3f);
        quit = true;
    }
}
