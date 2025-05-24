using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Objects
{
    [CreateAssetMenu(fileName = "NewObjectSpawnerConfig", menuName = "Spawning/Object Spawner Config")]
    public class ObjectSpawnerConfig : ScriptableObject
    {
        public List<ObjectCategory> objectCategories;
    }
}