using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;



namespace StupidHumanGames
{
    public class Preview : MonoBehaviour
    {
        public LayerMask layer;
        public GameObject prefab;
        public float spawnOffset = 0f;
        private MeshRenderer myRend;
        public bool isGrounded = false;
        private Transform spawnedTransform;
        public Material goodMat;//green material
        public Material badMat;//red material
        private BuildSystem buildSystem;
        private bool isSnapped = false;//only this script should change this value

        public bool isFoundation = false;//this is a special rule for foundations. 
        public List<string> tagsISnapTo = new List<string>();//list of all of the SnapPoint tags this particular preview can snap too
        public Collider[] hitColliders;
        


        public Material[] materials;
        private void Start()
        {
            buildSystem = GameObject.FindObjectOfType<BuildSystem>();
            myRend = GetComponentInChildren<MeshRenderer>();
            materials = myRend.materials;
            ChangeColor();
        }
        public void Place()
        {
           // if (OnHitObstacle()) buildSystem.removeItemFromField();
           StartCoroutine(OnPlace());
            
        }
        IEnumerator OnPlace()
        {
            yield return new WaitForSeconds(.1f);
           
            SaveGame.SpawnPrefab(prefab, transform.position, transform.rotation);
           
            // prefab.transform.SetParent(adoptiveParent);
            //Debug.Log(adoptiveParent.name);
            Destroy(gameObject);
        }

        private void ChangeColor()//changes between red and greed depending if this preview is/is not snapped to anything
        {


            if (isFoundation && isGrounded)
            {
                isSnapped = true;
            }


            if (isSnapped)
            {
                var materials = myRend.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = goodMat;
                }
                myRend.sharedMaterials = materials;
            }
            else
            {
                var materials = myRend.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {

                    materials[i] = badMat;
                }
                myRend.sharedMaterials = materials;

            }
        }
        private void OnTriggerEnter(Collider other)//this is what dertermins if you are snapped to a snap point
        {
            

            if (other.CompareTag("Ground"))
            {
                isGrounded = true;
                ChangeColor();
            }
          
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
           
            if (other.CompareTag("Ground"))
            {
                isGrounded = false;
                ChangeColor();
            }
            for (int i = 0; i < tagsISnapTo.Count; i++)//loop through all tags
            {
                string currentTag = tagsISnapTo[i];

                if (other.CompareTag(currentTag))//if we OnTriggerExit something that we can snap too
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
        bool OnHitObstacle()
        {
            bool hit = false;
            hitColliders = Physics.OverlapSphere(transform.position, 3f, layer);
            foreach (Collider col in hitColliders)
            {
                if (transform.position == col.transform.position)
                {
                    Debug.Log(col.gameObject.name);
                    hit = true;
                }
            }
            return hit;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 3f);
        }
    }
}
