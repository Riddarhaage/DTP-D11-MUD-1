using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dtp11_MUD_1
{
        /// <summary>
        /// A class for opening doors between Room:s, the Lockkey is defined
        /// for two room numbers, and it can be adressed whether it really
        /// opens the door from one Room to another.
        ///
        /// <b>weakness:</b> the key cannot be used to actually open a door
        /// from Room, the Room code must detect whether the Lockkey fits a
        /// certain door, and then open it itself.
        /// </summary>
    public class LockKey
    {
        /// <summary>
        /// The number of the two rooms that the door spans in
        /// increasing order
        /// </summary>
        public int[] adjRooms = new int[2];
        /// <summary>
        /// LockKey constructor for two rooms
        /// </summary>
        /// <param name="room1"></param>
        /// <param name="room2"></param>
        public LockKey(int room1, int room2)
        {
            /// \todo Do something!
            if (room1 < room2)
            {
                adjRooms[0] = room1;
                adjRooms[1] = room2;
            }
            else
            {
                adjRooms[0] = room2;
                adjRooms[1] = room1;
            }
        }
        /// <summary>
        /// Ask key if it's OK to open between two rooms
        /// </summary>
        /// <param name="room1"></param>
        /// <param name="room2"></param>
        /// <returns></returns>
        public bool Opens(int room1, int room2)
        {
            if (room1 < room2)
            {
                return room1 == adjRooms[0] && room2 == adjRooms[1];
            }
            else
            {
                return room1 == adjRooms[1] && room2 == adjRooms[0];
            }
        }
    }
    public class Door
    {
        Room room1, room2;
        bool isOpen;
        public Door(Room r1, Room r2, bool open)
        {
            room1 = r1;
            room2 = r2;
            isOpen = open;
        }
        public Room NextRoom(Room current)
        {
            if (current == room1) return room2;
            return room1;
        }
    }
    public class Room
    {
        // Constants and static members:
        public const int North = 0;
        public const int East = 1;
        public const int South = 2;
        public const int West = 3;
        public const int NoDoor = -1;
        public static string CardinalDirection(int dir)
        {
            switch (dir)
            {
                case 0: return "norr";
                case 1: return "öster";
                case 2: return "söder";
                case 3: return "väster";
                default: return "okänd riktning";
            }
        }

        // Object attributes:
        public int number;
        public string roomname = "";
        public string story = "";
        public List<string> imageFile = new List<string>();
        public int[] adjacent = new int[4]; // adjacent[Room.North] etc.
        public bool[] isOpen = new bool[4]; // isOpen[Room.North] etc.
        public Door[] door = new Door[4];

        // Stuff in this room:
        public List<LockKey> keys = new List<LockKey>();
        public Room(int num, string name)
        {
            number = num; roomname = name;
        }
        public void SetStory(string theStory)
        {
            story = theStory;
        }
        public void SetImage(string theImage)
        {
            imageFile.Add(theImage);
        }
        private void setNextRoom(int direction, int roomNumber)
        {
            if (roomNumber < 0)
            {
                adjacent[direction] = -roomNumber;
                isOpen[direction] = false;
            }
            else
            {
                adjacent[direction] = roomNumber;
                isOpen[direction] = true;
            }
        }
        public void SetDirections(int N, int E, int S, int W)
        {
            setNextRoom(North, N);
            setNextRoom(East, E);
            setNextRoom(South, S);
            setNextRoom(West, W);
        }
        public void setDoors(Door N, Door E, Door S, Door W)
        {
            door[North] = N;
            door[East] = E;
            door[South] = S;
            door[West] = W;
        }
        private int GetNextRoom(int direction)
        {
            int N = adjacent[direction];
            // Refuse if there is no such door:
            if (N == Room.NoDoor) return number;
            if (!isOpen[direction]) return number;
            return adjacent[direction];
        }
        public Door GetNorthDoor() => door[North];
        public Door GetSouthDoor() => door[South];
        public Door GetWestDoor() => door[West];
        public Door GetEastDoor() => door[East];
        public int GetNorth() => GetNextRoom(North);
        public int GetEast() => GetNextRoom(East);
        public int GetSouth() => GetNextRoom(South);
        public int GetWest() => GetNextRoom(West);
        public void HasKey(int room1, int room2)
        {
            keys.Add(new LockKey(room1, room2));
        }
        public List<LockKey> getKeys(List<LockKey> otherKeys)
        {
            foreach (LockKey lc in keys)
            {
                otherKeys.Add(lc);
            }
            return otherKeys;
        }
        public void Unlock(List<LockKey> keys)
        {
            foreach (LockKey lc in keys)
            {
                for (int i = North; i <= West; i++)
                {
                    if (lc.Opens(number, adjacent[i]))
                    {
                        isOpen[i] = true;
                    }
                }
            }
        }
    }
}
