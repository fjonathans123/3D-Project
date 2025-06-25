using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffect : MonoBehaviour
{
    public Material defaultMaterial, freezeMaterial;

    private Renderer objectRederer;
    private Material[] originalMaterial;
    public bool isFrozen = false;

    private bool isMaterialStored = false;

    // Start is called before the first frame update
    void Start()
    {
        objectRederer = GetComponent<Renderer>();
        storeOriginalMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFrozen)

        {
            if (objectRederer.materials.Length > 0)
            {
                Material[] frozenMaterial = new Material[originalMaterial.Length];
                for (int i = 0; i < frozenMaterial.Length; i++)
                {
                    frozenMaterial[i] = freezeMaterial;
                }
                objectRederer.materials = frozenMaterial;
            }
        }
        else
        {
            RestoreOriginalMaterial();
        }
    }


    void RestoreOriginalMaterial()
    {
        if(isMaterialStored)
        {
            objectRederer.materials = originalMaterial;
        }
    }

    void storeOriginalMaterial()
    {
        if(!isMaterialStored)
        {
            originalMaterial = objectRederer.materials;
            isMaterialStored = true;
        }
    }
}
