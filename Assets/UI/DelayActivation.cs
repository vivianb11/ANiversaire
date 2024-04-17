using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayActivation : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToActivate;

    [SerializeField] float delay = 1f;

    private void Start()
    {
        StartCoroutine(ActivateObjects());
    }

    IEnumerator ActivateObjects()
    {
        yield return new WaitForSeconds(delay);

        foreach (var obj in objectsToActivate)
        {
            obj.SetActive(true);
        }

        Destroy(this);
    }
}
