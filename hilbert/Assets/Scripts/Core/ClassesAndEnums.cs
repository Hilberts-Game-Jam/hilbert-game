using UnityEngine;

namespace Scripts.Core
{
    public class ClassesAndEnums
    {
        public enum RoomType
        {
            RANDOM_ROOM,
            SCRIPTED_ROOM,
        }

        public enum TileType
        {
            FREE,
            BLOCKED,
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
        
        /// <summary>
        /// Used to specify the type of interaction that the player is performing.
        /// </summary>
        public enum InteractionType
        {
            NONE,
            DOOR,
            ITEM,
            WEIRDNESS25,
            WEIRDNESS50,
            WEIRDNESS100,
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