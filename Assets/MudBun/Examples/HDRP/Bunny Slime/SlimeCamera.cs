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
  public class SlimeCamera : MonoBehaviour
  {
    public Transform Slime;
    public float Spring = 1.0f;

    private Vector3 m_target;
    private Vector3 m_offset;

    void Start()
    {
      if (Slime == null)
        return;

      m_offset = transform.position - Slime.position;
    }

    private void OnEnable()
    {
      Start();
    }

    void FixedUpdate()
    {
      if (Slime == null)
        return;

      m_target = Slime.position + m_offset;
      Vector3 delta = m_target - transform.position;

      transform.position += delta * Spring * Time.fixedDeltaTime;
    }
  }
}
