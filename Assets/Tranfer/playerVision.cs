using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerVision : MonoBehaviour
{
    public float MouseSensitivity = 150;
    public Transform playerBody;
    float xRotation = 0f;
    public conversationStarter cs;
    private pauseMenuController pause;
    // Start is called before the first frame update
    void Start()
    {
        pause = GameObject.FindGameObjectWithTag("UI").GetComponent<pauseMenuController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
        if (cs != null)
        {
            if (cs.inConversation == false)
            {
                xRotation -= MouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * MouseX);
            }
        }
        else
        {
            if (!pause.isinventory)
            {
                xRotation -= MouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * MouseX);
            }
            
        }
       
    }
}
