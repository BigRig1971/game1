using Lowscope.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lowscope.Saving.Examples
{

    public class PartSpawner : MonoBehaviour, ISaveable
    {
        [System.Serializable]
        private struct SpawnablePrefabs
        {
            public string id;
            public GameObject prefab;
        }

        [SerializeField] private string spawnedSceneName;
        [SerializeField] private float totalOperationsAtOnce = 50;
        [SerializeField] private Vector2Int gridSize = new Vector2Int(10, 10);
        [SerializeField] private SpawnablePrefabs[] spawnablePrefabs;

        private Dictionary<string, GameObject> indexedSpawnablePrefabs;

        private Camera cam;
        private bool neverSaved = true;
        private int operations = 0;

        private void Awake()
        {
            cam = Camera.main;

            indexedSpawnablePrefabs = new Dictionary<string, GameObject>();

            int spawnablePrefabCount = spawnablePrefabs.Length;
            for (int i = 0; i < spawnablePrefabCount; i++)
            {
                SpawnablePrefabs data = spawnablePrefabs[i];
                indexedSpawnablePrefabs.Add(data.id, data.prefab);
            }

            SaveMaster.AddPrefabResourceLocation("MultiLevelBenchmark", OnLoadResource);
        }

        private void OnDestroy()
        {
            SaveMaster.RemovePrefabResourceLocation("MultiLevelBenchmark");
        }

        private GameObject OnLoadResource(string arg1)
        {
            GameObject resource = null;
            if (indexedSpawnablePrefabs.TryGetValue(arg1, out resource))
            {
                return resource;
            }
            else
            {
                return null;
            }
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1);

            int ySize = (int)(gridSize.x * 0.5f);
            int xSize = (int)(gridSize.y * 0.5f);

            for (int y = -ySize; y < ySize; y++)
            {
                for (int x = -xSize; x < xSize; x++)
                {
                    int operationIndex = SceneManager.sceneCount;
                    var operation = SceneManager.LoadSceneAsync(spawnedSceneName, LoadSceneMode.Additive);

                    Vector3 targetPosition = new Vector3(x * 20, 0, y * 20);

                    operation.completed += (s) =>
                    {
                        OnSceneSpawned(SceneManager.GetSceneAt(operationIndex), targetPosition, operationIndex);
                    };

                    operations++;

                    yield return new WaitUntil(() => operations < totalOperationsAtOnce);
                }
            }
        }

        private void OnSceneSpawned(Scene scene, Vector3 targetPosition, int id)
        {
            operations--;
            SaveMaster.SpawnInstanceManager(scene, targetPosition.ToString());
            scene.GetRootGameObjects()[0].gameObject.transform.position = targetPosition;

            if (neverSaved)
            {
                scene.GetRootGameObjects()[0].GetComponent<SpawnRandomSaveables>().Spawn();
            }
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000000))
                {
                    foreach (var item in Physics.OverlapSphere(hit.point, 10))
                    {
                        // Using default Unity tag so it doesn't interfere with project settings.
                        if (item.CompareTag("Finish"))
                        {
                            Vector3 offset = item.transform.position - hit.point;
                            offset.y = 0;
                            item.transform.position += offset * Time.deltaTime;
                        }
                    }
                }
            }
        }

        public string OnSave()
        {
            return bool.FalseString;
        }

        public void OnLoad(string data)
        {
            neverSaved = bool.Parse(data);
        }

        public bool OnSaveCondition()
        {
            return true;
        }
    }
}