using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReactor : MonoBehaviour
{
    public enum ReactionType
    {
        Scale,
        Rotate,
        Move,
        Color
    }

    public AudioSource audioSource;
    [Space(10)]
    public float sensitivity = 10;
    public float smoothTime = 0.1f;
    [Space(10)]
    public ReactionType reactionType;
    [ShowIf("reactionType", ReactionType.Rotate)]
    public float maxRotation = 90;
    [ShowIf("reactionType", ReactionType.Scale)]
    public float maxScale = 2;
    [ShowIf("reactionType", ReactionType.Move)]
    public float maxMove = 2;
    [ShowIf("reactionType", ReactionType.Color)]
    public Color color;

    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Vector3 initialRotation;
    private Color initialColor;
    private Material initialMaterial;

    private void Start()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
        initialRotation = transform.eulerAngles;
        initialColor = GetComponent<Renderer>().material.color;
    }

    private void Update()
    {
        float[] spectrum = new float[256];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        float sum = 0;
        for (int i = 0; i < spectrum.Length; i++)
        {
            sum += spectrum[i];
        }

        float value = sum / spectrum.Length * sensitivity;

        switch (reactionType)
        {
            case ReactionType.Scale:
                transform.localScale = Vector3.Lerp(transform.localScale, initialScale + Vector3.one * value * maxScale, smoothTime);
                break;
            case ReactionType.Rotate:
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, initialRotation + Vector3.one * value * maxRotation, smoothTime);
                break;
            case ReactionType.Move:
                transform.position = Vector3.Lerp(transform.position, initialPosition + Vector3.one * value * maxMove, smoothTime);
                break;
            case ReactionType.Color:
                GetComponent<Renderer>().material.color = Color.Lerp(initialColor, color, value);
                break;
        }
    }

}
