/******************************************************************************/
/*
  Project   - MudBun
  Publisher - Long Bunny Labs
              http://LongBunnyLabs.com
  Author    - Ming-Lun "Allen" Chou
              http://AllenChou.net
*/
/******************************************************************************/

using System.Collections.Generic;

using UnityEngine;

namespace MudBun
{
  public class MudTorus : MudSolid
  {
    [SerializeField] private float m_elongation = 0.0f;
    public float Elongation { get => m_elongation; set { m_elongation = value; MarkDirty(); } }

    public float Radius
    {
      get => Mathf.Abs(0.25f * transform.localScale.y);
      set
      {
        transform.localScale = new Vector3(transform.localScale.x, 4.0f * value, transform.localScale.z);
        MarkDirty();
      }
    }

    public override Aabb BoundsRaw
    {
      get
      {
        Vector3 posRs = PointRs(transform.position);
        Vector3 size = VectorUtil.Abs(transform.localScale);
        Vector3 r = new Vector3(0.5f * size.x, m_elongation, 0.5f * size.z) + Radius * Vector3.one;
        Aabb bounds = new Aabb(-r, r);
        bounds.Rotate(RotationRs(transform.rotation));
        Vector3 round = Radius * Vector3.one;
        bounds.Min += posRs;
        bounds.Max += posRs;
        return bounds;
      }
    }

    public override void SanitizeParameters()
    {
      base.SanitizeParameters();

      Validate.NonNegative(ref m_elongation);
    }

    public override int FillComputeData(SdfBrush [] aBrush, int iStart, List<Transform> aBone)
    {
      SdfBrush brush = SdfBrush.New;
      brush.Type = (int) SdfBrush.TypeEnum.Torus;
      brush.Radius = Radius;

      brush.Data0.x = m_elongation;

      if (aBone != null)
      {
        brush.BoneIndex = aBone.Count;
        aBone.Add(gameObject.transform);
      }

      aBrush[iStart] = brush;

      return 1;
    }

    public override void DrawSelectionGizmosRs()
    {
      base.DrawSelectionGizmosRs();

      GizmosUtil.DrawInvisibleTorus
      (
        PointRs(transform.position), 
        0.25f * transform.localScale.y, 
        transform.localScale.x, 
        transform.localScale.z, 
        RotationRs(transform.rotation)
      );
    }

    public override void DrawOutlineGizmosRs()
    {
      base.DrawOutlineGizmosRs();

      GizmosUtil.DrawWireTorus
      (
        PointRs(transform.position), 
        0.25f * transform.localScale.y, 
        transform.localScale.x, 
        transform.localScale.z, 
        RotationRs(transform.rotation)
      );
    }
  }
}

