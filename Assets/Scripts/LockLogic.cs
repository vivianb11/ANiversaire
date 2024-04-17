using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LockLogic : MonoBehaviour
{
    [SerializeField] List<PinInteraction> interactables;

    public List<Pincode> OpenCodes = new List<Pincode>();

    public UnityEvent OnCorrectOpen;
    public UnityEvent OnCorrectStep;
    public UnityEvent OnIncorectCode;

    private int currentCodeIndex = 0;

    void Start()
    {
        foreach (PinInteraction interactable in interactables)
        {
            interactable.OnInteract.AddListener(OnKeyPressed);
        }
    }

    public void OnKeyPressed(int number)
    {
        bool changed = false;

        foreach (Pincode code in OpenCodes.Where(code => code.isValide == true))
        {
            
            if(code.Check(currentCodeIndex, number))
            {
                OnCorrectStep?.Invoke();
                currentCodeIndex++;
                
                changed = true;
            
                if (currentCodeIndex >= code.codeLength)
                {
                    currentCodeIndex = 0;

                    OnCorrectOpen?.Invoke();

                    ResetCodes();
                }
            }
        }

        if (changed)
            return;

        if(OpenCodes.Where(code => code.isValide).Count() == 0)
        {
            currentCodeIndex = 0;

            OnIncorectCode?.Invoke();

            ResetCodes();

            return;
        }
    }

    private void ResetCodes()
    {
        foreach (Pincode code in OpenCodes)
        {
            code.isValide = true;
        }
    }
}

[Serializable]
public class Pincode
{
    [HideInInspector] public bool isValide = true;

    public List<int> code;

    public int codeLength => code.Count;

    public Pincode()
    {
        code = new List<int>(4);
        isValide = true;
    }

    public bool Check(int index, int value)
    {
        if (index >= codeLength || code[index] != value)
        {
            isValide = false;
            return false;
        }

        return true;
    }
}