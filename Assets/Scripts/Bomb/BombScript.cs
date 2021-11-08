using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public Collider2D cldr;
    public Rigidbody2D rb;

    public Collider2D wallCollider;
    public Collider2D collisionTrigger;

    public float speed = 5.0f;

    private List<Collider2D> cldr_overlap = new List<Collider2D>();
    public ContactFilter2D contactFilter;

    public float bombTime;

    public static FloorScript floorScript;

    Vector3 movementDirection;
    public Transform movePoint;

    List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();

    public GameObject explosion;
    public PlayerController player;

    private IEnumerator bombTimer;
    private IEnumerator BombTimer()
    {
        float timeRemaining = bombTime;
        Debug.Log("starting timer: " + timeRemaining);
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1.0f);
            timeRemaining -= 1.0f;
        }

        yield return new WaitUntil(() => (Vector3.Distance(transform.position, floorScript.CenterObject(gameObject)) <= 0.05f));
        Explode();
    }

    // Start is called before the first frame update
    void Start()
    {
        contactFilter.SetLayerMask(LayerMask.GetMask("Player"));
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = false;

        movementDirection = Vector3.zero;
        movePoint.parent = null;

        cldr.OverlapCollider(contactFilter, cldr_overlap);
        foreach (Collider2D x in cldr_overlap)
        {
            Physics2D.IgnoreCollision(x, cldr, true); // Players will not get "jammed" into the collider immediately
            Physics2D.IgnoreCollision(x, wallCollider, true);
        }

        StartCoroutine(BombTimer());
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            Collider2D collisionX = null;
            Collider2D collisionY = null;
            Collider2D collisionD = null;

            if (movementDirection.magnitude > 0)
            {
                collisionD = Physics2D.OverlapCircle(movePoint.position + movementDirection, 0.2f, LayerMask.GetMask("Walls"));
                if (Mathf.Abs(movementDirection.x) > 0)
                {
                    collisionX = Physics2D.OverlapCircle(movePoint.position + new Vector3(movementDirection.x, 0), 0.2f, LayerMask.GetMask("Walls"));
                }
                if (Mathf.Abs(movementDirection.y) > 0)
                {
                    collisionY = Physics2D.OverlapCircle(movePoint.position + new Vector3(0, movementDirection.y), 0.2f, LayerMask.GetMask("Walls"));
                }

                //Debug.Log("X: " + (bool)collisionX + ", Y: " + (bool)collisionY + ", D: " + (bool)collisionD);

                // if the tile at the next move direction is a wall
                if (collisionD || collisionX || collisionY)
                {
                    movementDirection = Vector3.zero;
                }
                else
                {
                    movePoint.position += movementDirection;
                }
            }
        }
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with " + collision.collider.name);
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Debug.Log("Wall found " + collision.gameObject.name);
            movementDirection = Vector3.zero;
            transform.position = floorScript.CenterObject(this.gameObject);
        }
    }
    */

    private void OnTriggerExit2D(Collider2D collision)
    {
        Physics2D.IgnoreCollision(collision, cldr, false);
    }

    public void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);

        player.maxBombs++; // allow the player who placed the bomb to place another one
        player.bombUI.UpdateUI();

        Destroy(gameObject);
        Destroy(movePoint.gameObject);

        Debug.Log("Bomb " + name + ": BOOM!");
    }

    public void Kick(Vector2 direction)
    {
        movementDirection = direction;
        Debug.Log("bomb facing " + movementDirection.ToString());
    }
}
