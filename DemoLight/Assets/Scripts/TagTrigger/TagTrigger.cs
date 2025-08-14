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
        // �ȴ�3��
        yield return new WaitForSeconds(2.5f);
        // �����ִ������Ĵ���
        talkText.text = "�G�٣�������ôû�ѿ�\r\n�ðɣ���������ôд��\r\n��û����";

        StartCoroutine(WaitAndDoSomething2());
    }
    IEnumerator WaitAndDoSomething2()
    {
        // �ȴ�3��
        yield return new WaitForSeconds(4.5f);
        // �����ִ������Ĵ���
        dialogBox.SetActive(false);
    }
}
