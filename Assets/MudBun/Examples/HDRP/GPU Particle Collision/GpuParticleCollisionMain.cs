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
using UnityEngine.Rendering;
using UnityEngine.VFX;

namespace MudBun
{
  public class GpuParticleCollisionMain : MonoBehaviour
  {
    public VisualEffect VFX;
    public MudRendererBase Renderer;
    public MudNoiseVolume NoiseVolume;

    private RenderTexture m_sdf;

    private void UpdateSdf()
    {
      if (VFX == null)
        return;

      if (Renderer == null)
        return;

      if (NoiseVolume == null)
        return;

      if (m_sdf == null)
        return;

      Renderer.GenerateSdf(m_sdf, NoiseVolume.PointRs(NoiseVolume.transform.position), NoiseVolume.transform.localScale);
    }

    public void OnEnable()
    {
      if (m_sdf == null)
      {
        m_sdf = new RenderTexture(64, 16, 0, RenderTextureFormat.RFloat);
        m_sdf.dimension = TextureDimension.Tex3D;
        m_sdf.volumeDepth = 64;
        m_sdf.enableRandomWrite = true;
        m_sdf.Create();

        if (VFX != null)
        {
          VFX.SetTexture("Signed Distance Field", m_sdf);
        }
      }
    }

    public void OnDisable()
    {
      if (m_sdf != null)
      {
        m_sdf.Release();
        m_sdf = null;
      }
    }

    public void Update()
    {
      UpdateSdf();
    }
  }
}

