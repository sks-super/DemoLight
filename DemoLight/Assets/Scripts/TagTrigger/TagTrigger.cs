using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagTrigger : MonoBehaviour
{
    private bool isFirst=true;
    public Text talkText;
    public GameObject dialogBox;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision .transform.CompareTag("Box")&&isFirst)
        {
            isFirst = false;
            dialogBox.SetActive(true);

            StartCoroutine(WaitAndDoSomething());
        }
    }



    IEnumerator WaitAndDoSomething()
    {
        // 等待3秒
        yield return new WaitForSeconds(2.5f);
        // 三秒后执行下面的代码
        talkText.text = "欸嘿，箱子怎么没裂开\r\n好吧，代码是这么写的\r\n那没事了";

        StartCoroutine(WaitAndDoSomething2());
    }
    IEnumerator WaitAndDoSomething2()
    {
        // 等待3秒
        yield return new WaitForSeconds(4.5f);
        // 三秒后执行下面的代码
        dialogBox.SetActive(false);
    }
}
