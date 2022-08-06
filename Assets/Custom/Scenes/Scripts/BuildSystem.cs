﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;
using UnityEngine.InputSystem;

namespace StupidHumanGames
{
	public class BuildSystem : MonoBehaviour
	{
		public ThirdPersonController tpc;
	    [SerializeField] Camera cam;//camera used for raycast
		public LayerMask layer;//the layer that the raycast will hit on
		public int buildDistance;
		private GameObject previewGameObject = null;//referance to the preview gameobject
		private Preview previewScript = null;//the Preview.cs script sitting on the previewGameObject
		public float stickTolerance = 1.5f;//used for measuring deviation in the mouse when the buildSystem is paused
		[HideInInspector] //hiding this in inspector, so it doesnt accidently get clicked
		public bool isBuilding = false;//are we or are we not currently trying to build something? 
		private bool pauseBuilding = false;//used to pause the raycast
		public ItemSO[] buildItems;             //attempt to spawn inventory items from database  ***********
		public ItemSO currentBuildItem;
		public bool isBuilt = false;


		private void Awake()
		{
			buildItems = Resources.LoadAll<ItemSO>("BuildItems");
		}
		private void Start()
		{

			cam = Camera.main;
		}
		private void Update()
		{
            if (InventoryManager.IsOpen() && tpc._input._pickup)
            {				
				removeItemFromField();
				tpc._input._pickup = false;
			}
			
			if (InventoryManager.IsOpen() && tpc._input._attack)
			{
				BuildTheFucker();
				pauseBuilding = false;
				tpc._input._attack = false;
			}
	/*		if (Input.GetKeyDown(KeyCode.Y))//rotate
			{
				if (previewGameObject != null)
					previewGameObject.transform.Rotate(0, 45f, 0);//rotate the preview 90 degrees. You can add in your own value here
			}
			if (Input.GetKeyDown(KeyCode.X))//rotate
			{
				if (previewGameObject != null)
					previewGameObject.transform.Rotate(45f, 0, 0);//rotate the preview 90 degrees. You can add in your own value here
			}
			if (Input.GetKeyDown(KeyCode.Z))//rotate
			{
				if (previewGameObject != null)
					previewGameObject.transform.Rotate(0, 0, 45f);//rotate the preview 90 degrees. You can add in your own value here
			}
			if (Input.GetKeyDown(KeyCode.PageUp))
			{
				if (previewGameObject != null)
					previewGameObject.transform.localScale += new Vector3(0, .1f, 0);
			}
			if (Input.GetKeyDown(KeyCode.PageDown))
			{
				if (previewGameObject != null)
					previewGameObject.transform.localScale -= new Vector3(0, .1f, 0);
			}*/

			if (isBuilding)
			{

				if (pauseBuilding)//is the build system currently paused? if so then you need to check deviation in the mouse 
				{
					float mouseX = Input.GetAxis("Mouse X");//get the mouses horizontal movement..these may be different on your copy of unity
					float mouseY = Input.GetAxis("Mouse Y");//get the mouses vertical movement..these may be different on your copy of unity

					if (Mathf.Abs(mouseX) >= stickTolerance || Mathf.Abs(mouseY) >= stickTolerance)//check if mouseX or mouseY is greater than stickTolerance
					{
						pauseBuilding = false;//if it is, then unpause building, and call the raycast again

					}

				}
				else//if building system isn't paused then call the raycast
				{
					DoBuildRay();
				}
			}
			//if (isBuilding)  //testing camera movement
			{
				//float step = speed * Time.deltaTime;

			}

		}
		public void removeItemFromField()
		{

			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 10000))
			{

				Debug.DrawLine(ray.origin, hit.point);
				//Debug.Log("Clicked on " + hit.transform.gameObject.name);


				foreach (ItemSO item in buildItems)
				{
					if (hit.collider.name == item.name)
					{
						InventoryManager.AddItemToInventory(item, 1);
						Destroy(hit.transform.gameObject);
					}
				}
			}
		}



		/// <summary>
		/// However you want to start building with the system. This is the method you would need to call
		/// either from your Inventory, HotBar, or some other source.
		/// You will need to pass in a referance to the previewGameObject that you want to build
		/// </summary>

		public void NewBuild(GameObject _go)
		{
			

			isBuilt = false;
			//float step = speed * Time.deltaTime;
			//transform.position = Vector3.MoveTowards(transform.position.y, target.position, step);
			CancelBuild();
			//camPivot.transform.localPosition = new Vector3(camPivot.transform.localPosition.x + camPosX, camPivot.transform.localPosition.y + camPosY, camPivot.transform.localPosition.z + camPosZ);
			previewGameObject = Instantiate(_go, Vector3.zero, Quaternion.identity);
			previewScript = previewGameObject.GetComponent<Preview>();

			isBuilding = true;
			
			
		}

		public void CancelBuild()//you no longer want to build, this will get rid of the previewGameObject in the scene
		{

			Destroy(previewGameObject);
			previewGameObject = null;
			previewScript = null;
			isBuilding = false;

		}

		public void BuildTheFucker()
		{
			if (previewScript && previewScript.GetSnapped())
			{
				//camPivot.transform.localPosition = new Vector3(camPivot.transform.localPosition.x - camPosX, camPivot.transform.localPosition.y - camPosY, camPivot.transform.localPosition.z - camPosZ);
				previewScript.Place();
				previewGameObject = null;
				previewScript = null;
				isBuilding = false;
				isBuilt = true;

				//AudioManager.instance.Play("Hammer");
			}




		}

		public void PauseBuild(bool _value)//public method to change the pauseBuilding bool from another script. Preview.cs calls this 
										   //method whereever it snaps to a snap point
		{
			pauseBuilding = _value;
			//Debug.Log(_value);
		}

		private void DoBuildRay()//actually positions your previewGameobject in the world
		{
			Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());//raycast stuff
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, buildDistance, layer))//notice the layer
			{
				/*Since I am using unity primitives in this example I have to postion the previewGameobject in a special way, 
				 because of how unity positions things in the scene. If you brought something over from blender, and you have the 
				 anchor points setup correctly(located on bottom of model). You can use the line commented out below,
				 as opposed to the other lines*/

				//If your using unity primitives use these 3 lines
				//float y = hit.point.y + (previewGameObject.transform.localScale.y / 2f);
				//Vector3 pos = new Vector3(hit.point.x, y, hit.point.z);
				// previewGameObject.transform.position = pos;

				//if your using something from blender and anchor points are setup correctly use this line
				previewGameObject.transform.position = hit.point;
				
			}
		}
	}
}
