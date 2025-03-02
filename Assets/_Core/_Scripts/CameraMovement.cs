using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities.Extensions;
using Random = UnityEngine.Random;

namespace _Core._Scripts
{
    //easy if using cinemachine. but this is a simple camera movement script
    public class CameraMovement : MonoBehaviour
    {
        public float distance;
        public float height;
        public float smoothness;
        public float rotationForce;
        public float zoomHeightMultiplier;

        public Transform camTarget;
	
        Vector3 velocity;

        private void LateUpdate() {
            if(!camTarget)
                return;
		
            Vector3 angle = transform.localEulerAngles;
		
            if(angle.x < 37){
                angle.x += Time.deltaTime * 40;
                transform.localEulerAngles = angle;
            }
		
            Vector3 pos = Vector3.zero;
            pos.x = camTarget.position.x;
            pos.y = camTarget.position.y + height;
            //pos.y = transform.position.y;
            pos.z = camTarget.position.z - distance;
		
            transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smoothness);
		
            Vector3 rotation = transform.eulerAngles;
            rotation.y = -180 + (camTarget.position.x * rotationForce);
            transform.eulerAngles = rotation;
        }

        public IEnumerator Shake(float duration, float amount){
            float elapsed = 0f;
		
            while(elapsed < duration){
                float x = Random.Range(-1f, 1f) * amount;
                float y = Random.Range(-1f, 1f) * amount;
                float z = Random.Range(-1f, 1f) * amount;
			
                transform.position += new Vector3(x, y, z);
			
                elapsed += Time.deltaTime;
			
                yield return 0;
            }
        }
        
        public void Zoom(bool zoomIn) {
            if(zoomIn){
                height *= zoomHeightMultiplier;
            }
            else{
                height *= (1f/zoomHeightMultiplier);
            }
        }

        public void SwitchTargetTemp(Transform newTarget, float duration, float smooth) {
            StartCoroutine(Switch(newTarget, duration, smooth));
        }
        IEnumerator Switch(Transform newTarget, float duration, float smooth){
            Transform original = camTarget;
            float originalSmoothness = this.smoothness;
		
            camTarget = newTarget;
            smoothness = smooth;
		
            yield return new WaitForSeconds(duration);
		
            camTarget = original;
		
            yield return new WaitForSeconds(1f);
		
            smoothness = originalSmoothness;
        }
    }
}