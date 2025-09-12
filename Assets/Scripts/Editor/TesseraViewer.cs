#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public sealed class TesseraViewer : EditorWindow
    {
        [MenuItem("Aleotor/Tessera Viewer")]
        public static void Open() => GetWindow<TesseraViewer>("Tessera Viewer").Show();

        private Vector2 scroll;

        private void OnGUI()
        {
            var svc = FindAnyObjectByType<Data.TesseraService>();
            if (!svc)
            {
                if (GUILayout.Button("Create TesseraService"))
                {
                    var go = new GameObject("TesseraService");
                    go.AddComponent<Data.TesseraService>().LoadFromStreamingAssets();
                    Selection.activeGameObject = go;
                }
                return;
            }

            if (GUILayout.Button("Reload")) svc.LoadFromStreamingAssets();

            EditorGUILayout.Space();

            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (var r in svc.All)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField($"{r.id} [{r.rarity}]");
                EditorGUILayout.LabelField(r.name);
                EditorGUILayout.LabelField(r.desc, EditorStyles.wordWrappedLabel);
                EditorGUILayout.LabelField($"effectId: {r.effectId}");
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
#endif