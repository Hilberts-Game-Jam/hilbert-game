using UnityEngine;

namespace Scripts.Core
{
    public class ClassesAndEnums
    {
        public enum RoomType
        {
            RANDOM_ROOM,
            SCRIPTED_ROOM,
            CORRIDOR
        }

        public enum TileType
        {
            FREE,
            BLOCKED,
            DOOR,
            WALL
        }

        public enum ObjectOverlayType
        {
            BLOODSTAINED,
            BURNED,
            BROKEN,
            NONE
        }
        
        public enum ObjectType
        {
            BED,
            SHELF,
            TABLE,
            CHAIR,
            MISC,
            CLOTHES
        }

        public enum KeyObjectType
        {
            //TODO: Add more key object types as needed
        }
        
        public class D20
        {
            public static int Roll()
            {
                return Random.Range(1, 21);
            }
        }
    }
}