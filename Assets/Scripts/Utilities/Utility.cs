using System.Collections;
using UnityEngine;

public static class Utility
{
    public static void InvokeAfter(this MonoBehaviour mono, System.Action action, float time)
    {
        mono.StartCoroutine(InvokeAfterCoroutine(action, time));
    }

    private static IEnumerator InvokeAfterCoroutine(System.Action action, float time)
    {
        yield return new WaitForSeconds(time);

        Debug.Log("Invoking action after " + time + " seconds");

        action();
    }
}
