using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn_Delay : MonoBehaviour
{
    public float delay = 30f;

    private void Start()
    {
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

}
