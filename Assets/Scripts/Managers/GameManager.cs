using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool[] _completedRooms = new bool[4];
    /// <summary>
    /// A puzzle in a room is completed
    /// </summary>
    /// <param name="rooomIndex"></param>
    public void CompleteRoom(int rooomIndex)
    {
        _completedRooms[rooomIndex] = true;
    }


}
