using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private int explosionPower;
    public int explosionDamage = 10;

    private Dictionary<Vector3, RaycastHit2D[]> explosionHits = new Dictionary<Vector3, RaycastHit2D[]>();

    public Collider2D explosionTrigger;

    public float chainReactionDelay = 0.1f; 

    private IEnumerator DelayExplosion(BombScript bs)
    {
        yield return new WaitForSeconds(chainReactionDelay);
        if (bs)
        {
            bs.Explode();
        }
    }

    private IEnumerator DelayExplosion(WallScript ws, Vector3 position)
    {
        yield return new WaitForSeconds(chainReactionDelay);
        
        ws.DestroyBlock(position);
    }

    // Start is called before the first frame update
    void Start()
    {
        ExplosionHit();
        Destroy(gameObject, 1.25f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ExplosionHit()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                explosionHits[new Vector3(x, y)] = (Physics2D.CircleCastAll(transform.position, .1f, new Vector2(x, y), 1f, LayerMask.GetMask("Player", "Items", "Walls")));
            }
        }

        foreach (Vector3 position in explosionHits.Keys)
        {
            foreach (RaycastHit2D explosion in explosionHits[position])
            {
                // FIXME: Collider detection is inconsistent among various objects

                // FIXED: 
                // Detect all colliders in range
                // Apply specific functionality using TryGetComponent()
                // Ignore collider if null

                GameObject collisionObject = explosion.collider.gameObject;

                if (!collisionObject) { continue; }

                Debug.Log("Collided with " + collisionObject.name + " at relative position" + position.ToString());
                if (collisionObject.TryGetComponent(out PlayerController pc))
                {
                    //Debug.Log("Player");
                    pc.TakeDamage(explosionDamage);
                }
                else if (collisionObject.TryGetComponent(out BombScript bs))
                {
                    //Debug.Log("Other Bomb");
                    StartCoroutine(DelayExplosion(bs));
                }
                else if (collisionObject.TryGetComponent(out WallScript ws))
                {
                    //Debug.Log("Block");
                    StartCoroutine(DelayExplosion(ws, (transform.position + position)));
                }
                else
                {
                    Debug.Log("Script compononent in collision was not detected");
                }

            }
        }
    }
}
