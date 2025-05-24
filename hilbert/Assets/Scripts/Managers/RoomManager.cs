using System;
using Scripts.Rooms;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Scripts.Core.ClassesAndEnums;

namespace Scripts.Managers
{
    public class RoomManager : MonoBehaviour
    {
    
        public static RoomManager Instance;
        public int roomNumber = 0;
        public RoomType roomType = RoomType.RANDOM_ROOM;
        public RoomLayout layout;

        private void Awake()
        {
            // Creating Singleton Instance
            if (Instance)
            {
                Debug.Log("RoomManager instance already exists");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Setting RoomManager instance");
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
            // TODO: Random Rooms, right now only test room
            layout = new RoomLayout(11, 10);
            layout.SetTileAt(1, 1, TileType.DOOR);
            layout.SetTileAt(2, 2, TileType.BLOCKED);
            layout.SetTileAt(3, 2, TileType.BLOCKED);

            SceneManager.LoadScene("Room");

        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

