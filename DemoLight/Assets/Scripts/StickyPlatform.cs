using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{

    private Transform boxParent;//假设浮台上最多只有一个箱子

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.SetParent(transform);
        }
        else if(collision.transform.CompareTag("Box"))
        {
            boxParent = collision.transform.parent;
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" )
        {
            collision.gameObject.transform.SetParent(null);
        }
        else if (collision.transform.CompareTag("Box"))
        {
            collision.gameObject.transform.SetParent(boxParent);
            boxParent = null;
        }
    }


}
