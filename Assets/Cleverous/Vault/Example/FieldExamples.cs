
using Cleverous.VaultSystem;
using UnityEngine;

namespace VaultExample
{
    public class FieldExamples : DataEntity
    {
        [Header("   ///// Default Fields")]
        public string StringDefault;

        public int IntegerDefault;
        public float FloatDefault;
        public Vector2 Vector2;
        public Vector3 Vector3;
        public Vector4 Vector4;
        public Rect Rect;
        public Bounds Bounds;
        public Color Color;
        public AnimationCurve AnimationCurve;
        public enum EnumTest {Alpha, Beta, Charlie, Delta, Echo}
        public EnumTest Enum;
        public LayerMask LayerMask;


        [Header("   //// Attribute Fields")]
        [Range(23, 84)]
        public int IntegerRange;
        [Range(0.34f, 0.983f)]
        public float FloatRange;
        [TextArea(1, 5)]
        public string StringTextArea;
        [Multiline]
        public string StringMultiline;

        [Space]
        [AssetDropdown(typeof(FieldExamples))]
        public FieldExamples AssetDropdownField1;

        [AssetDropdown(typeof(FieldExamples))]
        public FieldExamples AssetDropdownField2;

        [AssetDropdown(typeof(FieldExamples))]
        public FieldExamples AssetDropdownField3;

        [Space]
        [AssetDropdown(typeof(FieldExamples))]
        public FieldExamples[] AssetDropdownFieldArray = new FieldExamples[4];

        protected string Protected;
        protected string m_private;

        protected override void Reset()
        {
            base.Reset();
            Title = "Field Example : " + Random.Range(0, 98123);
            Description = int.MaxValue.ToString();
        }
    }
}