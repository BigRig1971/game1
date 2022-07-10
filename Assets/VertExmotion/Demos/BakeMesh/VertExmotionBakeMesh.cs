using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Kalagaan
{
	public class VertExmotionBakeMesh : MonoBehaviour
    {	
                
        public VertExmotion m_vtm;
        public MeshFilter m_target;
        public MeshCollider m_meshCollider;

        public bool m_bake = false;
        public bool m_bakeCollider = false;

        public void Awake()
		{
            if(m_vtm == null)
                m_vtm = GetComponent<VertExmotion>();

            m_meshCollider = GetComponent<MeshCollider>();

        }


		public void Update()
		{
			if ((m_bake || m_bakeCollider) && m_vtm != null)
			{
				Mesh m = m_vtm.BakeMesh();
				if (m_bake && m_target != null)
				{
					m_target.sharedMesh = m;
				}

                if(m_meshCollider !=null && m_bakeCollider)
                {
                    m_meshCollider.sharedMesh = m;
                    m_meshCollider.inflateMesh = false;
                    
                }
			}
		}       
	}
}