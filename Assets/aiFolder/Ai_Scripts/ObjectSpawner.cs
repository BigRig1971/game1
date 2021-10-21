using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObjectSpawner : MonoBehaviour
{
 
    public GameObject prefab; 
    public float delay = .1f;
    public int maximum = 15;
	GameObject go;
    


    public List<GameObject> list = new List<GameObject>();
    private float m_internalTimer = 5f;
    void Start()
    {
        m_internalTimer = delay;
    }
    public void OnRemoveObject()
	{
        list.Remove(go);
	}
    void Update()
    {
        
        if (list.Count >= maximum)
		{
           
            return;
        }
           
        

        m_internalTimer -= Time.deltaTime;
        m_internalTimer = Mathf.Max(m_internalTimer, 0f);
        if (m_internalTimer == 0f)
        {
            Vector3 offset = GetOffset();
			GameObject obj = Instantiate(prefab, transform.position + offset, Quaternion.identity) as GameObject;
      

            list.Add(obj);
            go = obj;
            m_internalTimer = delay;
        }
    }

    void LateUpdate()
    {
        //remove all destroyed objects
        list.RemoveAll(o => (o == null || o.Equals(null)));
		
    }

    Vector3 GetOffset()
    {
        // here goes your random offset, please implement this by yourself
        return Vector3.zero;
    }
}