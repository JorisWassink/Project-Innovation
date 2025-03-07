using UnityEngine;
using System.Collections.Generic;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }

    private List<Collectible> collectibles = new List<Collectible>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeCollectibles();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeCollectibles()
    {
        for (int i = 0; i < 12; i++)
        {
            collectibles.Add(new Collectible(i, $"Collectible {i}"));
        }
    }

    public void CollectItem(int id)
    {
        Collectible collectible = collectibles.Find(c => c.ID == id);
        if (collectible != null && !collectible.IsCollected)
        {
            collectible.IsCollected = true;
            Debug.Log($"{collectible.Name} collected!");
        }
        else
        {
            Debug.LogWarning($"Collectible {id} not found or already collected!");
        }
    }

    public bool IsCollected(int id)
    {
        Collectible collectible = collectibles.Find(c => c.ID == id);
        return collectible != null && collectible.IsCollected;
    }

    public bool AreAllCollected()
    {
        return collectibles.TrueForAll(c => c.IsCollected);
    }
}

