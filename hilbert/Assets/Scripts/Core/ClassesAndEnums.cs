using UnityEngine;

namespace Scripts.Core
{
    public static class ClassesAndEnums
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
        
        /// <summary>
        /// Used to specify the type of interaction that the player is performing.
        /// </summary>
        public enum InteractionType
        {
            NONE,
            DOOR,
            ITEM,
            ELEVATOR,
        }
        
        public enum KeyObjectType
        {
            //TODO: Add more key object types as needed
        }

        public class FurnitureObjectInfo
        {

            private int _x_start;
            private int _y_start;
            private int _x_end;
            private int _y_end;
            private string _name;
            
            public FurnitureObjectInfo(int x_start, int x_end, int y_start, int y_end, string name)
            {
                _x_start = x_start;
                _y_start = y_start;
                _x_end = x_end;
                _y_end = y_end;
                _name = name;
            }
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