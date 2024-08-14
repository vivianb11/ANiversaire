using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SoundInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] bool playClip = true;
    [ShowIf("playClip")]
    [SerializeField] bool RandomClip = false;

    [SerializeField] bool playOnce = false;

    [ShowIf("playClip")]
    public List<AudioClip> clips;
    private int currentClipIndex = 0;

    [HideIf("playClip")]
    public MusicNote note = MusicNote.A;

    [HideInInspector] public AudioSource audioSource;

    public UnityEvent onInteract;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact(Movement player)
    {
        if (!playClip)
        {
            PlayNote();
            onInteract?.Invoke();
            return;
        }

        StopAllCoroutines();

        PlayClip();
    }

    public void PlayClip()
    {
        if (clips is null)
            return;

        if (RandomClip)
            currentClipIndex = Random.Range(0, clips.Count);

        audioSource.clip = clips[currentClipIndex];
        currentClipIndex = (currentClipIndex + 1) % clips.Count;
        audioSource.Play();

        onInteract?.Invoke();

        if (!playOnce)
            StartCoroutine(PlayNextClip());
    }

    // create a new method called PlayNote
    public void PlayNote()
    {
        audioSource.clip = CreateNote(note);
        audioSource.Play();
    }

    AudioClip CreateNote(MusicNote note)
    {
        // creates the desired note

        AudioClip clip = AudioClip.Create(note.ToString(), 44100, 1, 44100, false);
        float[] data = new float[44100];
        float f = 0f;

        switch (note)
        {
            case MusicNote.C:
                f = 261.63f;
                break;
            case MusicNote.D:
                f = 293.66f;
                break;
            case MusicNote.E:
                f = 329.63f;
                break;
            case MusicNote.F:
                f = 349.23f;
                break;
            case MusicNote.G:
                f = 392.00f;
                break;
            case MusicNote.A:
                f = 440.00f;
                break;
            case MusicNote.B:
                f = 493.88f;
                break;
        }

        for (int i = 0; i < 44100; i++)
        {
            data[i] = Mathf.Sin(2 * Mathf.PI * f * i / 44100);
        }

        clip.SetData(data, 0);

        return clip;
    }

    private System.Collections.IEnumerator PlayNextClip()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        PlayClip();
    }
}

public enum MusicNote
{
    C,
    D,
    E,
    F,
    G,
    A,
    B
}