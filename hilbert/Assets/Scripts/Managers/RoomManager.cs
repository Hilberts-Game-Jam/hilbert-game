using System;
using System.Collections.Generic;
using System.Numerics;
using System.Transactions;
using Scripts.Rooms;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Scripts.Core.ClassesAndEnums;
using Random = System.Random;

namespace Scripts.Managers
{
    public class RoomManager : MonoBehaviour
    {
    
        public static RoomManager Instance;
        public int roomNumber = 0;
        public RoomType roomType = RoomType.RANDOM_ROOM;
        public RoomLayout layout;
        public Vector2Int door_pos = new Vector2Int();
        public List<FurnitureObjectInfo> furniture_objects = new List<FurnitureObjectInfo>();

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

            GenerateNewRoom();

            // TODO: remove if not necessary for testing anymore
            //SceneManager.LoadScene("Room");

        }

        private void GenerateNewRoom()
        {

            furniture_objects = new List<FurnitureObjectInfo>();
            
            // TODO: Random Rooms, right now only test room
            Random random = new Random();
            layout = new RoomLayout(random.Next(7, 11), random.Next(7, 11));
            
            // Placing the door
            if (random.Next(0, 2) == 1)
            {

                if (random.Next(0, 2) == 1) door_pos.x = 0;
                else door_pos.x = layout.GetWidth() - 1;
                
                door_pos.y = random.Next(1, layout.GetHeight() - 1);
                
            }
            else
            {

                if (random.Next(0, 2) == 1) door_pos.y = 0;
                else door_pos.y = layout.GetHeight() - 1;
                
                door_pos.x = random.Next(1, layout.GetWidth() - 1);

            }
            
            layout.SetTileAt(door_pos.x, door_pos.y, TileType.DOOR);
            
            // Adding Objects based on weirdness level
            
            // Adding a bed
            if (random.Next(0, 2) == 1) // Double bed
            {

            }
            else // bed
            {
                
            }
            
        }

        private bool CheckObjectValidity(int x_s, int x_e, int y_s, int y_e)
        {

            for (int i = x_s; i < x_e; i++)
            {
                for (int j = y_s; j < y_e; j++)
                {
                    if (layout.GetTileAt(i, j) != TileType.FREE) return false;
                }
            }

            return true;

        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

