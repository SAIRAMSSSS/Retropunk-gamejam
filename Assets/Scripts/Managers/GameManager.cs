using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _timerText;

    [Inject]
    LevelManager _levelManager;
    [Inject]
    AudioManager _audioManager;
    SFXController _SFXPlayer;

    public bool AllPuzzlesCompleted => _completedRooms.All(r => r);
    readonly string _endCutsceneName = "Ending_MainDrag";

    bool[] _completedRooms = new bool[4];
    bool _timerStarted = false;
    int _timerMinutes = 45;
    float _timerSeconds = 0f;

    private void Start()
    {
        _SFXPlayer = GetComponent<SFXController>();
    }
    /// <summary>
    /// A puzzle in a room is completed
    /// </summary>
    /// <param name="rooomIndex"></param>
    public void CompleteRoom(LevelNames room)
    {
        if ((int)room <= _completedRooms.Length)
            return;

        _completedRooms[(int)room - 1] = true;
        _SFXPlayer.PlaySound("Complete");

        if (AllPuzzlesCompleted)
        {
            _timerStarted = false;
            _timerText.gameObject.SetActive(false);
            StartCoroutine(_audioManager.StartNewLevelMusic(5));
            _levelManager.PlayCutscene(_endCutsceneName);
        }
    }

    public bool IsRoomComplete(int room) => _completedRooms[room];

    public void WrongAnswerPuzzle()
    {
        _SFXPlayer.PlaySound("Wrong");
    }

    public void StartTimer()
    {
        _timerStarted = true;
        _timerText.gameObject.SetActive(true);
    }

    private void Update()
    {
        //updates timer
        if(_timerStarted)
        {
            if (_timerSeconds <= 0)
            {
                _timerMinutes--;
                _timerSeconds = 60f;

                if(_timerMinutes < 0)
                {
                    _timerText.gameObject.SetActive(false);
                    _levelManager.GetCurrentCutscene<GameOverCutscene>().StartGameOver();
                }
            }
            _timerSeconds -= Time.deltaTime;
            _timerText.text = $"{_timerMinutes:00} : {Mathf.CeilToInt(_timerSeconds):00}";
        }
    }
}
