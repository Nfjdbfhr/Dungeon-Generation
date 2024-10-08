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
    public GameObject startSpace;
    public GameObject endSpace;
    
    public List<int[,]> rooms = new List<int[,]>();
    public List<RoomCenterCoord> roomCenters = new List<RoomCenterCoord>();
    public List<GameObject> floorObjects = new List<GameObject>();

    public int[,] fullDungeonArray;

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

    // Start is called before the first frame update
    void Start()
    {
        GenerateDungeon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateDungeon()
    {
        rooms.Clear();
        roomCenters.Clear();

        for (int i = 0; i < floorObjects.Count; i++)
        {
            Destroy(floorObjects[i]);
        }
        
        floorObjects.Clear();

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
    
    public void GenerateRooms()
    {
        int roomSkipsLeft = rooms.Count - minRooms;

        int startRoom = Random.Range(0, rooms.Count);
        int endRoom = 0;
        do
        {
            endRoom = Random.Range(0, rooms.Count);
        }
        while (endRoom == startRoom);

        for (int i = 0; i < rooms.Count; i++)
        {

            Debug.Log(i);

            if (roomSkipsLeft > 0 && Random.Range(0, 4) == 2)
            {
                if (i != 8)
                {
                    if (i == startRoom)
                    {
                        startRoom++;
                    }
                    if (i == endRoom)
                    {
                        endRoom++;
                    }

                    Debug.Log(i + ": Skipped");

                    roomSkipsLeft--;
                    continue;
                }
            }

            if (i == rooms.Count - 1 && startRoom == endRoom)
            {
                for (int row = 0; row < rooms[0].GetLength(0); row++)
                {
                    for (int col = 0; col < rooms[0].GetLength(1); col++)
                    {
                        if (rooms[0][row, col] == 2)
                        {
                            rooms[0][row, col] = 3;
                        }
                    }
                }
            }

            int[,] roomArea = rooms[i];

            int roomType = Random.Range(1, 9);

            if (roomType == 1)
            {
                int length = Random.Range(minRoomLength, maxRoomLength + 1);
                int width = Random.Range(minRoomWidth, maxRoomWidth + 1);
        
                int rowsDown = Random.Range(0, (maxRoomWidth - width) + 1);
                int columnsRight = Random.Range(0, (maxRoomLength - length) + 1);

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

                int centerRow = rowsDown + width / 2;
                int centerColumn = columnsRight + length / 2;

                if (centerRow < roomArea.GetLength(0) && centerColumn < roomArea.GetLength(1))
                {
                    if (i == startRoom)
                    {
                        roomArea[centerRow, centerColumn] = 3;
                    }
                    else if (i == endRoom)
                    {
                        roomArea[centerRow, centerColumn] = 4;
                    }
                    else
                    {
                        roomArea[centerRow, centerColumn] = 2;
                    }
                }
            }
            else if (roomType == 2  || roomType == 3)
            {
                int length = Random.Range(minRoomLength, maxRoomLength + 1);
                int width = Random.Range(minRoomWidth, maxRoomWidth + 1);
        
                int rowsDown = Random.Range(0, (maxRoomWidth - width) + 1);
                int columnsRight = Random.Range(0, (maxRoomLength - length) + 1);

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

                int centerRow = rowsDown + width / 2;
                int centerColumn = columnsRight + length / 2;

                if (centerRow < roomArea.GetLength(0) && centerColumn < roomArea.GetLength(1))
                {
                    if (i == startRoom)
                    {
                        roomArea[centerRow, centerColumn] = 3;
                    }
                    else if (i == endRoom)
                    {
                        roomArea[centerRow, centerColumn] = 4;
                    }
                    else
                    {
                        roomArea[centerRow, centerColumn] = 2;
                    }
                }

                int position = Random.Range(0, 2);

                if (position == 0)
                {
                    int distanceOut = Random.Range(0, rowsDown);
                    int lengthOut = Random.Range(2, length);
                    for (int row = rowsDown - distanceOut; row < rowsDown; row++)
                    {
                        for (int col = columnsRight; col < columnsRight + lengthOut; col++)
                        {
                            roomArea[row, col] = 1;
                        }
                    }
                }
                else if (position == 1)
                {
                    int distanceOut = Random.Range(0, rowsDown);
                    int lengthOut = Random.Range(2, length);
                    for (int row = rowsDown - distanceOut; row < rowsDown; row++)
                    {
                        for (int col = columnsRight + length - 1; col >= columnsRight + length - lengthOut; col--)
                        {
                            roomArea[row, col] = 1;
                        }
                    }
                }
            }
            else
            {
                // Define the room size (still within the bounds of max/min length/width).
                int length = Random.Range(minRoomLength, maxRoomLength + 1);
                int width = Random.Range(minRoomWidth, maxRoomWidth + 1);

                int rowsDown = Random.Range(0, (maxRoomWidth - width) + 1);
                int columnsRight = Random.Range(0, (maxRoomLength - length) + 1);

                // Start at a random position inside the room
                int currentX = Random.Range(rowsDown, rowsDown + width);
                int currentY = Random.Range(columnsRight, columnsRight + length);

                // Set the total number of steps (amount of floor to be "walked")
                int totalSteps = Random.Range(((length * width) / 3) + 5, ((length * width) / 2) + 5);
                totalSteps += 75;

                // Mark the starting position as part of the room
                roomArea[currentX, currentY] = 1;

                int middleX = 0;
                int middleY = 0;

                // Random walk directions
                for (int step = 0; step < totalSteps; step++)
                {
                    // Choose a random direction (0 = up, 1 = down, 2 = left, 3 = right)
                    int direction = Random.Range(0, 4);

                    switch (direction)
                    {
                        case 0: // Up
                            if (currentX > rowsDown)
                            {
                                currentX--;
                            }
                            break;
                        case 1: // Down
                            if (currentX < rowsDown + width - 1)
                            {
                                currentX++;
                            }
                            break;
                        case 2: // Left
                            if (currentY > columnsRight)
                            {
                                currentY--;
                            }
                            break;
                        case 3: // Right
                            if (currentY < columnsRight + length - 1)
                            {
                                currentY++;
                            }
                            break;
                    }

                    // Mark the new position as part of the room
                    roomArea[currentX, currentY] = 1;

                    if (step == 1)
                    {
                        middleX = currentX;
                        middleY = currentY;
                    }
                }
                
                if (i == startRoom)
                {
                    roomArea[middleX, middleY] = 3;
                }
                else if (i == endRoom)
                {
                    roomArea[middleX, middleY] = 4;
                }
                else
                {
                    roomArea[middleX, middleY] = 2;
                }
            }
        }

        CombineRoomsIntoArray();
    }

    public void CombineRoomsIntoArray()
    {
        int bigArrayWidth = 3 * maxRoomWidth;
        int bigArrayLength = 3 * maxRoomLength;
    
        fullDungeonArray = new int[bigArrayWidth, bigArrayLength];
    
        for (int a = 0; a < 3; a++)
        {
            for (int i = 0; i < 3; i++)
            {
                int[,] room = rooms[i + (a * 3)];
            
                int startX = i * maxRoomWidth;
                int startY = a * maxRoomLength;
            
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
        bool hasPlacedStart = false;
        for (int i = 0; i < fullDungeonArray.GetLength(0); i++)
        {
            for (int j = 0; j < fullDungeonArray.GetLength(1); j++)
            {
                if (fullDungeonArray[i, j] != 0)
                {
                    if (fullDungeonArray[i, j] == 1)
                    {
                        floorObjects.Add(Instantiate(floorPrefab, new Vector3(i, 0, j), Quaternion.identity));
                    }
                    else if (fullDungeonArray[i, j] == 2)
                    {
                        floorObjects.Add(Instantiate(roomCenter, new Vector3(i, 0, j), Quaternion.identity));
                    }
                    else if (fullDungeonArray[i, j] == 3 && !hasPlacedStart)
                    {
                        floorObjects.Add(Instantiate(startSpace, new Vector3(i, 0, j), Quaternion.identity));
                        hasPlacedStart = true;
                    }
                    else
                    {
                        floorObjects.Add(Instantiate(endSpace, new Vector3(i, 0, j), Quaternion.identity));
                    }
                }
            }
        }
    }
}
