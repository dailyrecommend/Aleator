#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    // 에디터 전용 자동 리로드(StreamingAssets 변경 감지)
    [InitializeOnLoad]
    public static class CategoryWatcher
    {
        private static FileSystemWatcher _watcher;

        static CategoryWatcher()
        {
            var root = Path.Combine(Application.streamingAssetsPath, "Data");
            if (!Directory.Exists(root)) return;

            _watcher = new FileSystemWatcher(root, "categories.csv");
            _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName;
            _watcher.Changed += OnChanged;
            _watcher.Created += OnChanged;
            _watcher.Renamed += OnChanged;
            _watcher.EnableRaisingEvents = true;
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            EditorApplication.delayCall += () =>
            {
                var asset = Resources.FindObjectsOfTypeAll<Data.CategoryService>();
                foreach (var a in asset) a.LoadFromStreamingAssets();

                Debug.Log("categories.csv reloaded");
            };
        }
    }
}
#endif