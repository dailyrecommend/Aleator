#if UNITY_EDITOR
using Data;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public sealed class CategoryViewer : EditorWindow
    {
        [MenuItem("Aleotor/Category Viewer")]
        public static void Open()
        {
            GetWindow<CategoryViewer>("Category Viewer").Show();
        }

        private Vector2 _scroll;

        private void OnGUI()
        {
            var svc = FindAnyObjectByType<Data.CategoryService>();
            if (!svc)
            {
                if (GUILayout.Button("Create Runtime CategoryService"))
                {
                    var go = new GameObject("CategoryService");
                    go.AddComponent<CategoryService>().LoadFromStreamingAssets();
                    Selection.activeGameObject = go;
                }
                return;
            }

            if (GUILayout.Button("Reload")) svc.LoadFromStreamingAssets();

            EditorGUILayout.Space();

            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            foreach (var r in svc.All)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField($"{r.name}  L{r.level}");
                EditorGUILayout.LabelField($"base: chips {r.baseChips}, mult {r.baseMultiplier}");
                EditorGUILayout.LabelField($"+/lvl: chips {r.chipsPerLevel}, mult {r.multiplierPerLevel}");
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
#endif