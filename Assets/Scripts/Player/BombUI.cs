using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BombUI : MonoBehaviour
{
    private int GLOBAL_MAX_BOMBS = 10;

    public GameObject bombUI;

    public PlayerController player;
    public GameObject bombIcon;

    private List<GameObject> bombIconLayout = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        for (int i = 1; i <= player.maxBombs; i++)
        {
            bombIconLayout.Add(Instantiate(bombIcon, bombUI.transform));
        }
    }

    public void UpdateUI()
    {
        while (bombIconLayout.Count < player.maxBombs && bombIconLayout.Count < GLOBAL_MAX_BOMBS)
        {
            bombIconLayout.Add(Instantiate(bombIcon, bombUI.transform));
        }
        while (bombIconLayout.Count > player.maxBombs && bombIconLayout.Count > 0)
        {
            GameObject remove = bombIconLayout[0];
            bombIconLayout.Remove(remove);
            Destroy(remove);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
