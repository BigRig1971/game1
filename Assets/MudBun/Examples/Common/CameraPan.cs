/******************************************************************************/
/*
  Project   - MudBun
  Publisher - Long Bunny Labs
              http://LongBunnyLabs.com
  Author    - Ming-Lun "Allen" Chou
              http://AllenChou.net
*/
/******************************************************************************/

using UnityEngine;

namespace MudBun
{
  public class CameraPan : MonoBehaviour
  {
    public float Omega = 1.0f;

    private float m_xzDist = 0;
    private float m_theta = 0.0f;

    private void Start()
    {
      Vector3 posXz = transform.position;
      posXz.y = 0.0f;
      m_xzDist = posXz.magnitude;
      m_theta = Mathf.Atan2(posXz.z, posXz.x);
    }

    private void Update()
    {
      Vector3 pos = transform.position;
      pos.x = m_xzDist * Mathf.Cos(m_theta);
      pos.z = m_xzDist * Mathf.Sin(m_theta);

      transform.position = pos;
      transform.rotation = Quaternion.LookRotation(-transform.position);

      m_theta += Omega * Time.deltaTime;
    }
  }
}

