using Lowscope.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExampleThrottledPrefabLoading : MonoBehaviour, ISaveable
{
	private bool hasSpawnedPrefabs = false;

	[SerializeField] private int spawnPrefabs = 10000;
	
	[SerializeField] private GameObject testSaveable;
	[SerializeField] private GameObject initialMessage;
	[SerializeField] private GameObject doneMessage;

	[SerializeField] private Slider progressSlider;
	[SerializeField] private Text progressText;

	private GameObject OnLoadResource(string resourceIndex)
	{
		// We dont use the resource index, mainly because this is designed to just return one object.
		// Refer to the Bomber Dog sample to see a better example of a custom resource location.
		return testSaveable;
	}

	private void Awake()
	{
		SaveMaster.AddPrefabResourceLocation("ThrottlePrefabResource", OnLoadResource);

		if (!hasSpawnedPrefabs)
		{
			for (int i = 0; i < 10000; i++)
			{
				SaveMaster.SpawnSavedPrefab(InstanceSource.Custom, ".", "ThrottlePrefabResource");
			}

			initialMessage.gameObject.SetActive(true);
		}

		SaveMaster.OnStartSpawningInstances += OnStartSpawningInstances;
		SaveMaster.OnFinishSpawningInstances += OnFinishSpawningInstances;
		SaveMaster.OnSpawnInstanceProgress += OnSpawnInstanceProgress;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (doneMessage.gameObject.activeSelf)
			{
				SaveMaster.WipeSceneData(this.gameObject.scene.name, true);
				doneMessage.gameObject.SetActive(false);
			}
		}
	}

	private void OnSpawnInstanceProgress(Scene scene, int activeSpawn, int totalSpawn, float percentage)
	{
		progressText.text = string.Format("Loading... Spawned {0} of {1}. {2:00}% done.", 
			activeSpawn, totalSpawn, (percentage * 100));

		progressSlider.value = activeSpawn;
	}

	private void OnStartSpawningInstances(Scene scene, int amount)
	{
		progressSlider.gameObject.SetActive(true);
		progressText.gameObject.SetActive(true);

		progressSlider.minValue = 0;
		progressSlider.maxValue = amount;
		progressSlider.value = 0;
	}

	private void OnFinishSpawningInstances(Scene obj)
	{
		progressSlider.gameObject.SetActive(false);
		progressText.gameObject.SetActive(false);
		doneMessage.gameObject.SetActive(true);
	}

	private void OnDestroy()
	{
		SaveMaster.OnStartSpawningInstances -= OnStartSpawningInstances;
		SaveMaster.OnFinishSpawningInstances -= OnFinishSpawningInstances;
		SaveMaster.OnSpawnInstanceProgress -= OnSpawnInstanceProgress;
	}

	public string OnSave()
	{
		hasSpawnedPrefabs = true;
		return hasSpawnedPrefabs.ToString();
	}

	public void OnLoad(string data)
	{
		hasSpawnedPrefabs = bool.Parse(data);
	}

	public bool OnSaveCondition()
	{
		return !hasSpawnedPrefabs;
	}
}