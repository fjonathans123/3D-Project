using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lootDrop : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootlist = new List<Loot>();
    
    Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> possItem = new List<Loot>();

        foreach(Loot item in lootlist)
        {
            if(randomNumber <= item.dropChance)
            {
                possItem.Add(item);
            }
        }

        if(possItem.Count > 0)
        {
            Loot droppedItem = possItem[Random.Range(0, possItem.Count)];
            return droppedItem;
        }

        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Loot droppedItem = GetDroppedItem(); 

        if (droppedItem != null)
        {
            droppedItemPrefab = droppedItem.lootitems;
            GameObject lootgameobject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
            float dropforce = 0f;
            Vector3 dropDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            lootgameobject.GetComponent<Rigidbody>().AddForce(dropDirection * dropforce, ForceMode.Impulse);
        }
    }
}
