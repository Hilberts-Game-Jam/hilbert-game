using UnityEngine;

namespace Scripts.Core
{
    public static class ClassesAndEnums
    {
        public enum RoomType
        {
            RANDOM_ROOM,
            SCRIPTED_ROOM
        }

        public enum TileType
        {
            FREE,
            BLOCKED,
        }
        
        public enum KeyObject
        {
            MIRROR_SHARD
        }
        
        public static class D20
        {
            public static int Roll()
            {
                return Random.Range(1, 21);
            }
        }
    }
}