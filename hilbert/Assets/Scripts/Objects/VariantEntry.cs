using UnityEngine;

namespace Scripts.Objects
{
    [System.Serializable]
    public class VariantEntry
    {
        public GameObject prefab;
        [Range(0, 100)] public float spawnWeight = 1f;
    }
}