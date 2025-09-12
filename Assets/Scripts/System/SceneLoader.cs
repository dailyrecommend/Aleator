using UnityEngine;
using UnityEngine.SceneManagement;

namespace SystemLayer
{
    public static class SceneLoader
    {
        public static void Load(string scene) { SceneManager.LoadScene(scene); }

        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}