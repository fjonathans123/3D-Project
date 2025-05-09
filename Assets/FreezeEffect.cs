using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffect : MonoBehaviour
{
    public Material defaultMaterial, freezeMaterial;

    private Renderer objectRederer;
    private Material[] originalMaterial;
    public bool isFrozen = false;

    // Start is called before the first frame update
    void Start()
    {
        objectRederer = GetComponent<Renderer>();
        originalMaterial = objectRederer.materials;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFrozen)
        {
            changeMaterials(0, defaultMaterial);
            changeMaterials(1, freezeMaterial);
        }
        else
        {
            resetMaterials();
        }
    }

    void changeMaterials(int i, Material newMaterial)
    {
        if(i < objectRederer.materials.Length)
        {
            Material[] mats = objectRederer.materials;
            mats[i] = newMaterial;
            objectRederer.materials = mats;

        }
    }

    void resetMaterials()
    {
        objectRederer.materials = originalMaterial;
    }
}
