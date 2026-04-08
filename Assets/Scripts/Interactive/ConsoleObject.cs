using Zenject;

public class ConsoleObject : InteractionObject
{
    [Inject]
    GameManager _gameManager;
    /// <summary>
    /// Closes the interaction with the puzzle and signals that it's been completed
    /// </summary>
    /// <param name="roomNum"></param>
    public void CompletePuzzle(int roomNum)
    {
        _canInteract = false;
        _gameManager.CompleteRoom(roomNum);
    }
}
