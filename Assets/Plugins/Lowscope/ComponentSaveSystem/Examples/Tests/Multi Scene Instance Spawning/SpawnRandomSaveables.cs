using Lowscope.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lowscope.Saving.Examples
{
    public class SpawnRandomSaveables : MonoBehaviour
    {
        [SerializeField] private string[] possiblePrefabs;

        public void Spawn()
        {
            int random = Random.Range(90, 100);

            int randomPrefab = Random.Range(0, possiblePrefabs.Length);

            for (int i = 0; i < random; i++)
            {
                var spawnedObject = SaveMaster.SpawnSavedPrefab(InstanceSource.Custom, possiblePrefabs[randomPrefab], customSource: "MultiLevelBenchmark", scene: this.gameObject.scene);
                spawnedObject.transform.position = new Vector3()
                {
                    x = this.transform.position.x + Random.Range(-10f, 10f),
                    y = 0,
                    z = this.transform.position.z + Random.Range(-10f, 10f)
                };

                spawnedObject.transform.rotation = Quaternion.Euler(Random.Range(-10f, 10f), Random.Range(0, 360), Random.Range(-10f, 10f));
            }
        }
    }
}