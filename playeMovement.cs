using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playeMovement : MonoBehaviour
{
    public float speed = 12f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    public CharacterController controller;
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private GameManager gm;
    Vector3 velocity;
    private gameData stats;
    public bool slowMoTrigger;

    private pauseMenuController pause;

    public bool action;

    [SerializeField] public Animator anim;

    public float maxDashTime = 1.0f;
    public float dashSpeed = 10f;
    public float dashStoppingSpeed = 0.1f;
    private float currentDashTime;

    public GameObject player;
    public Vector3 PlayerPos;



    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        stats = GameObject.FindGameObjectWithTag("stats").GetComponent<gameData>();
        pause = GameObject.FindGameObjectWithTag("UI").GetComponent<pauseMenuController>();
        stats.LoadData();

        currentDashTime = maxDashTime;
        PlayerPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (pause.isPaused == false)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            Jump();
            move();
            if (gm.health <= 0)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                stats.SaveData();
                SceneManager.LoadScene("gametitle");
            }
        }
        else
        {
            Debug.Log("game is paused");   
        }
        
    }
    void move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.E))
        {
            slowMoTrigger = ! slowMoTrigger;
        }

        float time = (slowMoTrigger) ? 1f : .03f;
        float lerpTime = (slowMoTrigger) ? .05f : .5f;

        //float time = (x != 0|| z!=0) ? 1f : .03f;
        //float lerpTime = (x != 0 || z!=0) ? .05f : .5f;

        time = action ? 1 : time;
        lerpTime = action ? .1f : lerpTime;

        Time.timeScale = Mathf.Lerp(Time.timeScale, time, lerpTime);

        if(x > 0.5f || z > 0.5f || x < -0.5f || z < -0.5f)
        {
            anim.SetBool("IsRunning", true);
        }
        if(x == 0 && z== 0)
        {
            anim.SetBool("IsRunning", false);
        }

        Vector3 move; 

        if (Input.GetKeyDown(KeyCode.V))
        {
            currentDashTime = 0.0f;
        }
        if (currentDashTime < maxDashTime)
        {
            move = transform.forward * dashSpeed;
            currentDashTime += dashStoppingSpeed;
        }
        else
        {
            move = transform.right * x + transform.forward * z;
        }
        controller.Move(move * speed * Time.deltaTime);
    }

    public IEnumerator ActionSlow(float time)
    {
        action = true;
        yield return new WaitForSecondsRealtime(.03f);
        action = false; 
    }
   
    void Jump()
    {
        if (isGrounded && velocity.y < 0)
        {
            anim.SetBool("IsJumping", false);
        }
            if (Input.GetButtonDown("Jump") && isGrounded)
        {
            anim.SetBool("IsJumping", true);
            velocity.y = Mathf.Sqrt (jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemies"))
        {
            if(gm.armor > 0f)
            {
                gm.armor -= 3f;
                if (gm.armor < 0f)
                {
                    gm.armor = 0f;
                }
            }
            else
            {
                gm.health--;
                gm.healthcooldown = 5f;
                Debug.Log("damage");
            }
            
        }
    }
}
