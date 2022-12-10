using System.Collections;
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
		public LayerMask removeLayer;
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
		bool canCancel = false;


		private void Awake()
		{
			buildItems = Resources.LoadAll<ItemSO>("");
		}
		private void Start()
		{

			cam = Camera.main;
		}
		public void OnPickup()
		{
			if (!InventoryManager.IsOpen()) return;
			removeFromField();
		}
		
		private void Update()
		{
			if (InventoryManager.IsOpen())
			{
				canCancel = true;
			}

			if (InventoryManager.IsOpen() && tpc._input._attack)
			{

				BuildTheFucker();
				pauseBuilding = false;
				tpc._input._attack = false;
			}
			if (!InventoryManager.IsOpen())
			{
				if (canCancel)
				{
					canCancel = false;
					CancelBuild();

				}
			}
			if (Input.GetKeyDown(KeyCode.Y))
			{
				if (previewGameObject != null)
					previewGameObject.transform.Rotate(0, 15f, 0);
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

		public void removeFromField()
		{
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (!Physics.Raycast(ray, out hit, 100, removeLayer)) return;

			List<Transform> children = new List<Transform>(hit.transform.GetComponentsInChildren<Transform>());
			foreach (Transform child in children)
			{
				foreach (ItemSO i in buildItems)
				{
					if (i.name == child.name)
					{
						Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
						child.gameObject.transform.SetParent(null);
						rb.AddForce(cam.transform.forward * 5, ForceMode.Impulse);
						InventoryManager.AddItemToInventory(i, 1);
						Destroy(child.gameObject, 5);
					}
				}
			}

		}
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
