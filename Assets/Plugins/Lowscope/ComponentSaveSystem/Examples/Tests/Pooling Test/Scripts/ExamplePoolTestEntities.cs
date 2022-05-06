using Lowscope.Saving.Components;
using Lowscope.Saving.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lowscope.Saving.Examples
{
    public class ExamplePoolTestEntities : MonoBehaviour
    {
        [SerializeField] private Text poolCounter;

        [SerializeField] private GameObject bulletPrefab;

        [SerializeField] private Button buttonSpawnBullet;
        [SerializeField] private Button buttonSyncSave;
        [SerializeField] private Button buttonSyncLoad;
        [SerializeField] private Button buttonWriteToDisk;
        [SerializeField] private Button buttonLoadFromDiskAndSync;
        [SerializeField] private Button buttonReloadScene;

        private List<GameObject> activePoolObjects = new List<GameObject>();
        private List<GameObject> inactivePoolObjects = new List<GameObject>();

        private int currentSpawnedObjects = 0;
        private const int initialPoolSize = 10;

        private void Awake()
        {
            // Add a custom resource location for the spawned prefab.
            // The removes the need for it to be in the resources folder.
            SaveMaster.AddPrefabResourceLocation("ExamplePoolTestEntitiesResource", (id) =>
            {
                if (id == "bullet")
                {
                    return bulletPrefab;
                }
                else
                {
                    return null;
                }
            });

            SaveMaster.OnSpawnedSavedInstance += OnSpawnedSavedInstance;

            buttonSpawnBullet.onClick.AddListener(OnSpawnButton);
            buttonSyncSave.onClick.AddListener(OnSyncSave);
            buttonSyncLoad.onClick.AddListener(OnSyncLoad);
            buttonWriteToDisk.onClick.AddListener(OnWriteToDisk);
            buttonLoadFromDiskAndSync.onClick.AddListener(OnLoadFromDiskAndSync);
            buttonReloadScene.onClick.AddListener(OnReloadScene);
        }

        private void OnDestroy()
        {
            SaveMaster.OnSpawnedSavedInstance -= OnSpawnedSavedInstance;

            buttonSpawnBullet.onClick.RemoveListener(OnSpawnButton);
            buttonSyncSave.onClick.RemoveListener(OnSyncSave);
            buttonSyncLoad.onClick.RemoveListener(OnSyncLoad);
            buttonWriteToDisk.onClick.RemoveListener(OnWriteToDisk);
            buttonLoadFromDiskAndSync.onClick.RemoveListener(OnLoadFromDiskAndSync);
            buttonReloadScene.onClick.RemoveListener(OnReloadScene);
        }

        private void Start()
        {
            int fillPool = initialPoolSize - currentSpawnedObjects;
            for (int i = 0; i < fillPool; i++)
            {
                var spawnedPrefab = SpawnPrefab();
                spawnedPrefab.gameObject.SetActive(false);
                currentSpawnedObjects++;
            }
        }

        private GameObject SpawnPrefab()
        {
            var spawnedPrefab = SaveMaster.SpawnSavedPrefab(
                InstanceSource.Custom,
                "bullet",
                "ExamplePoolTestEntitiesResource",
                userTag: "examplepool");

            AssignEvents(spawnedPrefab);

            // Remove existing data before displaying it again.
            // Since it might have been saved before. Meaning it loads older data.
            SaveMaster.WipeSaveable(spawnedPrefab.GetComponent<Saveable>(), false);

            return spawnedPrefab;
        }

        private void AssignEvents(GameObject spawnedPrefab)
        {
            var examplePoolBall = spawnedPrefab.gameObject.GetComponent<ExamplePoolBall>();

            // Just sample code of listening to OnDisabled, doing it this way is not best practice.
            examplePoolBall.OnDisabled.AddListener(() =>
            {
                activePoolObjects.Remove(spawnedPrefab);

                if (spawnedPrefab == null)
                    return;

                inactivePoolObjects.Add(spawnedPrefab);
            });

            examplePoolBall.OnDestroyed.AddListener(() =>
            {
                activePoolObjects.Remove(spawnedPrefab);
                inactivePoolObjects.Remove(spawnedPrefab);
            });
        }

        private void OnSpawnedSavedInstance(Scene scene, SavedInstance savedInstance)
        {
            if (scene == this.gameObject.scene && savedInstance.SpawnInfo.userTag == "examplepool"
                && savedInstance.SpawnInfo.spawnSource == SaveInstanceManager.SpawnSource.FromSave)
            {
                if (savedInstance.gameObject.activeSelf)
                {
                    activePoolObjects.Add(savedInstance.gameObject);
                }
                else
                {
                    inactivePoolObjects.Add(savedInstance.gameObject);
                }

                currentSpawnedObjects++;

                var instanceGameObject = savedInstance.gameObject;

                AssignEvents(instanceGameObject);
            }
        }

        private void OnReloadScene()
        {
            SceneManager.LoadScene(this.gameObject.scene.name);
        }

        private void OnLoadFromDiskAndSync()
        {
            SaveMaster.ClearSlot(false, false);
            SaveMaster.SetSlotToLastUsedSlot(true);
        }

        private void OnWriteToDisk()
        {
            SaveMaster.WriteActiveSaveToDisk(true);
        }

        private void OnSyncLoad()
        {
            SaveMaster.SyncLoad();
        }

        private void OnSyncSave()
        {
            SaveMaster.SyncSave();
        }

        private void OnSpawnButton()
        {
            if (inactivePoolObjects.Count > 0)
            {
                int getIndex = inactivePoolObjects.Count - 1;
                var getObject = inactivePoolObjects[getIndex];

                getObject.GetComponent<ExamplePoolBall>().ResetValues();

                // Remove existing data before displaying it again.
                // Since it might have been saved before. Meaning it loads older data.
                SaveMaster.WipeSaveable(getObject.GetComponent<Saveable>(), false);

                inactivePoolObjects.RemoveAt(getIndex);
                activePoolObjects.Add(getObject);

                getObject.transform.position = ReturnRandomPosition();

                getObject.SetActive(true);
            }
            else
            {
                var spawn = SpawnPrefab();
                spawn.transform.position = ReturnRandomPosition();
            }
        }

        private Vector3 ReturnRandomPosition()
        {
            return new Vector3(UnityEngine.Random.Range(-1, 1), 10, UnityEngine.Random.Range(-1, 1));
        }
    }
}