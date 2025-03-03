using System.Linq;
using UnityEngine;

namespace _Core._Scripts.Utilities.Extensions
{
    public static class AnimatorExtension
    {
        public static void PlayAnimation(this Animator animator, string animationStateName,float crossFadeTime = 0, int layerIndex = 0) {
            PlayAnimation(animator, Animator.StringToHash(animationStateName), layerIndex);
        }
        public static void PlayAnimation(this Animator animator, int animationStateHash,float crossFadeTime = 0, int layerIndex = 0) {
            if (animator == null) {
                Debug.LogError("Animator is null");
                return;
            }
            if (IsAnimationPlaying(animator, animationStateHash, layerIndex)) {
                return;
            }
            if(crossFadeTime > 0) {
                animator.CrossFade(animationStateHash, crossFadeTime, layerIndex);
            } else {
                animator.Play(animationStateHash, layerIndex);
            }
        }
        public static bool IsAnimationPlaying(this Animator animator, string animationStateName, int layerIndex = 0) {
            return IsAnimationPlaying(animator, Animator.StringToHash(animationStateName), layerIndex);
        }

        public static bool IsAnimationPlaying(this Animator animator, int animationStateHash, int layerIndex = 0) {
            if (animator == null) {
                Debug.LogError("Animator is null");
                return false;
            }
            return animator.GetCurrentAnimatorStateInfo(layerIndex).shortNameHash == animationStateHash;
        }
        public static float GetAnimationClipLength(this Animator animator, string animationStateName) {
            return GetAnimationClipLength(animator, Animator.StringToHash(animationStateName));
        }
        public static float GetAnimationClipLength(this Animator animator, int animationClipHash) {
            if (animator == null) {
                Debug.LogError("Animator is null");
                return 0;
            }
            return animator.runtimeAnimatorController.animationClips
                .FirstOrDefault(c => Animator.StringToHash(c.name) == animationClipHash)?.length ?? 0;
        }
    }
}