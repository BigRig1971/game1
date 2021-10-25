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

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace MudBun
{
  public class MudCurveSimple : MudSolid
  {
    [Header("Shape")]
  
    [SerializeField] private float m_elongation = 0.0f;
    public float Elongation { get => m_elongation; set { m_elongation = value; MarkDirty(); } }

    public Transform PointA;
    public Transform ControlPoint;
    public Transform PointB;

    [SerializeField] private float m_radiusA = 0.2f;
    public float RadiusA { get => m_radiusA; set { m_radiusA = value; MarkDirty(); } }

    [SerializeField] private float m_radiusControlPoint = -1.0f;
    public float ControlPointRadius { get => m_radiusControlPoint; set { m_radiusControlPoint = value; MarkDirty(); } }

    [SerializeField] private float m_radiusB = 0.2f;
    public float RadiusB { get => m_radiusB; set { m_radiusB = value; MarkDirty(); } }

    [SerializeField] [Range(0.0f, 1.0f)] private float m_smoothStepBlend = 0.0f;
    public float SmoothStepBlend { get =>m_smoothStepBlend; set { m_smoothStepBlend = value; MarkDirty(); } }

    [Header("Noise")]

    [SerializeField] private bool m_enableNoise = false;
    [SerializeField] private float m_noiseOffset = 0.0f;
    [SerializeField] private Vector2 m_noiseBaseOctaveSize = 0.5f * Vector2.one;
    [SerializeField] [Range(0.0f, 1.0f)] private float m_noiseThreshold = 0.45f;
    [SerializeField] [Range(0.0f, 1.0f)] private float m_noiseThresholdFade = 0.0f;
    [SerializeField] [Range(-1.0f, 1.0f)] private float m_noiseThresholdCoreBias = 0.0f;
    [SerializeField] [Range(1, 3)] private int m_noiseNumOctaves = 2;
    [SerializeField] private float m_noiseOctaveOffsetFactor = 0.5f;
    [SerializeField] private float m_noiseTwist = 0.0f;
    [SerializeField] private float m_noiseTwistOffset = 0.0f;
    public bool EnableNoise { get => m_enableNoise; set { m_enableNoise = value; MarkDirty(); } }
    public float NoiseOffset { get => m_noiseOffset; set { m_noiseOffset = value; MarkDirty(); } }
    public Vector2 NoiseBaseOctaveSize { get => m_noiseBaseOctaveSize; set { m_noiseBaseOctaveSize = value; MarkDirty(); } }
    public float NoiseThreshold { get => m_noiseThreshold; set { m_noiseThreshold = value; MarkDirty(); } }
    public float NoiseThresholdFade { get => m_noiseThresholdFade; set { m_noiseThresholdFade = value; MarkDirty(); } }
    public float NoiseThresholdCoreBias { get => m_noiseThresholdCoreBias; set { m_noiseThresholdCoreBias = value; MarkDirty(); } }
    public int NoiseNumOctaves { get => m_noiseNumOctaves; set { m_noiseNumOctaves = value; MarkDirty(); } }
    public float NoiseOctaveOffsetFactor { get => m_noiseOctaveOffsetFactor; set { m_noiseOctaveOffsetFactor = value; MarkDirty(); } }
    public float NoiseTwist { get => m_noiseTwist; set { m_noiseTwist = value; MarkDirty(); } }
    public float NoiseTwistOffset { get => m_noiseTwistOffset; set { m_noiseTwistOffset = value; MarkDirty(); } }

    public override Aabb BoundsRaw
    {
      get
      {
        if (PointA == null || PointB == null || ControlPoint == null)
          return Aabb.Empty;

        Vector3 a = PointRs(PointA.position);
        Vector3 b = PointRs(PointB.position);
        Vector3 c = PointRs(ControlPoint.position);

        Vector3 r = Mathf.Max(m_radiusA, m_radiusB, m_radiusControlPoint) * Vector3.one;
        Aabb bounds = Aabb.Empty;
        bounds.Include(new Aabb(a - r, a + r));
        bounds.Include(new Aabb(b - r, b + r));
        bounds.Include(new Aabb(c - r, c + r));

        if (m_elongation != 0.0f)
        {
          Vector3 ab = b - a;
          Vector3 ac = c - a;
          Vector3 x = VectorUtil.NormalizeSafe(ab, VectorRs(transform.right));
          Vector3 z = VectorUtil.NormalizeSafe(Vector3.Cross(ab, ac), VectorRs(transform.forward));

          Vector3 e = m_elongation * VectorUtil.Abs(VectorRs(z));
          bounds.Min -= e;
          bounds.Max += e;
        }

        return bounds;
      }
    }

    public override void SanitizeParameters()
    {
      base.SanitizeParameters();

      Validate.NonNegative(ref m_elongation);

      Validate.NonNegative(ref m_radiusA);
      Validate.NonNegative(ref m_radiusB);

      Validate.NonNegative(ref m_noiseBaseOctaveSize);

    }

    private Transform[] m_aPoint = new Transform [] { null, null, null };
    private void Update()
    {
      if (m_aPoint[0] != PointA || m_aPoint[1] != PointB || m_aPoint[2] != ControlPoint)
        MarkDirty();

      m_aPoint[0] = PointA;
      m_aPoint[1] = PointB;
      m_aPoint[2] = ControlPoint;
      foreach (var p in m_aPoint)
      {
        if (p == null)
          return;

        if (!p.hasChanged)
          continue;

        MarkDirty();
        p.hasChanged = false;
      }
    }

    public Matrix4x4 m_basis;

    public override int FillComputeData(SdfBrush [] aBrush, int iStart, List<Transform> aBone)
    {
      if (PointA == null || PointB == null || ControlPoint == null)
        return 0;

      Vector3 a = PointA.position;
      Vector3 b = PointB.position;
      Vector3 c = ControlPoint.position;
      Vector3 d = 0.5f * (a + b);
      Vector3 ab = b - a;
      Vector3 ac = c - a;
      Vector3 x = VectorUtil.NormalizeSafe(ab, transform.right);
      Vector3 z = VectorUtil.NormalizeSafe(Vector3.Cross(ab, ac), transform.forward);
      Vector3 y = VectorUtil.NormalizeSafe(Vector3.Cross(z, x), transform.up);
      m_basis = Matrix4x4.TRS(d, RotationRs(Quaternion.LookRotation(z, y)), Vector3.one);
      Matrix4x4 basisInv = m_basis.inverse;
      a = basisInv.MultiplyPoint(a);
      b = basisInv.MultiplyPoint(b);
      c = basisInv.MultiplyPoint(c);

      int iBrush = iStart;
      SdfBrush brush = SdfBrush.New;

      brush.Type = (int) SdfBrush.TypeEnum.CurveSimple;
      brush.Data0 = new Vector4(a.x, a.y, a.z, m_radiusA);
      brush.Data1 = new Vector4(b.x, b.y, b.z, m_radiusB);
      brush.Data2 = new Vector4(c.x, c.y, c.z, m_enableNoise ? 1.0f : 0.0f);
      brush.Data3 = new Vector4(m_elongation, m_radiusControlPoint, m_smoothStepBlend, 0.0f);

      if (aBone != null)
      {
        brush.BoneIndex = aBone.Count;
        aBone.Add(PointA);
        aBone.Add(PointB);
        aBone.Add(ControlPoint);
      }

      aBrush[iBrush++] = brush;

      if (m_enableNoise)
      {
        brush.Type = (int) SdfBrush.TypeEnum.Nop;
        brush.Data0 = new Vector4(m_noiseBaseOctaveSize.x, m_noiseBaseOctaveSize.y, m_noiseBaseOctaveSize.y, m_noiseThreshold);
        brush.Data1 = new Vector4(m_noiseOffset, 0.0f, 0.0f, m_noiseNumOctaves);
        brush.Data2 = new Vector4(m_noiseOctaveOffsetFactor, MathUtil.TwoPi * (m_noiseTwistOffset), MathUtil.TwoPi * (m_noiseTwistOffset + m_noiseTwist), 0.0f);
        brush.Data3 = new Vector4(m_noiseThresholdFade, m_noiseThresholdCoreBias, 0.0f, 0.0f);
        aBrush[iBrush++] = brush;
      }

      return iBrush - iStart;
    }

    public override void FillBrushData(ref SdfBrush brush, int iBrush)
    {
      base.FillBrushData(ref brush, iBrush);

      brush.Position = PointRs(m_basis.MultiplyPoint(Vector3.zero));
      brush.Rotation = RotationRs(m_basis.rotation);
    }

    internal override bool IsSelected()
    {
      if (base.IsSelected())
        return true;

      #if UNITY_EDITOR
      if (PointA?.gameObject != null && Selection.Contains(PointA.gameObject))
        return true;

      if (PointB?.gameObject != null && Selection.Contains(PointB.gameObject))
        return true;

      if (ControlPoint?.gameObject != null && Selection.Contains(ControlPoint.gameObject))
        return true;
      #endif

      return false;
    }

    public override void DrawSelectionGizmosRs()
    {
      base.DrawSelectionGizmosRs();

      if (PointA == null || PointB == null || ControlPoint == null)
        return;

      Vector3 a = PointRs(PointA.position);
      Vector3 b = PointRs(PointB.position);
      Vector3 c = PointRs(ControlPoint.position);

      int n = 8;
      float t = 0.0f;
      float dt = 1.0f / (n - 1);
      for (int i = 0; i < n; ++i)
      {
        GizmosUtil.DrawInvisibleSphere(VectorUtil.BezierQuad(a, b, c, t), Mathf.Lerp(m_radiusA, m_radiusB, t), Vector3.one, Quaternion.identity);
        t += dt;
      }
    }

    public override void DrawOutlineGizmosRs()
    {
      base.DrawOutlineGizmosRs();

      if (PointA != null)
      {
        GizmosUtil.DrawWireSphere(PointRs(PointA.position), m_radiusA, Vector3.one, RotationRs(PointA.rotation));
      }

      if (PointB != null)
      {
        GizmosUtil.DrawWireSphere(PointRs(PointB.position), m_radiusB, Vector3.one, RotationRs(PointB.rotation));
      }

      if (ControlPoint != null)
      {
        float da = (ControlPoint.position - PointA.position).magnitude;
        float db = (ControlPoint.position - PointB.position).magnitude;
        float r = m_radiusControlPoint >= 0.0f ? m_radiusControlPoint : Mathf.Lerp(m_radiusA, m_radiusB, da / (da + db));
        GizmosUtil.DrawWireSphere(PointRs(ControlPoint.position), r, Vector3.one, RotationRs(ControlPoint.rotation));
      }

      if (PointA != null && PointB != null && ControlPoint != null)
      {
        GizmosUtil.DrawBezierQuad(PointRs(PointA.position), PointRs(PointB.position), PointRs(ControlPoint.position));
      }
    }
  }
}

