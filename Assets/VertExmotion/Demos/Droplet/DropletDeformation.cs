using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kalagaan
{
    public class DropletDeformation : MonoBehaviour
    {
        public float m_collisionInflate = .5f;
        public float m_deformationSpeed = 1f;
        public float m_velocityDeformation = .3f;
        public float m_radius = .8f;

        VertExmotion m_vtm = null;

        List<VertExmotionSensorBase> m_collisionSensors;
        VertExmotionSensorBase m_fallingSensor;
        Rigidbody m_rigidbody;

        SphereCollider m_selfCollider = null;
        RaycastHit[] raycastResult = new RaycastHit[20];

        void Awake()
        {
            m_vtm = GetComponent<VertExmotion>();
            m_selfCollider = GetComponent<SphereCollider>();
            m_collisionSensors = new List<VertExmotionSensorBase>();
            m_rigidbody = GetComponent<Rigidbody>();

            m_fallingSensor = m_vtm.CreateSensor("Falling");
            m_fallingSensor.m_params.translation.motionFactor = 0f;//disable wobble FX on falling sensors
            m_vtm.AddSensor(m_fallingSensor);
            
        }



        private void Update()
        {
            if(m_rigidbody != null)
            {
                float scale = (transform.lossyScale.x + transform.lossyScale.y + transform.lossyScale.z) / 3f;
                m_rigidbody.mass = scale;
               if ( m_rigidbody.velocity.magnitude>0)
               {                    
                    m_fallingSensor.transform.position = transform.position - m_rigidbody.velocity.normalized * .5f * scale;                    
                    m_fallingSensor.m_params.translation.worldOffset = -m_rigidbody.velocity * m_velocityDeformation * scale;                    
               }
            }


        }


        void FixedUpdate()
        {
            float scale = (transform.lossyScale.x+ transform.lossyScale.y + transform.lossyScale.z)/3f;

            for (int i = 0; i < m_collisionSensors.Count; ++i)
            {
                //reset deformation (2x slower than collision)
                m_collisionSensors[i].m_params.inflate -= Time.deltaTime * 10f * .5f * m_deformationSpeed;
                m_collisionSensors[i].m_params.inflate = Mathf.Clamp(m_collisionSensors[i].m_params.inflate, 0f, m_collisionInflate);
            }

            Ray r = new Ray();
            r.origin = transform.position;// + Vector3.up * .1f;
            r.direction = Vector3.down;

            //check collisions on the sphere
            int id = 0;


            int hitCount = Physics.SphereCastNonAlloc(r, m_radius * scale, raycastResult, .1f);
            for (int i = 0; i < hitCount; ++i)
            {
                if (raycastResult[i].collider != m_selfCollider)
                {
                    Vector3 hitpoint = raycastResult[i].collider.ClosestPoint(transform.position);//closest point doesn't work with MeshCollider
                    Vector3 hitDir = (hitpoint - transform.position);
                   
                    if (m_collisionSensors.Count == id)
                    {
                        //create a new sensor in the pool                        
                        m_collisionSensors.Add( m_vtm.CreateSensor("Collision") );
                        m_vtm.AddSensor(m_collisionSensors[id]);
                        m_collisionSensors[id].m_envelopRadius = 1f;
                        m_collisionSensors[id].m_params.translation.motionFactor = 0f;//disable wobble FX on collision sensors

                    }
                    if (Vector3.Distance(hitpoint, m_selfCollider.transform.position) > 0)
                    {

                        //m_collisionSensors[id].transform.position = hitpoint + hitDir.normalized * .3f * scale;
                        m_collisionSensors[id].transform.position = r.origin + hitDir.normalized * scale * m_radius;

                    }
                    m_collisionSensors[id].m_params.inflate += Time.deltaTime*10f* m_deformationSpeed;
                    m_collisionSensors[id].m_params.inflate = Mathf.Clamp(m_collisionSensors[id].m_params.inflate, 0f, m_collisionInflate*(1f-(hitDir.magnitude-m_selfCollider.radius* scale)));
                    id++;
                }
            }

            for (int i = hitCount; i < m_collisionSensors.Count; ++i)
            {
                //disable unused sensors of the pool
                m_collisionSensors[i].m_params.inflate = 0f;
            }

                      
           
        }
    }
}