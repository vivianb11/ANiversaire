using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YayBoom : MonoBehaviour
{
    public GameObject prefabToSpawn;

    public List<Transform> SpawnPoints;

    public void SrartSpawn()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            int index = Random.Range(0, SpawnPoints.Count);
            Instantiate(prefabToSpawn, SpawnPoints[index].position, Quaternion.identity);
        }
    }
}
