using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class WallScript : FloorScript
{
    public TileManager tileManager;
    public GameObject explosion;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool DestroyBlock(Vector3 position)
    {
        Vector3Int cell = tilemap.WorldToCell(position);
        TileBase tileBase = tilemap.GetTile(cell);
        if (tileBase)
        {
            Debug.Log("breaking block " + tileBase.name + " at " + cell.ToString());

            TileData td = tileManager.tileDataDictionary[tileBase];

            if (td.isDestructible)
            {
                tilemap.SetTile(cell, null);
            }

            if (td.isExplosive)
            {
                Instantiate(explosion, cell + new Vector3(0.5f, 0.5f), Quaternion.identity);
                tilemap.SetTile(cell, null);
            }

            return true;
        }
        return false;
    }
}
