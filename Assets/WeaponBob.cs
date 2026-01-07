using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    public float magnitude, idleSpeed, walkSpeedMultiplier, walkSpeedMax, aimReduction;

    public playeMovement player;

    float sinY = 0f;
    float sinX = 0f;
    Vector3 lastPosition;

    public AudioSource footStepSource;
    public AudioClip[] footstepsSound;
    public float stepThreshold = -0.95f;
    private bool stepped = false;
    public float inputthreshold = -0.1f;

    public Terrain terrain;
    public List<TerrainSurface> surfaces = new List<TerrainSurface>();

    [System.Serializable]
    public class TerrainSurface
    {
        public TerrainLayer layer;
        public AudioClip[] footstepsSound;
    }

    public LayerMask surfaceLayer;
    public float surfaceCheckDistance = 1.5f;

    [System.Serializable]

    public class ObjectSurface
    {
        public PhysicMaterial material;
        public AudioClip[] footstepsSound;
    }

    public List<ObjectSurface> objects = new List<ObjectSurface>();

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGrounded)
        {

            float moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).magnitude;
            if(moveInput < inputthreshold)
            {
                stepped = false;
                transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime);
                lastPosition = transform.position;
                return;
            }

            float delta = Time.deltaTime * idleSpeed;
            float velocity = (lastPosition - transform.position).magnitude * walkSpeedMultiplier;
            delta += Mathf.Clamp(velocity, 0, walkSpeedMax);

            sinX += delta / 2;
            sinY += delta;

            float magnitude = this.magnitude;

            transform.localPosition = Vector3.zero + Vector3.up * Mathf.Sin(sinY) * magnitude;
            transform.localPosition += Vector3.right * Mathf.Sin(sinX) * magnitude;

            footStepHandle(velocity);
        }

        else

        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime);
            stepped = false;
        }
        lastPosition = transform.position;
    }

    void footStepHandle(float velocity)
    {
        if(velocity < 0.01f)
        {
            stepped = false;
            return;
        }

        float bob = Mathf.Sin(sinY);

        if (!stepped && bob <= stepThreshold)
        {
            PlayFootSteps();
            stepped = true;
        }
        else if(bob > stepThreshold)
        {
            stepped = false;
        }
    }

    void PlayFootSteps()
    {
        AudioClip[] clips = GetSurfaceClip();
        if (clips == null || clips.Length == 0)
            return;


        footStepSource.pitch = Random.Range(0.85f, 1.15f);
        footStepSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    int GetTextureTerrianAudio()
    {
        Vector3 playerpos = player.transform.position;
        TerrainData terrainData = terrain.terrainData;

        int mapX = (int)(((playerpos.x - terrain.transform.position.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((playerpos.z - terrain.transform.position.z) / terrainData.size.z) * terrainData.alphamapHeight);

        float[,,] splatmap = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        int maxIndex = 0;
        float maxWeight = 0f;

        for(int i = 0; i < terrainData.terrainLayers.Length; i++)
        {
            maxWeight = splatmap[0, 0, i];
            maxIndex = i;
        }

        return maxIndex;
    }

    AudioClip[] GetTerrainClip()
    {
        int idx = GetTextureTerrianAudio();

        foreach(var s in surfaces)
        {
            if(terrain.terrainData.terrainLayers[idx] == s.layer)
            {
                return s.footstepsSound;
            }
        }
        return footstepsSound;
    }

    AudioClip[] GetSurfaceClip()
    {
        RaycastHit ray;
        if (Physics.Raycast(player.groundCheck.position, Vector3.down, out ray, surfaceCheckDistance, surfaceLayer))
        {
            if(!(ray.collider is TerrainCollider))
            {
                PhysicMaterial mat = ray.collider.sharedMaterial;

                if(mat != null)
                {
                    foreach (var s in objects)
                    {
                        if (s.material == mat)
                            return s.footstepsSound;
                    }
                }
                return footstepsSound;
            }

            return GetTerrainClip();
        }
        return footstepsSound;
    }
}
