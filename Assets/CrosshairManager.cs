using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    public GameObject crosshair;
    
    public void ShowCrosshair(bool isShow)
    {
        crosshair.SetActive(isShow);
    }
}
