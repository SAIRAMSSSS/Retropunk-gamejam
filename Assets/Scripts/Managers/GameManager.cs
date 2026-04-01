using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool[] _completedRooms = new bool[4];

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void CompleteRoom(int rooomIndex)
    {
        _completedRooms[rooomIndex] = true;
    }
}
