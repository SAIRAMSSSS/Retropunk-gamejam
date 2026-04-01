using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Image _darkenScreen;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    /// <summary>
    /// Sets an alpha to the darken image color
    /// </summary>
    /// <param name="alpha"></param>
    public void SetDarkenScreen(float alpha)
    {
        Color newColor = _darkenScreen.color;
        newColor .a = alpha;    
        _darkenScreen.color = newColor;
    }
}
