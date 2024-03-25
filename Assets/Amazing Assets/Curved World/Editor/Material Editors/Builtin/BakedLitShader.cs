using System;

using UnityEngine;

using AmazingAssets.CurvedWorldEditor;


namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class CurvedWorld_BakedLitShader : BaseShaderGUI
    {
        // Properties
        private BakedLitGUI.BakedLitProperties shadingModelProperties;


        //Curved World
        MaterialHeaderScopeList curvedWorldMaterialScope;


        public override void FillAdditionalFoldouts(MaterialHeaderScopeList materialScopesList)
        {
            base.FillAdditionalFoldouts(materialScopesList);


            //Curved World
            Material material = (Material)materialEditor.target;
            if (curvedWorldMaterialScope == null)
                curvedWorldMaterialScope = new MaterialHeaderScopeList();
            if (material.HasProperty("_CurvedWorldBendSettings"))
                curvedWorldMaterialScope.RegisterHeaderScope(new GUIContent("Curved World"), AmazingAssets.CurvedWorldEditor.MaterialProperties.Expandable.CurvedWorld, _ => AmazingAssets.CurvedWorldEditor.MaterialProperties.DrawCurvedWorldMaterialProperties(materialEditor, MaterialProperties.STYLE.None, false, false));
        }

        public override void OnGUI(MaterialEditor materialEditorIn, MaterialProperty[] properties)
        {
            if (materialEditorIn == null)
                throw new ArgumentNullException("materialEditorIn");

            materialEditor = materialEditorIn;
            Material material = materialEditor.target as Material;

            FindProperties(properties);   // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly

            // Make sure that needed setup (ie keywords/renderqueue) are set up if we're switching some existing
            // material to a universal shader.
            if (m_FirstTimeApply)
            {
                OnOpenGUI(material, materialEditorIn);
                m_FirstTimeApply = false;
            }


            //Curved World
            curvedWorldMaterialScope.DrawHeaders(materialEditor, material);


            ShaderPropertiesGUI(material);
        }

        // collect properties from the material properties
        public override void FindProperties(MaterialProperty[] properties)
        {
            base.FindProperties(properties);
            shadingModelProperties = new BakedLitGUI.BakedLitProperties(properties);


            //Curved World
            MaterialProperties.InitCurvedWorldMaterialProperties(properties);
        }

        // material changed check
        public override void ValidateMaterial(Material material)
        {
            SetMaterialKeywords(material);


            //Curved World
            MaterialProperties.SetKeyWords(material);
        }

        // material main surface options
        public override void DrawSurfaceOptions(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // Use default labelWidth
            EditorGUIUtility.labelWidth = 0f;

            base.DrawSurfaceOptions(material);
        }

        // material main surface inputs
        public override void DrawSurfaceInputs(Material material)
        {
            base.DrawSurfaceInputs(material);
            BakedLitGUI.Inputs(shadingModelProperties, materialEditor);
            DrawTileOffset(materialEditor, baseMapProp);
        }

        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // _Emission property is lost after assigning Standard shader to the material
            // thus transfer it before assigning the new shader
            if (material.HasProperty("_Emission"))
            {
                material.SetColor("_EmissionColor", material.GetColor("_Emission"));
            }

            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
            {
                SetupMaterialBlendMode(material);
                return;
            }

            SurfaceType surfaceType = SurfaceType.Opaque;
            BlendMode blendMode = BlendMode.Alpha;
            if (oldShader.name.Contains("/Transparent/Cutout/"))
            {
                surfaceType = SurfaceType.Opaque;
                material.SetFloat("_AlphaClip", 1);
            }
            else if (oldShader.name.Contains("/Transparent/"))
            {
                // NOTE: legacy shaders did not provide physically based transparency
                // therefore Fade mode
                surfaceType = SurfaceType.Transparent;
                blendMode = BlendMode.Alpha;
            }
            material.SetFloat("_Blend", (float)blendMode);

            material.SetFloat("_Surface", (float)surfaceType);
            if (surfaceType == SurfaceType.Opaque)
            {
                material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }
            else
            {
                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }
        }
    }
}
