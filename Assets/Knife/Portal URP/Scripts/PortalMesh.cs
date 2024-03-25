using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knife.Portal
{
    [RequireComponent(typeof(Renderer))]
    public class PortalMesh : MonoBehaviour
    {
        [SerializeField] private string viewTextureName = "_MainTex";
        [SerializeField] private PortalView view;
        [SerializeField] private int materialId;
        [SerializeField] private PortalTransition otherPortalTransition;
        [SerializeField] private BoxCollider meshCollider;

        private Renderer attachedRenderer;

        public PortalTransition OtherPortalTransition { get => otherPortalTransition; set => otherPortalTransition = value; }
        public BoxCollider MeshCollider { get => meshCollider; set => meshCollider = value; }

        private void Start()
        {
            attachedRenderer = GetComponent<Renderer>();
            view.OnRenderTextureChanged += OnRenderTextureChanged;
        }

        private void OnRenderTextureChanged()
        {
            var materials = attachedRenderer.materials;
            materials[materialId].SetTexture(viewTextureName, view.RenderTexture);
            attachedRenderer.materials = materials;
        }
    }
}