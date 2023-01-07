using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;
using UnityEngine.InputSystem;

namespace StupidHumanGames
{
	public class BuildSystem : MonoBehaviour
	{
		[SerializeField] AudioSource _audioSource;
		[SerializeField] AudioClip _buildSound;
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
		public void OnAttack()
		{
			if (!InventoryManager.IsOpen()) return;
			BuildTheFucker();
			pauseBuilding = false;
		}
		private void Update()
		{
			if (InventoryManager.IsOpen())
			{
				canCancel = true;
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
			CancelBuild();
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
				_audioSource.PlayOneShot(_buildSound, 1);
				//AudioManager.instance.Play("Hammer");
			}
		}

		public void PauseBuild(bool _value)//public method to change the pauseBuilding bool from another script. Preview.cs calls this 
										   //method whereever it snaps to a snap point
		{
			pauseBuilding = _value;
		}
		private void DoBuildRay()//actually positions your previewGameobject in the world
		{
			Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());//raycast stuff
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, buildDistance, layer))//notice the layer
			{
				previewGameObject.transform.position = hit.point;
			}
		}
	}
}
