using UnityEngine;
using Zenject;

public class PuzzleDiscriptionCutscene : MonoBehaviour
{
    [SerializeField]
    bool _startOnTrigger;

    CutsceneManager _cutsceneManager;

    private void Start()
    {
        _cutsceneManager = GetComponent<CutsceneManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_startOnTrigger)
        {
            _cutsceneManager.StartCutscene();
            _cutsceneManager.SetCutout();
            _startOnTrigger = false;
        }
    }
}
