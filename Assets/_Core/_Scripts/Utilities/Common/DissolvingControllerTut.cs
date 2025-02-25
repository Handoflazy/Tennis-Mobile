using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

namespace Platformer.Common
{
    public class DissolvingControllerTut : MonoBehaviour
    {
        public SkinnedMeshRenderer[] skinnedMeshes;
        public VisualEffect visualEffect;
        public float DissolveRate = 0.0125f;
        public float refreshRate = 0.025f;
        
        [Button("Test")]
        public void Dissolve() {
            visualEffect?.Play();
            foreach (var skin in skinnedMeshes)
            {
                StartCoroutine(DissolveCoroutine(skin));
            }
        }

        private void OnEnable() {
            foreach (var skinnedMesh in skinnedMeshes)
            {
                Material[] skinnedMaterials = skinnedMesh.materials;
                if(skinnedMaterials.Length > 0)
                {
                    foreach (var material in skinnedMaterials)
                    {
                        material.SetFloat("_DissolveAmount", 0);
                    }
                }
            }
            
        }

        IEnumerator DissolveCoroutine(SkinnedMeshRenderer skinnedMeshRenderer) {
            Material[] skinnedMaterials = skinnedMeshRenderer.materials;
            if(skinnedMaterials.Length > 0)
            {
                float counter = 0;
                while (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
                {
                    counter += DissolveRate;
                    foreach (var material in skinnedMaterials)
                    {
                        material.SetFloat("_DissolveAmount", counter);
                    }

                    yield return new WaitForSeconds(refreshRate);
                }
            }
        }
    }
}