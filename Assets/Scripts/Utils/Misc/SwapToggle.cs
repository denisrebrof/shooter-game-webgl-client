using UnityEngine;
using UnityEngine.UI;

public class SwapToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private Image baseGraphics;

    private void Start()
    {
        onChanged(toggle.isOn);
        toggle.onValueChanged.AddListener(onChanged);
    }

    private void onChanged(bool active)
    {
        baseGraphics.enabled = !active;
    }
}