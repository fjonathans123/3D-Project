using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class gunScript : MonoBehaviour
{
    public float range = 100f, meleeRange;
    public Camera fpsCamera;
    public GameObject bullet;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameManager gm;
    private krocoscript enemy;
    public pauseMenuController pause;

    public Transform impactSpawn;

    private playeMovement player;

    private ElementController element;

    [SerializeField] public Animator anim;

    public float timeBetweenShooting, spread, timeBetweenShots;
    public int bulletPerTap, MaxAmmo, bulletShots;
    public bool allowHoldButton;

    bool shooting, readyToShoot;

    public recoil recoil;

    [Header("Realoading System")]
    public int magazineSize, totalAmmo;
    public float reloadTime;
    public TextMeshProUGUI ammonText;
    private bool isReloading = false;
    private int currentAmmoMagazine;

    public conversationStarter cs;

    private gameData data;
    public Weaponswap weaponType;
    public int damage;

    public Animator handAnim, gunAnim;

    private void Buttoninput()
    {
        if (allowHoldButton)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
    }
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemies").GetComponent<krocoscript>();
        pause = GameObject.FindGameObjectWithTag("UI").GetComponent<pauseMenuController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<playeMovement>();
        element = GameObject.FindGameObjectWithTag("Player").GetComponent<ElementController>();
        data = GameObject.FindGameObjectWithTag("stats").GetComponent<gameData>();
        readyToShoot = true;
        currentAmmoMagazine = magazineSize;
        totalAmmo = MaxAmmo - magazineSize;
    }

    private void Update()
    {
        if (cs != null)
        {
            if (cs.inConversation == false  && !pause.isinventory)
            {
                prepareShoot();
            }
        }
        else
        {
            prepareShoot();
        }
    }

    // Update is called once per frame
    void prepareShoot()
    {
        if (!pause.isPaused)
        {
            DisplayAmmo();
            Aim();
            Buttoninput();
            if (readyToShoot && shooting && currentAmmoMagazine > 0)
            {
                if (element.element == 2 && gm.FireMana > 0f)
                {
                    gm.FireMana -= 0.01f;
                }
               
                if (element.element == 3 && gm.WaterMana > 0f)
                {
                    gm.WaterMana -= 0.01f;
                }

                if (element.element == 4 && gm.LightningMana > 0f)
                {
                    gm.LightningMana -= 0.01f;
                }

                if ((gm.FireMana <= 0f && element.element == 2) || 
                    (gm.WaterMana <= 0f && element.element == 3) || 
                    (gm.LightningMana <= 0f && element.element == 4))
                {
                        element.element = 1;
                }
                bulletShots = bulletPerTap;

                StopCoroutine(player.ActionSlow(.03f));
                StartCoroutine(player.ActionSlow(.03f));
                Shoot();
                anim.SetBool("IsShooting", true);
                StartCoroutine(waitShooting(0.2f));
                
            }

            if (currentAmmoMagazine == 0 && !isReloading && currentAmmoMagazine < magazineSize && totalAmmo > 0)
            {
                StartCoroutine(Reload());
            }

            if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmoMagazine < magazineSize && totalAmmo > 0)
            {
                StartCoroutine(Reload());
            }

            if(Input.GetButtonDown("Fire2"))
            {
                HitMelee();
            }
        }
        
    }

    void HitMelee()
    {
        RaycastHit hit; 
        if(Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, meleeRange))
        {
            krocoscript enemy = hit.transform.GetComponent<krocoscript>();
            dragonScript dragon = hit.transform.GetComponent<dragonScript>();

            if (enemy !=null)
            {
                //enemy.isHitting = true; 
                enemy.TakeDamage(1);
            }

            if(dragon != null)
            {
                dragon.isHitting = true;
                if (dragon.DragonType == 0 && dragon.DragonHP <= dragon.lowHP) 
                {
                    dragon.TakeDamage(999);
                }
            }
        }
    }

    void Aim()
    {
        RaycastHit aim;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out aim, range))
        {
            krocoscript enemy = aim.transform.GetComponent<krocoscript>();
            dragonScript dragon = aim.transform.GetComponent<dragonScript>();

            if (enemy != null)
            {
                gm.enemyHPTxt.text = "Enemy HP: " + enemy.enemyHP;
            }
            else gm.enemyHPTxt.text = "";

            if (dragon != null)
            {
                gm.enemyHPTxt.text = "Enemy HP: " + dragon.DragonHP;
            }

        }
    }

    void Shoot()
    {
        readyToShoot = false;

        currentAmmoMagazine--;
        bulletShots--;
        anim.SetBool("IsShooting", true);


        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);   
        
        Vector3 direction = fpsCamera.transform.forward + new Vector3(x, y, 0);
        soundManager.PlaySound("playerShoot");
        muzzleFlash.Play();
        RaycastHit hit;
        BulletSpawn();
        recoil.RecoilFire();
        if (Physics.Raycast(fpsCamera.transform.position, direction, out hit, range))
        {
            krocoscript enemy = hit.transform.GetComponent<krocoscript>();
            dragonScript dragon = hit.transform.GetComponent<dragonScript>();
            Debug.Log("Dragon takes damage");

            if (enemy != null)
            {
               if(weaponType.selectedweapon == 0)
                {
                    enemy.TakeDamage(data.levelwep1 * damage);
                }
                if (weaponType.selectedweapon == 1)
                {
                    enemy.TakeDamage(data.levelwep2 * damage);
                }
                if (weaponType.selectedweapon == 2)
                {
                    enemy.TakeDamage(data.levelwep3 * damage);
                }

            }
            if (dragon != null)
            {
                if (weaponType.selectedweapon == 0)
                {
                    dragon.TakeDamage(data.levelwep1 * damage + 3);
                }
                if (weaponType.selectedweapon == 1)
                {
                    dragon.TakeDamage(data.levelwep2 * damage + 3);
                }
                if (weaponType.selectedweapon == 2)
                {
                    dragon.TakeDamage(data.levelwep3 * damage + 3);
                }

            }
            GameObject impacts = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            impacts.transform.parent = hit.transform;
            Destroy(impacts, 2f);

        }
        Invoke("ResetShot", timeBetweenShooting);

        if (MaxAmmo > 0 && bulletShots > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true; 
    }

    private IEnumerator waitShooting(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("IsShooting", false);
    }

    void BulletSpawn()
    {
        Rigidbody rb = Instantiate(bullet, impactSpawn.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 380f, ForceMode.Impulse);
    }

    void DisplayAmmo()
    {
        if(ammonText != null)
        {
            ammonText.text = currentAmmoMagazine + " / " + (totalAmmo/bulletPerTap);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        handAnim.SetBool("IsReloading", true);
        gunAnim.SetBool("IsReloading", true);
        AnimatorClipInfo[] handClip = handAnim.GetCurrentAnimatorClipInfo(0);
        AnimatorClipInfo[] gunClip = handAnim.GetCurrentAnimatorClipInfo(0);

        foreach(var clip in handClip)
        {
            
            if (clip.clip.name == "Reload")
            {
                reloadTime = clip.clip.length;
                break;
            }
        }

        if (reloadTime == 0f)
        {
            yield break;

        }
        Debug.Log("test");

        yield return new WaitForSeconds(reloadTime);
        

        int ammoReload = Mathf.Min(magazineSize - currentAmmoMagazine, totalAmmo);

        currentAmmoMagazine += ammoReload;
        totalAmmo -= ammoReload;

        handAnim.SetBool("IsReloading", false);
        gunAnim.SetBool("IsReloading", false);


        isReloading = false;
    }
}
