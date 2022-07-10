using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Kalagaan
{
    public class BakeMeshDemoRandomPosition : MonoBehaviour
    {

        public Vector3 m_direction = Vector3.forward;        
        public float m_speed = 1f;
        public float m_maxAngle = 50f;

        Vector3 m_startposition;

        // Use this for initialization
        void Start()
        {
            m_startposition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position += m_direction * m_speed * Time.deltaTime;

            Vector3 newDir = m_startposition - transform.position;
            m_direction = Vector3.RotateTowards(m_direction, newDir, Mathf.Deg2Rad * m_maxAngle * Time.deltaTime, 1f).normalized;
        }
    }
}