using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject prefab; // Sleep hier je prefab in
    public Transform spawnPoint; // Locatie waar het object gespawned wordt
    public float launchForce = 10; // Richting en kracht van de lancering

    void Start()
    {
        StartSpawn();
    }
    public void StartSpawn()
    {
        StartCoroutine(SpawnAndLaunch());
    }

    private IEnumerator SpawnAndLaunch()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject obj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(spawnPoint.up * launchForce, ForceMode.Impulse);
        }
    }
}
