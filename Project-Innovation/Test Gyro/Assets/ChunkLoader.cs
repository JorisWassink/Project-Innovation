using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class ChunkPlacer : MonoBehaviour
{
    public Transform Player;
    private Chunk[] Chunks;
    private List<Chunk> _spawnedChunks = new List<Chunk>();
    
    [SerializeField] private int outerRadius;
    [SerializeField] private int innerRadius;

    void Start()
    {
        Chunks = gameObject.GetComponentsInChildren<Chunk>();
        StartCoroutine(CheckPosition());

        foreach (var chunk in Chunks)
        {
            _spawnedChunks.Add(chunk);
        }
    }


    
    private IEnumerator CheckPosition()
    {
        while (true)
        {
            foreach (Chunk chunk in Chunks)
            {
                

            
                // Get the chunk's world position
                Vector3 chunkWorldPosition = transform.TransformPoint(chunk.transform.localPosition);

                // Calculate the absolute X and Z distance between the player and the chunk
                //float distanceX = Mathf.Abs(Player.position.x - chunkWorldPosition.x);
                //float distanceZ = Mathf.Abs(Player.position.z - chunkWorldPosition.z);
                float distanceX = Mathf.Abs(Player.position.x - (transform.TransformPoint(chunk.transform.localPosition)).x);
                float distanceZ = Mathf.Abs(Player.position.z - (transform.TransformPoint(chunk.transform.localPosition)).z);


                
                if ((distanceX < innerRadius && distanceZ < innerRadius) && !_spawnedChunks.Contains(chunk))
                {
                    SpawnChunk(chunk); 
                    Debug.Log($"spawned chunk {chunk.ID} at {distanceX}, {distanceZ}");
                }
                else if ((distanceX > outerRadius || distanceZ > outerRadius) &&  _spawnedChunks.Contains(chunk))
                {
                    DeleteChunk(chunk);
                    Debug.Log($"destroyed chunk {chunk.ID} at {distanceX}, {distanceZ}");
                }
            }
            // Prevent this from checking every single frame
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

