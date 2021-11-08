using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
    public TileBase[] tileBases;
    public bool isDestructible;
    public bool isExplosive;
}
