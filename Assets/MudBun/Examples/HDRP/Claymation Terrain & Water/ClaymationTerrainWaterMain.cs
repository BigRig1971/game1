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
  public class ClaymationTerrainWaterMain : MonoBehaviour
  {
    public float Period = 0.05f;
    private float m_lastComputeTime = 0.0f;

    private Mesh m_colliderMesh;

    private void Update()
    {
      var renderer = GetComponent<MudRenderer>();
      if (renderer == null)
        return;

      if (renderer.IsMeshGenerationPending(m_colliderMesh))
        return;

      int q = (int) Mathf.Floor(Time.time / Period);
      float qT = q * Period;
      if (qT <= m_lastComputeTime)
        return;

      m_colliderMesh = renderer.AddCollider(renderer.gameObject, true, m_colliderMesh);
      renderer.MarkNeedsCompute();

      m_lastComputeTime = qT;
    }
  }
}

