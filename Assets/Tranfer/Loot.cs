using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public GameObject lootitems;
    public string lootname;
    public int dropChance;

    public Loot(string lootname, int dropChance)
    {
        this.lootname = lootname;
        this.dropChance = dropChance;
    }
}
