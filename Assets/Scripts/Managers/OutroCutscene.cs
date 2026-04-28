using UnityEngine;
using Zenject;

public class OutroCutscene : MonoBehaviour
{
    [Inject]
    UIManager _UI;

    public void EndGame()
    {
        _UI.ReloadGame();
    }
}
