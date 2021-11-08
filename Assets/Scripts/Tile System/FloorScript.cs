using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorScript : MonoBehaviour
{
    public TilemapCollider2D tilemapCollider;
    public Tilemap tilemap;

    private List<Collider2D> cldr_overlap = new List<Collider2D>();

    private ContactFilter2D contactFilter;
    private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        BombScript.floorScript = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool PlaceObject(GameObject obj, Vector3 position, GameObject placer)
    {
        Vector3Int cell = tilemap.WorldToCell(position);
        GameObject placedObject = Instantiate(obj, tilemap.CellToWorld(cell) + new Vector3(0.5f, 0.5f), Quaternion.identity);

        // TODO: Block PlaceObject from placing an item if there is an item there already
        if (placedObject.TryGetComponent(out Collider2D o))
        {
            Debug.Log("trying to place object" + o.name + "...");

            layerMask = LayerMask.GetMask("Items");
            contactFilter.SetLayerMask(layerMask);
            contactFilter.useLayerMask = true;
            contactFilter.useTriggers = false;

            o.OverlapCollider(contactFilter, cldr_overlap);
            Debug.Log("found " + cldr_overlap.Count + " items");
            foreach (var c in cldr_overlap)
            {
                Debug.Log((c.isTrigger ? "trigger " : "collider ") + c.name);
            }
            if (cldr_overlap.Count > 0)
            {
                Destroy(placedObject);
                return false;
            }
        }

        if (placedObject.TryGetComponent(out BombScript bs))
        {
            bs.player = placer.GetComponent<PlayerController>();
        }

        return true;
    }

    public Vector3 CenterObject(GameObject obj)
    {
        return tilemap.WorldToCell(obj.transform.position) + new Vector3(0.5f, 0.5f);
    }
}
