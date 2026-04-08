using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    GameObject _dialogueLayout;
    [SerializeField]
    TextMeshProUGUI _dialogueSpeaker;
    [SerializeField]
    TextMeshProUGUI _dialogueText;

    [Inject]
    PlayerInput _input;
    [Inject]
    PlayerController _player;

    DialogueParser _parser;
    Animator _animator;
    PlayableDirector _timeline;

    readonly float _charactersPerSecond = 33;
    readonly float _weightSpeedChange = 10f;

    DialogueData[] _dialogueLines;
    int _lineIndex = 0;
    bool _isTyping;
    bool _phoneCall;
    int _layerIndex;
    int _layerValue;
    bool _changeLayer;

    void Start()
    {
        _parser = new DialogueParser();
        _animator = _player.GetComponent<Animator>();
    }

    private void Update()
    {
        if (_changeLayer)
        {
            _animator.SetLayerWeight(_layerIndex, Mathf.Lerp(_animator.GetLayerWeight(_layerIndex), _layerValue, Time.deltaTime * _weightSpeedChange));
            if (_animator.GetLayerWeight(_layerIndex) == _layerValue)
            {
                _changeLayer = false;
            }
        }

        if (_dialogueLayout.activeSelf && _input.Click)
        {
            if (_isTyping)
            {
                StopAllCoroutines();
                _isTyping = false;
                _dialogueText.text = _dialogueLines[_lineIndex].text;
            }
            else
            {
                ResetTrigger();
                _lineIndex++;
                ShowNextLine();
            }
        }
    }
    /// <summary>
    /// Loads a dialogue and opens it in the dialogue window
    /// </summary>
    /// <param name="dialogueName"></param>
    public void StartDialogue(string dialogueName)
    {
        _timeline = GameObject.Find("Cutscene").GetComponent<PlayableDirector>();
        _timeline.Pause();
        Dialogue dialogue = _parser.LoadDialogue(dialogueName);
        if (dialogue != null)
        {
            _dialogueLines = dialogue.lines;
            _dialogueLayout.SetActive(true);
            _phoneCall = dialogue.phoneCall;
            ShowNextLine();
        }
    }
    /// <summary>
    /// Shows a next line of the dialogue
    /// </summary>
    public void ShowNextLine()
    {
        if (_layerValue == 1)
        {
            _layerValue = 0;
            _changeLayer = true;
        }

        if (_lineIndex >= _dialogueLines.Length)
        {
            EndDialogue();
        }
        DialogueData line = _dialogueLines[_lineIndex];
        _dialogueSpeaker.text = line.speaker;
        PlayPlayerAnimation(line);

        StartCoroutine(TypeLine(line.text));
    }
    /// <summary>
    /// Closes the dialogue window
    /// </summary>
    public void EndDialogue()
    {
        if (_phoneCall)
        {
            _animator.SetTrigger("HangUp");
        }
        _lineIndex = 0;
        _dialogueLayout.SetActive(false);
        _timeline.Resume();

    }
    /// <summary>
    /// Types a dialogue line by letters
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    IEnumerator TypeLine(string line)
    {
        _isTyping = true;
        _dialogueText.text = "";

        float delay = 1f / _charactersPerSecond;

        foreach (char c in line)
        {
            _dialogueText.text += c;
            yield return new WaitForSeconds(delay);
        }

        _isTyping = false;
    }
    /// <summary>
    /// Plays an animation in the beginning of a line
    /// </summary>
    /// <param name="dialogue"></param>
    void PlayPlayerAnimation(DialogueData dialogue)
    {
        if (!string.IsNullOrEmpty(dialogue.animation))
        {
            if (!_phoneCall || dialogue.animationLayer != 2)
            {
                _layerValue = 1;
                _layerIndex = dialogue.animationLayer;
                _changeLayer = true;
            }
            _animator.SetTrigger(dialogue.animation);
        }
    }

    void ResetTrigger()
    {
        if (!string.IsNullOrEmpty(_dialogueLines[_lineIndex].animation))
        {
            _animator.ResetTrigger(_dialogueLines[_lineIndex].animation);
        }
    }
}
