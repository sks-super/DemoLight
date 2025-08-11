using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public static SpriteRenderer lastSave = null;
    public Transform SavePointPosition;

    private bool firstGet=true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (lastSave != null)
            {
                lastSave.color = Color.black;
            }
            lastSave = transform.GetComponent<SpriteRenderer>();
            lastSave.color = Color.white;
            SavePointPosition.position = transform.position;

            if (firstGet)
            {
                collision.GetComponent<item_collector>().Score();
                firstGet = false;
            }
        }
    }
}
