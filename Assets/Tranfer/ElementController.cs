using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementController : MonoBehaviour

{
    public float element = 1; //0 = fire, 1 = water, 3 = lightning
    private pauseMenuController pause;
    public Weaponswap switches;


    // Start is called before the first frame update
    void Start()
    {
        pause = GameObject.FindGameObjectWithTag("UI").GetComponent<pauseMenuController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (switches.selectedweapon < 3)
        {
            if (pause.isPaused == false)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    element++;
                    //element += (Input.GetAxis("Mouse ScrollWheel") * 10);
                    if (element > 4)
                    {
                        element = 1;
                    }
                    else if (element < 1)
                    {
                        element = 4;
                    }
                    changeElements();
                }
            }
        }      
    }

    void changeElements()
    {
        switch (element)
        {
            case 1:
                Debug.Log("Normal");
                break;
            case 2:
                Debug.Log("Fire");
                break;
            case 3:
                Debug.Log("water");
                break;
            case 4:
                Debug.Log("lightning");
                break;
            default:
                break;
        }

    }
}
