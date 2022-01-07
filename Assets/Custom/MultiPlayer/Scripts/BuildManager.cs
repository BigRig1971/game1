using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{

	public KeyCode foundationKey;
	public KeyCode wallKey;
	public KeyCode roofKey;
	public KeyCode doorwayKey;
	public KeyCode triangleKey;
	public KeyCode windowKey;
	public KeyCode buildModeKey;

	public GameObject foundationPreview;
	public GameObject wallPreview;
	public GameObject roofPreview;
	public GameObject doorwayPreview;
	public GameObject trianglePreview;
	public GameObject windowPreview;
	public Text modeTxt;
	public BuildSystem buildSystem;
	

	private void Update()
	{

		//modeTxt.text = "Build Mode";
		//modeTxt.color = Color.red;

		if (Input.GetKeyDown(foundationKey) && !buildSystem.isBuilding)
		{
			buildSystem.NewBuild(foundationPreview);
		}
		if (Input.GetKeyDown(wallKey) && !buildSystem.isBuilding)
		{

			buildSystem.NewBuild(wallPreview);
		}
		if (Input.GetKeyDown(roofKey) && !buildSystem.isBuilding)
		{
			buildSystem.NewBuild(roofPreview);
		}
		if (Input.GetKeyDown(doorwayKey) && !buildSystem.isBuilding)
		{
			buildSystem.NewBuild(doorwayPreview);
		}
		if (Input.GetKeyDown(triangleKey) && !buildSystem.isBuilding)
		{
			buildSystem.NewBuild(trianglePreview);
		}
		if (Input.GetKeyDown(windowKey) && !buildSystem.isBuilding)
		{

			buildSystem.NewBuild(windowPreview);
		}
		if (Input.GetMouseButtonDown(0))
		{
			buildSystem.BuildTheFucker();
		}
	}
}
