using UnityEngine;
using System.Collections.Generic;

namespace Scripts.Objects
{
    [CreateAssetMenu(fileName = "NewObjectCategory", menuName = "Spawning/Object Category")]
    public class ObjectCategory : ScriptableObject
    {
        public string categoryName;
        
        [Tooltip("Prefabs that belong to this category.")]
        public List<VariantEntry> variants;
    }
}