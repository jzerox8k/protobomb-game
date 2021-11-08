using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private List<TileData> tileDataList;

    public Dictionary<TileBase, TileData> tileDataDictionary;

    private void Awake()
    {
        tileDataDictionary = new Dictionary<TileBase, TileData>();

        // TODO: Add a foreach loop to populate the dictionary
        foreach (TileData d in tileDataList)
        {
            foreach (TileBase b in d.tileBases)
            {
                tileDataDictionary.Add(b, d);
                Debug.Log("Adding " + b + ", " + d + ": " + tileDataDictionary[b]);
            }
        }
    }
}
