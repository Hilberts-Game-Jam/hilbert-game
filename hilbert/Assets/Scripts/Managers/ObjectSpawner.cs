using System;
using Scripts.Objects;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using Events = Core.Events;
using Random = UnityEngine.Random;

namespace Scripts.Managers
{
    public class ObjectSpawner : MonoBehaviour
    {

        public static ObjectSpawner Instance;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
            
        }

        [Tooltip("Spawner config ScriptableObject")]
        public ObjectSpawnerConfig spawnerConfig;

        [SerializeField] private List<ObjectSpawnerConfig> _spawnerConfigs;

        private void OnEnable()
        {
            Events.OnWeirdnessChanged += OnWeirdnessChanged;
        }
        
        private void OnDisable()
        {
            Events.OnWeirdnessChanged -= OnWeirdnessChanged;
        }

        private void OnWeirdnessChanged(int newWeirdnessLevel)
        {
            spawnerConfig = _spawnerConfigs[newWeirdnessLevel - 1];
        }

        public GameObject GetRandomVariant(string categoryName)
        {
            if (spawnerConfig == null || spawnerConfig.objectCategories == null) return null;

            // Find the category SO by name
            ObjectCategory category = spawnerConfig.objectCategories.Find(c => c.categoryName == categoryName);
            if (category == null) return null;

            if (category.variants == null || category.variants.Count == 0) return null;

            return PickWeightedRandomVariant(category.variants);
        }
        
        private GameObject PickWeightedRandomVariant(List<VariantEntry> variants)
        {
            float totalWeight = 0f;
            foreach (var v in variants)
                totalWeight += v.spawnWeight;

            float randomValue = Random.Range(0f, totalWeight);
            float cumulative = 0f;

            foreach (var v in variants)
            {
                cumulative += v.spawnWeight;
                if (randomValue <= cumulative)
                    return v.prefab;
            }

            return null;
        }
    }
}

