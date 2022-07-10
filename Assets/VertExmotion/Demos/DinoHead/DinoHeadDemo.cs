using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kalagaan
{
	public class DinoHeadDemo : MonoBehaviour
	{		
		public VertExmotionSensorBase m_rotationSensor;
		public float m_maxAngle = 90f;
		public float m_rotationSpeed = 1f;
		public float m_vertexDisplacementOffset = 0f;
		public float m_sensorPositionOffset = 1f;

		[Range(0, 1)]
		public float m_fxFactor = 1f;
        		
		Vector3 m_fallDirection;
		Vector3 m_rotationTopStartPosition;		
        Vector3 m_lastposition;
        BounceSystem m_angleBS;
        
        public float m_angle = 0;


		void Start()
		{
			m_rotationTopStartPosition = m_rotationSensor.transform.localPosition;
			m_fallDirection = Vector3.left;			
            m_lastposition = transform.position;
            m_angleBS = new BounceSystem();            

        }

		void Update()
		{

            //Rotate the direction to the gravity vector & the delta position
            Vector3 fallTarget = Physics.gravity.normalized + (m_lastposition - transform.position) * Time.deltaTime * 100f;

            Quaternion q = Quaternion.FromToRotation(m_fallDirection.normalized, fallTarget.normalized);
            m_fallDirection = Quaternion.Lerp(Quaternion.identity, q, Time.deltaTime * m_rotationSpeed)* m_fallDirection;

            m_fallDirection.Normalize();


            
            float dot = Vector3.Dot(m_fallDirection, m_rotationSensor.transform.forward);
			if (dot < 0)
			{
                //limit the falling direction to 90°
                m_fallDirection -= dot * m_rotationSensor.transform.forward;
				m_fallDirection.Normalize();
			}

            if (m_fallDirection.sqrMagnitude > 0)
			{
                //set the position of the sensor for the bend effect
				m_rotationSensor.transform.localPosition = m_rotationTopStartPosition + transform.InverseTransformDirection(m_fallDirection).normalized * m_sensorPositionOffset;

                //the rotation axis must be in local space
				m_rotationSensor.m_params.rotation.axis = Vector3.Cross( transform.InverseTransformDirection(m_rotationSensor.transform.forward), transform.InverseTransformDirection(m_fallDirection)).normalized;

                //compute a bouncy value for the angle                
                m_angleBS.m_target = Mathf.Min(Vector3.Angle(m_rotationSensor.transform.forward, fallTarget), m_maxAngle);
                m_angleBS.damping = 3f;
                m_angleBS.bouncing = 2f;
                m_angleBS.limitMin = -200f;
                m_angleBS.limitMax = 200f;
                m_angle = m_angleBS.Compute(m_angle);

                m_rotationSensor.m_params.rotation.angle = Mathf.Lerp(0, m_angle, m_fxFactor);
				m_rotationSensor.m_params.translation.worldOffset = m_fallDirection * m_vertexDisplacementOffset * m_fxFactor;
			}

            m_lastposition = transform.position;            
		}
	}
}
