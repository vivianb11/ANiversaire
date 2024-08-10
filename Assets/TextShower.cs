using TMPro;
using UnityEngine;

public class TextShower : MonoBehaviour
{
    public TextMeshPro tmPro;

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
