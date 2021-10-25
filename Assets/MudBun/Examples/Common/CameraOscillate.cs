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
  public class CameraOscillate : MonoBehaviour
  {
    public float Omega = 1.0f;
    public float Range = 1.0f;

    private float m_xzDist = 0;
    private float m_initTheta = 0.0f;

    private void Start()
    {
      Vector3 posXz = transform.position;
      posXz.y = 0.0f;
      m_xzDist = posXz.magnitude;
      m_initTheta = Mathf.Atan2(posXz.z, posXz.x);
    }

    private void Update()
    {
      float theta = m_initTheta + Range * Mathf.Sin(Time.time * Omega);

      Vector3 pos = transform.position;
      pos.x = m_xzDist * Mathf.Cos(theta);
      pos.z = m_xzDist * Mathf.Sin(theta);

      transform.position = pos;
      transform.rotation = Quaternion.LookRotation(-transform.position);
    }
  }
}

