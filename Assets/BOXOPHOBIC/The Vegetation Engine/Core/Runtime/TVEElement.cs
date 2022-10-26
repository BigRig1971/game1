// Cristian Pop - https://boxophobic.com/

using UnityEngine;
using Boxophobic.StyledGUI;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheVegetationEngine
{
    public enum ElementVisibility
    {
        UseGlobalVolumeSettings = -1,
        AlwaysHidden = 0,
        AlwaysVisible = 10,
        HiddenAtRuntime = 20,
    }

    [HelpURL("https://docs.google.com/document/d/145JOVlJ1tE-WODW45YoJ6Ixg23mFc56EnB_8Tbwloz8/edit#heading=h.fd5y8rbb7aia")]
    [ExecuteInEditMode]
    [AddComponentMenu("BOXOPHOBIC/The Vegetation Engine/TVE Element")]
    public class TVEElement : StyledMonoBehaviour
    {
        [StyledBanner(0.890f, 0.745f, 0.309f, "Element")]
        public bool styledBanner;

#if UNITY_EDITOR
        [StyledMessage("Error", "The current element is outside the Global Volume / Follow Main Camera Volume or the material is missing the ElementType tag and it is not rendered! Activate the scene Gizmos to see the volume bounds!", 0, 10)]
        public bool showValidMessage;
#endif

        [Tooltip("Sets the element visibility.")]
        public ElementVisibility customVisibility = ElementVisibility.UseGlobalVolumeSettings;
        [Tooltip("Sets a custom material for element rendering.")]
        public Material customMaterial;

        [HideInInspector]
        public TVEElementMaterialData materialData;
        [HideInInspector]
        public TVEElementDefaultData elementData;

        Renderer meshRenderer;
        Material renderMaterial;
        new ParticleSystem particleSystem;

        int useVertexColorDirection = 0;
        int useRaycastFading = 0;
        Vector3 lastPosition;

        LayerMask raycastMask;
        float raycastEnd = 0;

        bool isSelected;
        bool isValid;

        void OnEnable()
        {
            meshRenderer = gameObject.GetComponent<Renderer>();

            if (customMaterial != null)
            {
                renderMaterial = customMaterial;
            }
            else
            {
                renderMaterial = meshRenderer.sharedMaterial;
            }

            particleSystem = gameObject.GetComponent<ParticleSystem>();

            if (renderMaterial == null || renderMaterial.name == TVEConstants.ElementName)
            {
                if (materialData == null)
                {
                    materialData = new TVEElementMaterialData();
                }

                if (materialData.shader == null)
                {
#if UNITY_EDITOR
                    renderMaterial = new Material(Resources.Load<Material>("Internal Colors"));
                    SaveMaterialData(renderMaterial);
#endif
                }
                else
                {
                    renderMaterial = new Material(materialData.shader);
                    LoadMaterialData(renderMaterial);
                }

                renderMaterial.name = TVEConstants.ElementName;
                gameObject.GetComponent<Renderer>().sharedMaterial = renderMaterial;
            }

            if (renderMaterial != null)
            {
                TVEUtils.SetElementSettings(renderMaterial);

                GetMaterialParameters();
            }

            elementData = TVEUtils.CreateElementData(gameObject, renderMaterial, this.GetHashCode(), customVisibility);

            TVEUtils.AddElementDataToVolume(elementData);
            TVEUtils.SetElementVisibility(elementData, customVisibility);
        }

        void OnDestroy()
        {
            TVEUtils.RemoveElementDataFromVolume(elementData);
        }

        void OnDisable()
        {
            TVEUtils.RemoveElementDataFromVolume(elementData);
        }

        void Update()
        {
            if (TVEManager.Instance == null || elementData == null)
            {
                return;
            }

#if UNITY_EDITOR
            if (Selection.Contains(gameObject))
            {
                isSelected = true;
            }
            else
            {
                isSelected = false;
            }

            if (isSelected)
            {
                if (customMaterial != null)
                {
                    renderMaterial = customMaterial;
                }
                else
                {
                    renderMaterial = meshRenderer.sharedMaterial;
                }

                GetMaterialParameters();

                if (!EditorUtility.IsPersistent(renderMaterial))
                {
                    SaveMaterialData(renderMaterial);
                }

                TVEUtils.RemoveElementDataFromVolume(elementData);

                elementData = TVEUtils.CreateElementData(gameObject, renderMaterial, this.GetHashCode(), customVisibility);

                TVEUtils.AddElementDataToVolume(elementData);
                TVEUtils.SetElementVisibility(elementData, customVisibility);

                TVEManager.Instance.globalVolume.SortElementObjects();

                // Needed when gizmos is not enabled
                isValid = TVEUtils.IsValidElement(elementData);

                if (isValid)
                {
                    showValidMessage = false;
                }
                else
                {
                    showValidMessage = true;
                }

            }
#endif

            if (particleSystem != null)
            {
                var particleModule = particleSystem.main;
                var particleColor = particleModule.startColor.color;

                if (useVertexColorDirection > 0)
                {
                    var direction = transform.position - lastPosition;
                    var localDirection = transform.InverseTransformDirection(direction);
                    var worldDirection = transform.TransformVector(localDirection);
                    lastPosition = transform.position;

                    var worldDirectionX = Mathf.Clamp01(worldDirection.x * 10 * 0.5f + 0.5f);
                    var worldDirectionZ = Mathf.Clamp01(worldDirection.z * 10 * 0.5f + 0.5f);

                    particleColor = new Color(worldDirectionX, worldDirectionZ, particleColor.b, particleColor.a);
                }

                if (useRaycastFading > 0)
                {
                    var fade = GetRacastFading();
                    particleColor = new Color(particleColor.r, particleColor.g, particleColor.b, fade);
                }
                else
                {
                    particleColor = new Color(particleColor.r, particleColor.g, particleColor.b, particleColor.a);
                }

                particleModule.startColor = particleColor;
            }
            else
            {
                if (useRaycastFading > 0)
                {
                    var fade = GetRacastFading();

                    elementData.fadeValue = fade;
                }
                else
                {
                    elementData.fadeValue = 1.0f;
                }
            }
        }


#if UNITY_EDITOR
        void SaveMaterialData(Material material)
        {
            materialData = new TVEElementMaterialData();
            materialData.props = new List<TVEElementPropertyData>();

            materialData.shader = material.shader;

            for (int i = 0; i < ShaderUtil.GetPropertyCount(material.shader); i++)
            {
                var type = ShaderUtil.GetPropertyType(material.shader, i);
                var prop = ShaderUtil.GetPropertyName(material.shader, i);

                if (type == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    var propData = new TVEElementPropertyData();
                    propData.type = PropertyType.Texture;
                    propData.prop = prop;
                    propData.texture = material.GetTexture(prop);

                    materialData.props.Add(propData);
                }

                if (type == ShaderUtil.ShaderPropertyType.Vector || type == ShaderUtil.ShaderPropertyType.Color)
                {
                    var propData = new TVEElementPropertyData();
                    propData.type = PropertyType.Vector;
                    propData.prop = prop;
                    propData.vector = material.GetVector(prop);

                    materialData.props.Add(propData);
                }

                if (type == ShaderUtil.ShaderPropertyType.Float || type == ShaderUtil.ShaderPropertyType.Range)
                {
                    var propData = new TVEElementPropertyData();
                    propData.type = PropertyType.Value;
                    propData.prop = prop;
                    propData.value = material.GetFloat(prop);

                    materialData.props.Add(propData);
                }
            }
        }
#endif

        void LoadMaterialData(Material material)
        {
            material.shader = materialData.shader;

            for (int i = 0; i < materialData.props.Count; i++)
            {
                if (materialData.props[i].type == PropertyType.Texture)
                {
                    material.SetTexture(materialData.props[i].prop, materialData.props[i].texture);
                }

                if (materialData.props[i].type == PropertyType.Vector)
                {
                    material.SetVector(materialData.props[i].prop, materialData.props[i].vector);
                }

                if (materialData.props[i].type == PropertyType.Value)
                {
                    material.SetFloat(materialData.props[i].prop, materialData.props[i].value);
                }
            }
        }

        void GetMaterialParameters()
        {
            if (renderMaterial.HasProperty(TVEConstants.ElementDirectionMode))
            {
                useVertexColorDirection = renderMaterial.GetInt(TVEConstants.ElementDirectionMode);
            }

            if (renderMaterial.HasProperty(TVEConstants.ElementRaycastMode))
            {
                useRaycastFading = renderMaterial.GetInt(TVEConstants.ElementRaycastMode);
                raycastMask = renderMaterial.GetInt(TVEConstants.RaycastLayerMask);
                raycastEnd = renderMaterial.GetInt(TVEConstants.RaycastDistanceEndValue);
            }
        }

        float GetRacastFading()
        {
            raycastEnd = renderMaterial.GetInt(TVEConstants.RaycastDistanceEndValue);

            RaycastHit hit;
            bool raycastHit = Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, raycastMask);

            if (hit.distance > 0)
            {
                return 1 - Mathf.Clamp01(hit.distance / raycastEnd);
            }
            else
            {
                return 0;
            }
        }

        void OnDrawGizmosSelected()
        {
            DrawGizmos(true);
        }

        void OnDrawGizmos()
        {
            DrawGizmos(false);
        }

        void DrawGizmos(bool selected)
        {
            if (TVEManager.Instance == null || elementData == null)
            {
                return;
            }

            var genericColor = new Color(0.0f, 0.0f, 0.0f, 0.1f);
            var invalidColor = new Color(1.0f, 0.0f, 0.0f, 0.1f);

            if (selected)
            {
                genericColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                //genericColor = new Color(0.890f, 0.745f, 0.309f, 1.0f);
                invalidColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }

            isValid = TVEUtils.IsValidElement(elementData);

            if (isValid)
            {
                Gizmos.color = genericColor;
            }
            else
            {
                Gizmos.color = invalidColor;
            }

            if (isSelected)
            {
                if (useRaycastFading > 0)
                {
                    Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - raycastEnd, transform.position.z));
                }

                if (elementData.mesh != null)
                {
                    if (renderMaterial.shader.name.Contains("Control") || renderMaterial.shader.name.Contains("Direction") || renderMaterial.shader.name.Contains("Turbulence"))
                    {
                        Gizmos.DrawLine(new Vector3(transform.position.x + transform.forward.x * transform.lossyScale.x * 0.1f, transform.position.y, transform.position.z + transform.forward.z * transform.lossyScale.x * 0.1f), new Vector3(transform.position.x + transform.forward.x * transform.lossyScale.x * 0.3f, transform.position.y, transform.position.z + transform.forward.z * transform.lossyScale.x * 0.3f));
                    }
                }
            }

            Bounds gizmoBounds;

            if (elementData.mesh != null)
            {
                gizmoBounds = elementData.mesh.bounds;
                Gizmos.matrix = transform.localToWorldMatrix;
            }
            else
            {
                gizmoBounds = elementData.renderer.bounds;
            }

            Gizmos.DrawWireCube(gizmoBounds.center, gizmoBounds.size);
        }
    }
}
