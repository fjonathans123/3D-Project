using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    public GameObject crosshair;
    
    public void ShowCrosshair()
    {
        crosshair.SetActive(true);
    }

    public void HideCrosshair()
    {
        crosshair.SetActive(false);
    }
}
