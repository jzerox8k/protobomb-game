using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FuelMeter : MonoBehaviour
{
    public Slider slider;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetFuel(int fuel)
    {
        slider.value = fuel;
    }

    public void SetMaxFuel(int maxFuel)
    {
        slider.maxValue = maxFuel;
        slider.value = maxFuel;
    }
}
