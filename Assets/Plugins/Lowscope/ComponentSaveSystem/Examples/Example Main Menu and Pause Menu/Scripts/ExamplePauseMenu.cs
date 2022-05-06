using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lowscope.Saving.Examples
{
    public class ExamplePauseMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private GraphicRaycaster graphicRaycaster;
        [SerializeField] private GameObject pauseMenuObjects;
        [SerializeField] private ExampleSlotMenu slotMenu;
        [SerializeField] private Transform darkenBackground;

        [SerializeField] private Button buttonContinue;
        [SerializeField] private Button buttonLoad;
        [SerializeField] private Button buttonSave;
        [SerializeField] private Button buttonQuit;

        [Header("Configuration")]
        [SerializeField] private KeyCode[] openMenuKeys = new KeyCode[1] { KeyCode.Escape };
        [SerializeField] private string quitToScene;
        [SerializeField] private bool closeWindowOnSave = true;
        [SerializeField] private bool closeWindowOnLoad = true;
        [SerializeField] private GameObject[] toggleObjectVisibility;
        [SerializeField] private bool pauseAndUnpauseGame = false;

        private Dictionary<GameObject, bool> cachedVisibility = new Dictionary<GameObject, bool>();

        private int openMenuKeyCount;
        
        // Mainly to update references of older prefab version.
#if UNITY_EDITOR
        [HideInInspector]
        public int updatedVersion;

        private void OnValidate()
        {
            if (updatedVersion < 1 && buttonContinue == null)
            {
                Button[] getButtons = GetComponentsInChildren<Button>();
                int buttonCount = getButtons.Length;
                for (int i = 0; i < buttonCount; i++)
                {
                    if (getButtons[i].name == "Button - Continue")
                    {
                        buttonContinue = getButtons[i];
                        return;
                    }
                }

                updatedVersion = 1;
            }
        }
#endif

        private void Awake()
        {
            openMenuKeyCount = openMenuKeys.Length;

            buttonContinue.onClick.AddListener(OnPressContinue);
            buttonLoad.onClick.AddListener(OnOpenSlotMenuLoad);
            buttonSave.onClick.AddListener(OnOpenSlotMenuSave);
            buttonQuit.onClick.AddListener(OnQuit);

            if (closeWindowOnSave)
            {
                SaveMaster.OnWritingToDiskDone += OnWrittenToDisk;
            }

            if (closeWindowOnLoad)
            {
                SaveMaster.OnSlotChangeDone += OnChangedSlot;
            }

            int toggleVisibilityCount = toggleObjectVisibility.Length;
            for (int i = 0; i < toggleVisibilityCount; i++)
            {
                cachedVisibility.Add(toggleObjectVisibility[i], toggleObjectVisibility[i].activeSelf);
            }
        }

        private void OnDestroy()
        {
            if (pauseAndUnpauseGame)
            {
                Time.timeScale = 1;
            }
            
            if (closeWindowOnSave)
            {
                SaveMaster.OnWritingToDiskDone -= OnWrittenToDisk;
            }

            if (closeWindowOnLoad)
            {
                SaveMaster.OnSlotChangeDone -= OnChangedSlot;
            }
        }
        
        private void OnPressContinue()
        {
            Hide();
        }

        private void OnChangedSlot(int arg1, int arg2)
        {
            Hide();
        }

        private void OnWrittenToDisk(int obj)
        {
            Hide();
        }

        private void OnEnable()
        {
            Hide();
        }

        private void OnQuit()
        {
            SceneManager.LoadScene(quitToScene);
        }

        private void OnOpenSlotMenuSave()
        {
            slotMenu.SetMode(ExampleSlotMenu.Mode.Save);
            slotMenu.gameObject.SetActive(true);
        }

        private void OnOpenSlotMenuLoad()
        {
            slotMenu.SetMode(ExampleSlotMenu.Mode.Load);
            slotMenu.gameObject.SetActive(true);
        }

        private void ToggleDisplay(bool display)
        {
            slotMenu.gameObject.SetActive(false);
            pauseMenuObjects.gameObject.SetActive(display);

            if (graphicRaycaster != null)
                graphicRaycaster.enabled = display;

            if (canvas != null)
                canvas.enabled = display;

            if (darkenBackground != null)
                darkenBackground.gameObject.SetActive(display);

            int toggleVisibilityCount = toggleObjectVisibility.Length;
            for (int i = 0; i < toggleVisibilityCount; i++)
            {
                bool visible;
                if (cachedVisibility.TryGetValue(toggleObjectVisibility[i], out visible))
                {
                    toggleObjectVisibility[i].gameObject.SetActive(display? visible : !visible);
                }
            }

            if (pauseAndUnpauseGame)
            {
                Time.timeScale = display? 0 : 1;
            }
        }

        public void Display()
        {
            ToggleDisplay(true);
        }

        public void Hide()
        {
            ToggleDisplay(false);
        }

        private void Update()
        {
            for (int i = 0; i < openMenuKeyCount; i++)
            {
                if (Input.GetKeyDown(openMenuKeys[i]))
                {
                    if (!pauseMenuObjects.activeSelf)
                    {
                        Display();
                    }
                    else
                    {
                        Hide();
                    }
                    return;
                }
            }
        }
    }
}