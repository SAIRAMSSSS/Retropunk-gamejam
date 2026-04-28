using Zenject;

public class ConsoleObject : InteractionObject
{
    [Inject]
    GameManager _gameManager;

    public override bool CanInteract()
    {
        return _canInteract && !_player.HasPickUp;
    }
    /// <summary>
    /// Closes the interaction with the puzzle and signals that it's been completed
    /// </summary>
    /// <param name="roomNum"></param>
    public void CompletePuzzle(LevelNames room)
    {
        _canInteract = false;
        _gameManager.CompleteRoom(room);
    }
}
