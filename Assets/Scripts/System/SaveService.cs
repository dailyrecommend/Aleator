using UnityEngine;

namespace SystemLayer
{
    public static class SaveService
    {
        private const string KEY = "aleotor_save_exists";

        public static bool HasSave() => PlayerPrefs.GetInt(KEY, 0) == 1;

        public static void MarkExists(bool exists) => PlayerPrefs.SetInt(KEY, exists ? 1 : 0);

        public static void Load() { Debug.Log("[Save] load stub"); }   // TODO
    }
}
