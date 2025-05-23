using System;
using UnityEngine;
using static ClassesAndEnums;


public class RoomManager : MonoBehaviour
{
    
    public static RoomManager Instance;
    public int roomNumber = 0;
    public RoomType roomType = RoomType.RANDOM_ROOM;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
