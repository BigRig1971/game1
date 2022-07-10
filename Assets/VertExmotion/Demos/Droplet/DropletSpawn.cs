using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Kalagaan
{
    public class DropletSpawn : MonoBehaviour
    {

        
        public GameObject m_ref;
        [Range(0f, 3f)]
        public float m_spawnRate = 1f;
        [Range(.5f,1.5f)]
        public float m_scale = 1f;


        List<GameObject> m_list;
        float m_lastSpawn;


        private void Awake()
        {
            m_list = new List<GameObject>();
        }

        void Update()
        {
            if (m_spawnRate > 0)
            {

                if (Time.time > m_lastSpawn + 1f / m_spawnRate)
                {
                    m_list.Add(GameObject.Instantiate(m_ref));
                    m_list[m_list.Count - 1].SetActive(true);
                    m_list[m_list.Count - 1].transform.position = transform.position;
                    m_list[m_list.Count - 1].transform.localScale = Vector3.one * m_scale;
                    m_lastSpawn = Time.time;
                }
            }

            for(int i=0;i< m_list.Count; ++i)
            {
                if (m_list[i].transform.position.y < -20f)
                {
                    Destroy(m_list[i]);
                    m_list.RemoveAt(i--);
                }
            }
        }
    }
}