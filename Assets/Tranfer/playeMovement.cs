using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

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
    public Vector3 velocity;
    private gameData stats;
    public bool slowMoTrigger;

    private pauseMenuController pause;

    public bool action;

    [SerializeField] public Animator anim;

    [Header("dashing")]
    public float dashDistance, dashDuration;
    public Vector3 move;
    public bool isDashing = false;
    public Camera cam;
    public int dashCounter = 0;
    public float cooldownDash = 3f;
    private bool canDash = true;

    public float maxDashTime = 1.0f;
    public float dashSpeed = 10f;
    public float dashStoppingSpeed = 0.1f;
    private float currentDashTime;

    public GameObject player;
    public Vector3 PlayerPos;

    [Header("Vaulting")]
    public float vaultForce = 5f;
    public float vaultHeight = 1.5f;
    public LayerMask vaultLayerMask;
    public bool isVaulting = false;

    public LayerMask itemLayer;

    public GameObject inventoryPanel;


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
            Move();
            if(Input.GetKeyDown(KeyCode.T))
            {
                GrabItemQuest();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                pause.isinventory = true;
                inventoryPanel.SetActive(true);
            }

            if (gm.health <= 0)
            {
                stats.SaveData();
                SceneManager.LoadScene(0);
                Cursor.visible = true;
                SceneManager.LoadScene("GameOver");
            }
        }
        else
        {
            //Debug.Log("game is paused");   
        }
        
    }

    void GrabItemQuest()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10f, itemLayer))
        {
            Itemcollectiontrigger item = hit.collider.GetComponent<Itemcollectiontrigger>();
            if(item != null)
            {
                item.CollectItem();
            }
        }
    }
    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.E))
        {
            slowMoTrigger = ! slowMoTrigger;
        }

        float time = (slowMoTrigger) ? .03f : 1f;
        float lerpTime = (slowMoTrigger) ? .5f : .05f;

        //float time = (x != 0|| z!=0) ? 1f : .03f;
        //float lerpTime = (x != 0 || z!=0) ? .05f : .5f;

        time = action ? 1 : time;
        lerpTime = action ? .1f : lerpTime;

        Time.timeScale = Mathf.Lerp(Time.timeScale, time, lerpTime);

        if(x == 0 && z== 0)
        {
            anim.SetBool("IsRunning", false);
        }
        else
        {
            anim.SetBool("IsRunning", true); 
        }

            move = new Vector3(x, 0f, z).normalized;

        if (Input.GetKeyDown(CustomInput.Instance.GetKey("Dash")) && !isDashing && dashCounter < 2 && canDash)
        {
            StartCoroutine(playerDashing());
        }
        else
        {
            move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
        }

        if(!canDash)
        {
            cooldownDash -= Time.deltaTime;
            if(cooldownDash <= 0f)
            {
                canDash = true;
                cooldownDash = 0f;
            }
        }

    }

    public void changeFOV(float endvalue)
    {
        cam.DOFieldOfView(endvalue, 0.25f);
    }

    IEnumerator playerDashing()
    {
        if(dashCounter < 2)
        {
            dashCounter++;
            isDashing = true;
            changeFOV(80f);
            float startTime = Time.time;

            while (Time.time < startTime + dashDuration)
            {
                controller.Move(move * dashDistance * Time.deltaTime);

                yield return null;
            }
            isDashing = false;
            changeFOV(60f);

            if(dashCounter >= 2)
            {
                canDash = false;
                cooldownDash = 3f;
                dashCounter = 0;
            }
        }
        
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
            if (Input.GetKeyDown(CustomInput.Instance.GetKey ("Jump")) && !isVaulting)
        {
            Debug.Log("junmping");

            if (isGrounded)
            {
                anim.SetBool("IsJumping", true);
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            else
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.forward, out hit, vaultHeight, vaultLayerMask))
                {
                    Debug.Log("vault");
                    StartCoroutine(Vault(hit.point));
                }
            }

        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Vault(Vector3 vaultPoint)
    {
        isVaulting = true;
        Vector3 start = transform.position;
        Vector3 end = new Vector3(vaultPoint.x, transform.position.y, vaultPoint.z);
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(start, end);

        Vector3 forwardOffset = transform.forward * 1f;
        Vector3 upwardOffset = Vector3.up * 0.5f;

        while(Time.time < startTime + vaultForce)
        {
            float distanceCovered = (Time.time - startTime) * vaultForce;
            float fracJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fracJourney) + upwardOffset;
            Debug.Log(transform.position);
            yield return null;
        }

        transform.position = end + forwardOffset;
        isVaulting = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemies") || other.CompareTag("enemyMelee"))
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
