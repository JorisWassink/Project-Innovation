using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class ChunkPlacer : MonoBehaviour
{
    public GameObject chunkPrefab; // Assign your chunk prefab here
    public int chunkSize = 10; // Size of each chunk
    public int sceneSize = 100; // Total scene size
    private Dictionary<Vector2Int, GameObject> chunkDictionary = new Dictionary<Vector2Int, GameObject>();

    public Transform Player;
   // private Chunk[] Chunks;
    private List<Chunk> Chunks = new List<Chunk>();
    private List<Chunk> _spawnedChunks = new List<Chunk>();
    
    [SerializeField] private int outerRadius;
    [SerializeField] private int innerRadius;

    void Start()
    {
        GenerateChunks();
        AssignObjectsToChunks();
        //DeleteEmptyChunks();        
        foreach (var chunk in Chunks)
            _spawnedChunks.Add(chunk);
        StartCoroutine(CheckPosition());
    }
    


    void GenerateChunks()
    {
        int halfChunks = (sceneSize / chunkSize) / 2; // Zorgt voor chunks in beide richtingen

        for (int x = -halfChunks; x < halfChunks; x++)
        {
            for (int z = -halfChunks; z < halfChunks; z++)
            {
                Vector3 position = new Vector3(x * chunkSize, 0, z * chunkSize);
                GameObject newChunk = Instantiate(chunkPrefab, position, Quaternion.identity);
                newChunk.name = $"Chunk_{x}_{z}";
                newChunk.transform.parent = transform;
                
                Chunk chunkComponent = newChunk.GetComponent<Chunk>();
                chunkComponent.ID = x * 1000 + z;

                if (chunkComponent != null)
                {
                    Chunks.Add(chunkComponent);
                }
                    
                chunkDictionary[new Vector2Int(x, z)] = newChunk;
            }
        }
    }


    void AssignObjectsToChunks()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
    
        foreach (GameObject obj in allObjects)
        {
            if (obj == gameObject || obj.layer != LayerMask.NameToLayer("Wall"))
                continue;
            

            Vector2Int closestChunk = GetClosestChunk(obj.transform.position);

            // Assign object to chunk if found
            if (chunkDictionary.TryGetValue(closestChunk, out GameObject chunk))
            {
                obj.transform.parent = chunk.transform;
            }
        }
    }


    void DeleteEmptyChunks()
    {
        foreach (var chunk in Chunks)
        {
            if (chunk.transform.childCount <= 0)
            {
                _spawnedChunks.Remove(chunk);
                Destroy(chunk.gameObject);
            }
        }
    }

    Vector2Int GetClosestChunk(Vector3 position)
    {
        int chunkX = Mathf.FloorToInt(position.x / chunkSize);
        int chunkZ = Mathf.FloorToInt(position.z / chunkSize);
        return new Vector2Int(chunkX, chunkZ);
    }


    
    private IEnumerator CheckPosition()
    {
        while (true)
        {
            foreach (Chunk chunk in Chunks)
            {
                float distanceX = Mathf.Abs(Player.position.x - (transform.TransformPoint(chunk.transform.localPosition)).x);
                float distanceZ = Mathf.Abs(Player.position.z - (transform.TransformPoint(chunk.transform.localPosition)).z);
                
                if ((distanceX < innerRadius && distanceZ < innerRadius) && !_spawnedChunks.Contains(chunk))
                {
                    SpawnChunk(chunk); 
                   // Debug.Log($"spawned chunk {chunk.ID} at {distanceX}, {distanceZ}");
                }
                else if ((distanceX > outerRadius || distanceZ > outerRadius) &&  _spawnedChunks.Contains(chunk))
                {
                    DeleteChunk(chunk);
                   // Debug.Log($"destroyed chunk {chunk.ID} at {distanceX}, {distanceZ}");
                }
            }
            yield return new WaitForSeconds(0.4f);
        }
    }

    
    private void SpawnChunk(Chunk chunk)
    {
        chunk.gameObject.SetActive(true);
        _spawnedChunks.Add(chunk);
    }

    private void DeleteChunk(Chunk chunk)
    {
        chunk.gameObject.SetActive(false);
        _spawnedChunks.Remove(chunk);
    }
    
    

    /*private void OnDrawGizmos()
    {
        foreach (Chunk chunk in Chunks)
        {
            // Get the chunk's world position if it were a child of the current transform
            Vector3 worldPosition = transform.TransformPoint(chunk.transform.localPosition);

            // Use the chunk's local rotation to apply to the Gizmos
            Gizmos.color = new Color(255, 255, 255, 50);
            Gizmos.matrix = Matrix4x4.TRS(worldPosition, chunk.transform.rotation, Vector3.one);
            Gizmos.DrawCube(Vector3.zero, innerRadius * 2 * Vector3.one);

            Gizmos.color = new Color(255, 0, 0, 10);
            Gizmos.matrix = Matrix4x4.TRS(worldPosition, chunk.transform.rotation, Vector3.one);
            Gizmos.DrawCube(Vector3.zero, outerRadius * 2 * Vector3.one);

            // Draw the line from the player to the chunk's world position
            Debug.DrawLine(Player.position, worldPosition, Color.green);
        }
    }*/


}