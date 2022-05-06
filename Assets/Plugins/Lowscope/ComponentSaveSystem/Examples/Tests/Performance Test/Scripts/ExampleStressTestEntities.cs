using Lowscope.Saving;
using Lowscope.Saving.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lowscope.Saving.Examples
{
    public class ExampleStressTestEntities : MonoBehaviour
    {
        [SerializeField] private Text debugText;
        [SerializeField] private GameObject examplePrefab;

        [SerializeField] private Button spawnEntities;
        [SerializeField] private Button removeEntities;
        [SerializeField] private Button syncSaveAll;
        [SerializeField] private Button syncLoadAll;
        [SerializeField] private Button writeToDisk;
        [SerializeField] private Button loadFromDisk;

        [SerializeField] private Button shuffleTransforms;

        [SerializeField] private Text spawnEntitiesMs;
        [SerializeField] private Text removeEntitiesMs;
        [SerializeField] private Text syncSaveAllMs;
        [SerializeField] private Text syncLoadAllMs;
        [SerializeField] private Text shuffleTransformsMs;
        [SerializeField] private Text writeToDiskMs;
        [SerializeField] private Text loadFromDiskMs;

        private List<SavedInstance> savedInstances = new List<SavedInstance>();

        private int totalSpawnedInstances = 0;

        private int totalSpawnedComponents;

        private const string CUSTOMSPAWNERID = "ExampleStressTestSpawner";

        private void Awake()
        {
            SaveMaster.AddPrefabResourceLocation(CUSTOMSPAWNERID, _ => examplePrefab);
            SaveMaster.OnSpawnedSavedInstance += OnSpawnedSavedPrefab;

            spawnEntities.onClick.AddListener(() => Spawn(1000));
            removeEntities.onClick.AddListener(() => Remove(1000));
            syncSaveAll.onClick.AddListener(SyncSave);
            syncLoadAll.onClick.AddListener(SyncLoad);
            writeToDisk.onClick.AddListener(WriteToDisk);
            loadFromDisk.onClick.AddListener(LoadFromDisk);

            shuffleTransforms.onClick.AddListener(() => ShuffleAllInstanceTransforms());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SaveMaster.SpawnSavedPrefab(InstanceSource.Custom, "DummyID", CUSTOMSPAWNERID, gameObject.scene);
            }
        }

        private void WriteToDisk()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            SaveMaster.WriteActiveSaveToDisk(false);

            sw.Stop();
            writeToDiskMs.text = sw.Elapsed.TotalMilliseconds.ToString();
        }

        private void LoadFromDisk()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            SaveMaster.ClearSlot(false, false);
            savedInstances.Clear();

            SaveMaster.SetSlotToLastUsedSlot(true);

            sw.Stop();
            loadFromDiskMs.text = sw.Elapsed.TotalMilliseconds.ToString();
        }

        private void SyncSave()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            SaveMaster.SyncSave();

            sw.Stop();
            syncSaveAllMs.text = sw.Elapsed.TotalMilliseconds.ToString();
        }

        private void SyncLoad()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            SaveMaster.SyncLoad();

            sw.Stop();
            syncLoadAllMs.text = sw.Elapsed.TotalMilliseconds.ToString();
        }

        private void ShuffleAllInstanceTransforms()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            System.Random rnd = new System.Random();

            int instanceCount = savedInstances.Count;
            for (int i = 0; i < instanceCount; i++)
            {
                ShuffleObjectTransform(rnd, savedInstances[i].gameObject);
            }

            sw.Stop();
            shuffleTransformsMs.text = sw.Elapsed.TotalMilliseconds.ToString();
        }

        private void OnDestroy()
        {
            SaveMaster.OnSpawnedSavedInstance -= OnSpawnedSavedPrefab;
        }

        private void OnSpawnedSavedPrefab(Scene scene, SavedInstance instance)
        {
            totalSpawnedInstances++;
            totalSpawnedComponents += 3;

            debugText.text = string.Format("Spawned Instances: {0}. Components: {1}", totalSpawnedInstances.ToString(), totalSpawnedComponents.ToString());
            savedInstances.Add(instance);
        }

        private void Remove(int amount)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int targetTotal = totalSpawnedInstances - amount;
            if (targetTotal < 0)
            {
                amount -= Mathf.Abs(targetTotal);
            }

            for (int i = amount - 1; i >= 0; i--)
            {
                GameObject.Destroy(savedInstances[i].gameObject);
                savedInstances.RemoveAt(i);

                totalSpawnedInstances--;
                totalSpawnedComponents -= 3;
            }

            sw.Stop();

            debugText.text = string.Format("Spawned Instances: {0}. Components: {1}", totalSpawnedInstances.ToString(), totalSpawnedComponents.ToString());
            removeEntitiesMs.text = sw.Elapsed.TotalMilliseconds.ToString();
        }

        private void Spawn(int amount)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < amount; i++)
            {
                // Id is not required, as the custom spawner does not watch for it and always returns examplePrefab.
                SaveMaster.SpawnSavedPrefab(InstanceSource.Custom, "DummyID", CUSTOMSPAWNERID, gameObject.scene);
            }

            sw.Stop();
            spawnEntitiesMs.text = sw.Elapsed.TotalMilliseconds.ToString();
        }

        private static void ShuffleObjectTransform(System.Random rnd, GameObject obj)
        {
            obj.transform.position = new Vector3(rnd.Next(-50, 50), rnd.Next(-50, 50), rnd.Next(-50, 50));
            obj.transform.rotation = Quaternion.Euler(rnd.Next(0, 360), rnd.Next(0, 360), rnd.Next(0, 360));
            obj.transform.localScale = Vector3.one * (float)(rnd.NextDouble() + 0.1f);
        }
    }
}