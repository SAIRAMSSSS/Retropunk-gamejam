using System.Collections;
using UnityEngine;

public class SoundLeversScreen : MonoBehaviour
{
    [SerializeField]
    SoundLightSignalPanel _soundLightSignalPanel;
    [SerializeField]
    ConsoleObject _console;

    SoundLever[] _levers;
    SFXController _SFXPlayer;
    GameObject _leversLayout;

    private void Start()
    {
        _SFXPlayer = GetComponent<SFXController>();
        _leversLayout = transform.GetChild(0).gameObject;
        _levers = new SoundLever[_leversLayout.transform.childCount];

        for (int i = 0; i < _leversLayout.transform.childCount; i++)
        {
            _levers[i] = _leversLayout.transform.GetChild(i).GetComponent<SoundLever>();
        }
    }

    public void ExitConsole()
    {
        _leversLayout.SetActive(false);
        _soundLightSignalPanel.CanPlaySequence(true);
    }

    public void Submit()
    {
        _SFXPlayer.PlaySound("Push");
        foreach (var lever in _levers)
        {
            if (!lever.CheckChosenTone())
            {
                _SFXPlayer.PlaySound("Wrong");
                return;
            }
        }

        StartCoroutine(PlayCorrectSequence());
    }
    /// <summary>
    /// Plays the right tone sequence if the puzzle is completed
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayCorrectSequence()
    {
        yield return new WaitForSeconds(10f);
        ExitConsole();
        _console.CompletePuzzle(1);
    }
}
