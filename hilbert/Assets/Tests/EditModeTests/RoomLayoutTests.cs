using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static RoomLayout;
using static ClassesAndEnums;

public class RoomLayoutTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void TileMapInit()
    {
        int width = 4;
        int height = 3;
        
        RoomLayout layout = new RoomLayout(width, height);
        
        // Testing if the tile set is initialized correctly
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Assert.AreEqual(TileType.FREE, layout.GetTileAt(i, j));
            }
        }
        
    }
    
}
