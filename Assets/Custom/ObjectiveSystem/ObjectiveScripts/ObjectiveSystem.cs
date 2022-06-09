using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StupidHumanGames
{

    public class ObjectiveSystem : MonoBehaviour
    {

        bool toggleCanvas = false;

        public GameObject _objectiveButton;
        public GameObject _objectiveDetail;
        public Canvas _objectiveCanvas;

        public Transform _objectivesContainer;
        public ObjectiveSO[] _objectives;
        public TMP_Text _detailText;
        // Start is called before the first frame update
        void Start()
        {
            if (_objectiveCanvas != null) _objectiveCanvas.enabled = false;
            OnSpawnObjectiveButtons();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                toggleCanvas = !toggleCanvas;
                if (toggleCanvas)
                {
                    _objectiveCanvas.enabled = true;
                    InventoryManager.OpenInventory();
                }
                else
                {
                    _objectiveCanvas.enabled = false;
                    InventoryManager.CloseInventory();
                }

            }
        }
        void OnSpawnObjectiveButtons()
        {
            foreach (var o in _objectives)
            {
                var _go = Instantiate(_objectiveButton, _objectivesContainer);

                _go.GetComponent<MouseOverDetect>()._objectiveName = o._objectiveName;
                _go.GetComponent<MouseOverDetect>()._objectiveDetail = o._objectiveDetail;


            }
        }
    }
}

