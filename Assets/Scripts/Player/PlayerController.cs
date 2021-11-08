using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform movePoint;
    public float speed;

    public Collider2D cldr;
    private List<Collider2D> cldr_overlap;

    private ContactFilter2D contactFilter;
    private LayerMask layerMask;

    public int fuel;
    private bool hasFuel = true;

    public GameObject bomb;
    public int maxBombs = 3;
    public BombUI bombUI;

    public FloorScript floorScript;
    public WallScript wallScript;

    private Vector3 movementVector = Vector3.zero;
    public Vector3 facing = Vector3.zero;

    public float kickRange = 1.0f;
    List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();
    ContactFilter2D kickContactFilter = new ContactFilter2D();

    public KeyCode BOMB_INPUT = KeyCode.Return;
    public KeyCode KICK_INPUT = KeyCode.X;

    private IEnumerator fuelTimer; // specific timer IEnumerator to be able to track and stop the Coroutine
    private IEnumerator FuelTimer(int amount)
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            fuel += amount;
            fuelMeter.SetFuel(fuel);
            // Debug.Log("fuel: " + fuel);
        }
    }

    public FuelMeter fuelMeter;

    private bool isHandlingInput = false;
    private IEnumerator InputHandler()
    {
        isHandlingInput = true;
        //Debug.Log("isHandlingInput: " + isHandlingInput);

        yield return new WaitForSecondsRealtime(0.05f);

        movementVector.Set(Mathf.Round(Input.GetAxisRaw("Horizontal")), Mathf.Round(Input.GetAxisRaw("Vertical")), 0);
        //Debug.Log("movement axis corrected for timing: " + movementVector.ToString());

        if (Vector3.Distance(rb.position, movePoint.position) <= 0.05f)
        {
            if (movementVector.magnitude > 0)
            {
                facing = movementVector;

                Collider2D collisionX = Physics2D.OverlapCircle(movePoint.position + new Vector3(movementVector.x, 0), 0.2f, LayerMask.GetMask("Walls"));
                Collider2D collisionY = Physics2D.OverlapCircle(movePoint.position + new Vector3(0, movementVector.y), 0.2f, LayerMask.GetMask("Walls"));
                Collider2D collisionD = Physics2D.OverlapCircle(movePoint.position + movementVector, 0.2f, LayerMask.GetMask("Walls"));

                // if the diagonal is active and only one of X or Y is also active
                // or if the diagonal is inactive
                if ((collisionD && (!collisionY || !collisionX) && (collisionY || collisionX)) || !collisionD)
                {
                    if (!collisionX)
                    {
                        movePoint.position += new Vector3(movementVector.x, 0, 0);
                    }

                    if (!collisionY)
                    {
                        movePoint.position += new Vector3(0, movementVector.y, 0);
                    }
                }
            }
        }

        isHandlingInput = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;

        fuel = 60;
        fuelTimer = FuelTimer(-1);
        StartCoroutine(fuelTimer);

        maxBombs = 3;

        cldr_overlap = new List<Collider2D>();

        contactFilter.SetLayerMask(LayerMask.GetMask("Floor"));
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = true;

        BOMB_INPUT = KeyCode.Return;
        KICK_INPUT = KeyCode.X;

        fuelMeter.SetMaxFuel(60);
    }

    // Update is called once per frame
    void Update()
    {
        // kinematic movement
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        if (!isHandlingInput)
        {
            movementVector.Set(Mathf.Round(Input.GetAxisRaw("Horizontal")), Mathf.Round(Input.GetAxisRaw("Vertical")), 0);

            if (movementVector.magnitude > 0f)
            {
                StartCoroutine(InputHandler());
            }
        }

        // out of fuel check
        if (fuel <= 0 && hasFuel)
        {
            StopCoroutine(fuelTimer);
            Debug.Log("out of fuel!");
            hasFuel = false;
        }

        // placing a bomb
        if (Input.GetKeyDown(BOMB_INPUT) && maxBombs > 0)
        {
            if (floorScript.PlaceObject(bomb, transform.position, gameObject))
            {
                maxBombs--;
                bombUI.UpdateUI();
            }
        }

        // kicking a bomb
        if (Input.GetKeyDown(KICK_INPUT))
        {
            kickContactFilter.SetLayerMask(LayerMask.GetMask("Items"));
            cldr.Raycast(facing, kickContactFilter, raycastHits, kickRange * facing.magnitude);

            Debug.Log("Kick raycast found " + raycastHits.Count + " items");

            if (raycastHits.Count > 0)
            {
                if (raycastHits[0].collider.TryGetComponent(out BombScript bs))
                {
                    bs.Kick(facing);
                }
            }

            /*
            foreach (RaycastHit2D hit in raycastHits) {
                if (hit.collider.TryGetComponent(out BombScript bs)) {
                    bs.Kick(facing);
                }
            }
            */
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entered trigger: " + collision.name);
    }

    public void TakeDamage(int damage)
    {
        fuel -= damage;
        fuelMeter.SetFuel(fuel);
    }
}