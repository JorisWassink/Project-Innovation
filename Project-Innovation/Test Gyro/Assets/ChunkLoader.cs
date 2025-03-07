using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class ChunkPlacer : MonoBehaviour
{
    public Transform Player;
    private Chunk[] Chunks;
    public Chunk FirstChunk;
    private List<Chunk> _spawnedChunks = new List<Chunk>();
    private HashSet<int> _spawnedChunkIDs = new HashSet<int>();
    
    [SerializeField] private int outerRadius;
    [SerializeField] private int innerRadius;

    void Start()
    {
        Chunks = gameObject.GetComponentsInChildren<Chunk>();
        StartCoroutine(CheckPosition());

        foreach (var chunk in Chunks)
        {
            _spawnedChunks.Add(chunk);
            _spawnedChunkIDs.Add(chunk.ID);
        }
    }


    
    private IEnumerator CheckPosition()
    {
        while (true)
        {
            foreach (Chunk chunk in Chunks)
            {
                float distanceX = Mathf.Abs(Player.position.x - (transform.TransformPoint(chunk.transform.localPosition)).x);
                float distanceY = Mathf.Abs(Player.position.y - (transform.TransformPoint(chunk.transform.localPosition)).y);


                
                if ((distanceX < innerRadius && distanceY < innerRadius) && !_spawnedChunkIDs.Contains(chunk.ID))
                {
                    SpawnChunk(chunk); // Return after SpawnChunk completes
                    Debug.Log($"spawned chunk {chunk.ID} at {distanceX}, {distanceY}");
                }
                else if ((distanceX > outerRadius || distanceY > outerRadius) &&  _spawnedChunkIDs.Contains(chunk.ID))
                {
                    DeleteChunk(chunk);
                    Debug.Log($"destroyed chunk {chunk.ID} at {distanceX}, {distanceY}");

                }
                
            }

            // Prevent this from checking every single frame
            yield return new WaitForSeconds(0.1f);
        }
    }

    
    private void SpawnChunk(Chunk chunk)
    {
        chunk.gameObject.SetActive(true);
        _spawnedChunks.Add(chunk);
        _spawnedChunkIDs.Add(chunk.ID);
        
    }

    private void DeleteChunk(Chunk chunk)
    {
        chunk.gameObject.SetActive(false);
        _spawnedChunks.Remove(chunk);
        _spawnedChunkIDs.Remove(chunk.ID);  
        
    }

    private void OnDrawGizmos()
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
    }


}

