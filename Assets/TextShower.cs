using UnityEngine;
using TMPro;

public class TextShower : MonoBehaviour
{
    public TextMeshProUGUI tmPro;

    public SoundInteraction soundInteraction;
    
    private void Awake()
    {
        soundInteraction.onInteract.AddListener(SetText);
    }

    private void SetText()
    {
        tmPro.text = soundInteraction.audioSource.clip.name;
    }
}
