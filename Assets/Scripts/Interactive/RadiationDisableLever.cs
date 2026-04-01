using UnityEngine;

public class RadiationDisableLever : MonoBehaviour
{
    [SerializeField]
    RadioactivePanel[] _panels;
    /// <summary>
    /// Disables radioactive panels after using the lever
    /// </summary>
    public void DisableRadiation()
    {
        foreach (var panel in _panels)
        {
            panel.enabled = false;
        }
    }
}
