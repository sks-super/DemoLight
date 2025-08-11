using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public bool effectOnBox=false;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();

            playerLife.Die();
        }
        else if(effectOnBox&& collision.gameObject.CompareTag("Box"))
        {
            collision.transform.position = collision.transform.parent.position;
        }

    }


}
