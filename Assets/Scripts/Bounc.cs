using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class Bounc : MonoBehaviour
{
    private MusicNote note;
    private AudioSource audioSource;

    private Rigidbody rb;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionExit(Collision collision)
    {
        rb.AddForce(rb.velocity.normalized * 10, ForceMode.Impulse);

        ChangeColor();
        EmitRandomSound();
    }

    private void EmitRandomSound()
    {
        note = (MusicNote)Random.Range(0, 7);
        PlayNote();
    }

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

    private void ChangeColor()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }
}
