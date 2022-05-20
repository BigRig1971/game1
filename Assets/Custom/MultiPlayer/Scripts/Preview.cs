using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Lowscope.Saving;


namespace StupidHumanGames
{
	public class Preview : MonoBehaviour
	{
		public GameObject prefab;

		private MeshRenderer myRend;


		private Transform spawnedTransform;
		public Material goodMat;//green material
		public Material badMat;//red material
		private BuildSystem buildSystem;
		private bool isSnapped = false;//only this script should change this value
		public bool isFoundation = false;//this is a special rule for foundations. 
		public List<string> tagsISnapTo = new List<string>();//list of all of the SnapPoint tags this particular preview can snap too
		private void Start()
		{
			buildSystem = GameObject.FindObjectOfType<BuildSystem>();
			myRend = GetComponent<MeshRenderer>();
			ChangeColor();
		}
		public void Place()
		{
			var spawnedObject = SaveMaster.SpawnSavedPrefab(InstanceSource.Resources, prefab.name);
			spawnedObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			spawnedObject.transform.rotation = this.transform.rotation;
			PhotonNetwork.AddCallbackTarget(spawnedObject);
			Destroy(gameObject);
		}
		private void ChangeColor()//changes between red and greed depending if this preview is/is not snapped to anything
		{
			Material[] mats = myRend.materials;
			if (isFoundation)
			{
				isSnapped = true;
			}
			if (isSnapped)
			{
				myRend.material = goodMat;
			}
			else
			{
				myRend.material = badMat;
			}
		}
		private void OnTriggerEnter(Collider other)//this is what dertermins if you are snapped to a snap point
		{
			for (int i = 0; i < tagsISnapTo.Count; i++)//loop through all the tags this preview can snap too
			{
				string currentTag = tagsISnapTo[i];//setting the current tag were looking at to a string...its easier to write currentTag then tagsISnapTo[i]

				if (other.CompareTag(currentTag))
				{
					buildSystem.PauseBuild(true);//this, and the line below are how you snap
					transform.position = other.transform.position;//set position of preview so that it "snaps" into position
					transform.rotation = other.transform.rotation;
					isSnapped = true;//change the bool so we know what color this preview needs to be
					ChangeColor();

				}
			}
		}
		private void OnTriggerExit(Collider other)//this is what determins if you are no longer snapped to a snap point
		{
			for (int i = 0; i < tagsISnapTo.Count; i++)//loop through all tags
			{
				string currentTag = tagsISnapTo[i];

				if (other.tag == currentTag)//if we OnTriggerExit something that we can snap too
				{
					isSnapped = false;//were no longer snapped
					ChangeColor();//change color
				}
			}
		}
		public bool GetSnapped()//accessor for the isSnapped bool. 
		{
			return isSnapped;
		}
	}
}
