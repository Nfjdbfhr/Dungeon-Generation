using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int maxRoomLength = 25;
    public int minRoomLength = 5;
    public int maxRoomWidth = 15;
    public int minRoomWidth = 5;
    public int numOfRooms = 9;
    public int minRooms = 4;

    public GameObject floorPrefab;
    public GameObject roomCenter;
    
    public List<int[,]> rooms = new List<int[,]>();
    public List<RoomCenterCoord> roomCenters = new List<RoomCenterCoord>();

    public int[,] fullDungeonArray;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numOfRooms; i++)
        {
            rooms.Add(new int[maxRoomWidth, maxRoomLength]);
            
            for (int j = 0; j < rooms[i].GetLength(0); j++)
            {
                for (int k = 0; k < rooms[i].GetLength(1); k++)
                {
                    rooms[i][j, k] = 0;
                }
            }
        }

        GenerateRooms();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void GenerateRooms()
    {
        int roomSkipsLeft = rooms.Count - minRooms;

        for (int i = 0; i < rooms.Count; i++)
        {
            if (roomSkipsLeft > 0 && Random.Range(0, 4) == 2)
            {
                roomSkipsLeft--;
                continue;
            }

            int[,] roomArea = rooms[i];
        
            int length = Random.Range(minRoomLength, maxRoomLength + 1);
            int width = Random.Range(minRoomWidth, maxRoomWidth + 1);
        
            int rowsDown = Random.Range(0, (maxRoomWidth - width) + 1);
            int columnsRight = Random.Range(0, (maxRoomLength - length) + 1);
        
            // Fill room edges with 1
            for (int j = columnsRight; j < columnsRight + length; j++)
            {
                roomArea[rowsDown, j] = 1;
                roomArea[rowsDown + width - 1, j] = 1;
            }
            for (int j = rowsDown; j < rowsDown + width; j++)
            {
                for (int k = columnsRight; k < columnsRight + length; k++)
                {
                    roomArea[j, k] = 1;
                }

                roomArea[j, columnsRight] = 1;
                roomArea[j, columnsRight + length - 1] = 1;
            }

            // Set the center spot to 2
            int centerRow = rowsDown + width / 2;
            int centerColumn = columnsRight + length / 2;

            // Ensure center is within bounds
            if (centerRow < roomArea.GetLength(0) && centerColumn < roomArea.GetLength(1))
            {
                roomArea[centerRow, centerColumn] = 2;
            }
        }

        CombineRoomsIntoArray();
    }

    public struct RoomCenterCoord
    {
        public int roomNum { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public RoomCenterCoord(int roomNumber, int x, int y)
        {
            roomNum = roomNumber;
            X = x;
            Y = y;
        }
    }

    public void CombineRoomsIntoArray()
    {
        // Calculate the size of the big array
        int bigArrayWidth = 3 * maxRoomWidth;
        int bigArrayLength = 3 * maxRoomLength;
    
        // Create the big array
        fullDungeonArray = new int[bigArrayWidth, bigArrayLength];
    
        // Loop through each room and place it in the big array
        for (int a = 0; a < 3; a++)
        {
            for (int i = 0; i < 3; i++)
            {
                // Get the current room
                int[,] room = rooms[i + (a * 3)];
            
                // Calculate the starting position in the big array
                int startX = i * maxRoomWidth;
                int startY = a * maxRoomLength;
            
                // Copy the room into the big array
                for (int j = 0; j < room.GetLength(0); j++)
                {
                    for (int k = 0; k < room.GetLength(1); k++)
                    {
                        fullDungeonArray[startX + j, startY + k] = room[j, k];
                        if (fullDungeonArray[startX + j, startY + k] != 0 && fullDungeonArray[startX + j, startY + k] != 1)
                        {
                            roomCenters.Add(new RoomCenterCoord(i + (a * 3), startX + j, startY + k));
                        }
                    }
                }
            }
        }

        SpawnDungeon();
    }

    public void SpawnDungeon()
    {
        for (int i = 0; i < fullDungeonArray.GetLength(0); i++)
        {
            for (int j = 0; j < fullDungeonArray.GetLength(1); j++)
            {
                if (fullDungeonArray[i, j] != 0)
                {
                    if (fullDungeonArray[i, j] == 1)
                    {
                        Instantiate(floorPrefab, new Vector3(i, 0, j), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(roomCenter, new Vector3(i, 0, j), Quaternion.identity);
                    }
                }
            }
        }
    }
}
