using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Transactions;

using Scripts.Rooms;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Rendering;
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
        public Vector2Int spawn_pos = new Vector2Int();
        public List<FurnitureObjectInfo> furniture_objects = new List<FurnitureObjectInfo>();
        private string[] possibleObjects = new[] {"Chest", "Couch", "Shelf", "Suitcase", "Table"};

        private const double WALKABLE_THRESHOLD = 0.1;

        private async Task Awake()
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
                await Task.Delay(2000);
                CorridorManager.Instance.GenerateInitialCorridor();
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

            spawn_pos = door_pos;

            GenerateNewRoom();

            // TODO: remove if not necessary for testing anymore
            // SceneManager.LoadScene("Room");

        }

        public void GenerateNewRoom()
        {

            Debug.Log("Generating Room");
            
            furniture_objects = new List<FurnitureObjectInfo>();
            
            // TODO: Random Rooms, right now only test room
            Random random = new Random(Guid.NewGuid().GetHashCode());
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

            spawn_pos = door_pos;
            
            if (door_pos.x == 0) spawn_pos.x++;
            else if (door_pos.x == layout.GetWidth() - 1) spawn_pos.x--;
            else if (door_pos.y == 0) spawn_pos.y++;
            else spawn_pos.y--;
            
            layout.SetTileAt(door_pos.x, door_pos.y, TileType.DOOR);
            
            // Adding Objects based on weirdness level
            
            // Adding a bed
            if (random.Next(0, 2) == 1) // Double bed
            {

                for (int i = 0; i < 10; i++) // Try 10 times
                {
                    FurnitureObjectInfo newObstacle = TryPlaceObject("DoubleBed");
                    if (newObstacle != null)
                    {
                        furniture_objects.Add(newObstacle);
                        break;
                    }
                }
            }
            else // bed
            {
                for (int i = 0; i < 10; i++) // Try 10 times
                {
                    FurnitureObjectInfo newObstacle = TryPlaceObject("Bed");
                    if (newObstacle != null)
                    {
                        furniture_objects.Add(newObstacle);
                        break;
                    }
                }
                
            }
            
            // Adding other random furniture
            int amountObj = random.Next(2, 8);

            for (int i = 0; i < amountObj; i++)
            {

                for (int j = 0; j < 10; j++) // Try 10 times
                {
                    
                    // Select a random category
                    int categoryInd = random.Next(0, 5);
                    FurnitureObjectInfo newFurniture = TryPlaceObject(possibleObjects[categoryInd]);

                    if (newFurniture != null)
                    {
                        furniture_objects.Add(newFurniture);
                        break;
                    }

                }
                
            }

        }

        private FurnitureObjectInfo TryPlaceObject(string name)
        {

            int width = 0;
            int height = 0;
            
            // Step 1: Define the size of the tile
            switch (name)
            {
                case "DoubleBed":
                case "Chest":
                    width = 2;
                    height = 2;
                    break;
                case "Bed":
                case "Shelf":
                    width = 2;
                    height = 1;
                    break;
                case "Couch":
                case "Suitcase":
                case "Table":
                    width = 1;
                    height = 1;
                    break;
                default: return null;
                    break;
            }
            
            // Step 2: Select a random tile
            int x_start = new Random().Next(1, layout.GetWidth() - width - 1);
            int x_end = x_start + width;
            int y_start = new Random().Next(1, layout.GetHeight() - height - 1);
            int y_end = y_start + height;
            
            // Step 3: Validate the area
            if (!CheckObjectValidity(x_start, x_end, y_start, y_end)) return null;
            
            // Step 4: Check if all free fields are accessible (kill me)
            // Copy the layout
            RoomLayout dummyLayout = new RoomLayout(layout.GetWidth(), layout.GetHeight());

            for (int i = 0; i < dummyLayout.GetWidth(); i++)
            {
                for (int j = 0; j < dummyLayout.GetHeight(); j++)
                {
                    dummyLayout.SetTileAt(i, j, layout.GetTileAt(i, j));
                }
            }
            
            for (int i = x_start; i < x_end; i++)
            {
                for (int j = y_start; j < y_end; j++)
                {
                    dummyLayout.SetTileAt(i, j, TileType.BLOCKED);
                }
            }

            if (!CheckIfAreaIsReachable(dummyLayout))
            {
                return null;
            }
            else
            {
                
                // Add to layout and return the object
                for (int i = x_start; i < x_end; i++)
                {
                    for (int j = y_start; j < y_end; j++)
                    {
                        layout.SetTileAt(i, j, TileType.BLOCKED);
                    }
                }
                return new FurnitureObjectInfo(x_start, x_end, y_start, y_end, name);
                
            }

            return null;

        }

        private bool CheckIfAreaIsReachable(RoomLayout thisLayout)
        {

            // If spawnpoint is blocked area is invalid
            if (thisLayout.GetTileAt(spawn_pos.x, spawn_pos.y) != TileType.FREE) return false;

            HashSet<Vector2Int> currentTiles;
            HashSet<Vector2Int> seenTiles;

            int freeTiles = 0;
            int reachableTiles = 0;
            
            for (int i = 1; i < thisLayout.GetWidth() - 1; i++)
            {
                for (int j = 1; j < thisLayout.GetHeight() - 1; j++)
                {

                    if (thisLayout.GetTileAt(i, j) != TileType.FREE) continue;
                    if (i == spawn_pos.x && j == spawn_pos.y)
                    {
                        freeTiles++;
                        reachableTiles++;
                        continue;
                    }

                    currentTiles = new HashSet<Vector2Int>();
                    seenTiles = new HashSet<Vector2Int>();
                    currentTiles.Add(new Vector2Int(i, j));

                    // Check if the currentTiles are trapped or not
                    while (currentTiles.Count > 0)
                    {

                        Vector2Int thisElem = currentTiles.ElementAt(0);
                        seenTiles.Add(thisElem);
                        
                        // Check if neighbors are free or the result
                        if (thisLayout.CoordsExist(thisElem.x - 1, thisElem.y) && thisLayout.GetTileAt(thisElem.x - 1, thisElem.y) == TileType.FREE) // Left Neighbor
                        {
                            // Check if this neibhor is the goal
                            if (thisElem.x - 1 == spawn_pos.x && thisElem.y == spawn_pos.y)
                            {
                                reachableTiles++;
                                currentTiles.Remove(thisElem);
                                break;
                            }
                            
                            // Check if its already seen
                            if (!seenTiles.Contains(new Vector2Int(thisElem.x - 1, thisElem.y)))
                            {
                                currentTiles.Add(new Vector2Int(thisElem.x - 1, thisElem.y));
                            }
                        }
                        
                        if (thisLayout.CoordsExist(thisElem.x + 1, thisElem.y) && thisLayout.GetTileAt(thisElem.x + 1, thisElem.y) == TileType.FREE) // Right Neighbor
                        {
                            // Check if this neibhor is the goal
                            if (thisElem.x + 1 == spawn_pos.x && thisElem.y == spawn_pos.y)
                            {
                                reachableTiles++;
                                currentTiles.Remove(thisElem);
                                break;
                            }
                            
                            // Check if its already seen
                            if (!seenTiles.Contains(new Vector2Int(thisElem.x + 1, thisElem.y)))
                            {
                                currentTiles.Add(new Vector2Int(thisElem.x + 1, thisElem.y));
                            }
                        }
                        
                        if (thisLayout.CoordsExist(thisElem.x, thisElem.y + 1) && thisLayout.GetTileAt(thisElem.x, thisElem.y + 1) == TileType.FREE) // Up Neighbor
                        {
                            // Check if this neibhor is the goal
                            if (thisElem.x == spawn_pos.x && thisElem.y + 1 == spawn_pos.y)
                            {
                                reachableTiles++;
                                currentTiles.Remove(thisElem);
                                break;
                            }
                            
                            // Check if its already seen
                            if (!seenTiles.Contains(new Vector2Int(thisElem.x, thisElem.y + 1)))
                            {
                                currentTiles.Add(new Vector2Int(thisElem.x, thisElem.y + 1));
                            }
                        }
                        
                        if (thisLayout.CoordsExist(thisElem.x, thisElem.y - 1) && thisLayout.GetTileAt(thisElem.x, thisElem.y - 1) == TileType.FREE) // Down Neighbor
                        {
                            // Check if this neibhor is the goal
                            if (thisElem.x == spawn_pos.x && thisElem.y - 1 == spawn_pos.y)
                            {
                                reachableTiles++;
                                currentTiles.Remove(thisElem);
                                break;
                            }
                            
                            // Check if its already seen
                            if (!seenTiles.Contains(new Vector2Int(thisElem.x, thisElem.y - 1)))
                            {
                                currentTiles.Add(new Vector2Int(thisElem.x, thisElem.y - 1));
                            }
                        }
                        
                        currentTiles.Remove(thisElem);

                    }
                    
                    // The goal was never found -> unreachable
                    freeTiles++;

                }
            }

            if ((double) reachableTiles / (double) freeTiles <= WALKABLE_THRESHOLD)
            {
                return false;
            }

            return true;

        }

        private bool CheckObjectValidity(int x_s, int x_e, int y_s, int y_e)
        {

            for (int i = x_s; i < x_e; i++)
            {
                for (int j = y_s; j < y_e; j++)
                {
                    if (layout.GetTileAt(i, j) != TileType.FREE || (spawn_pos.x == i && spawn_pos.y == j)) return false;
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

