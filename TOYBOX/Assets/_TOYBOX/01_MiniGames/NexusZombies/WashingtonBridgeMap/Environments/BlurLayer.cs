using UnityEngine;

namespace ToyBox
{
    public class BlurLayer : MonoBehaviour
    {
        [SerializeField] private float blurAmount = 0f;
        [SerializeField] private float diameter = 0f;
        [SerializeField] private MeshRenderer meshRenderer;

        // Add IsInEditMode and IsInitialized properties
        public bool IsInEditMode { get; set; }
        public bool IsInitialized { get; set; }

        public float BlurAmount
        {
            get => blurAmount;
            set => blurAmount = value;
        }

        public float Diameter
        {
            get => diameter;
            set
            {
                diameter = value;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, diameter);
        }

        public void SetValues()
        {
            if (meshRenderer != null)
            {
                meshRenderer.material.SetFloat("_BlurAmount", blurAmount);
            }
            transform.localScale = Vector3.one * diameter;
        }
    }
}
