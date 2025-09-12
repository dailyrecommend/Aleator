using UnityEngine;

namespace UI.Common
{
    public sealed class Popup : MonoBehaviour
    {
        [SerializeField] private GameObject root;

        public bool IsOpen => root && root.activeSelf;

        public void Open() { if (root) root.SetActive(true); }

        public void Close() { if (root) root.SetActive(false); }
    }
}