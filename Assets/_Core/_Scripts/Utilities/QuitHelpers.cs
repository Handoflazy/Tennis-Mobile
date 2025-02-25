using UnityEngine;
namespace Platformer._Scripts.Utilities
{
    public static class QuitHelpers
    {
        public static void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}