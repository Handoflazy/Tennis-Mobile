using System;
using DG.Tweening;
using UnityEngine;

namespace SVS.UI
{
    public class UISpinner : MonoBehaviour
    {
        [SerializeField] private float RotateSpeed;
        RectTransform rectTransform;

        private void Start() {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update() {
            rectTransform.Rotate(0f, 0f, RotateSpeed * Time.deltaTime);
        }
    }
}