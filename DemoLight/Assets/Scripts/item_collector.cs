using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AllControl;

public class item_collector : MonoBehaviour
{
    // ����һ�����л�ȡ�Ʒְ�����
    int cherries = GameManager.Instance.score;


    [SerializeField] private Text cherriesText;
    [SerializeField] private AudioSource collectSoundEffect;

    private void Start()
    {
        cherriesText.text = "��ǰ�÷֣�" + cherries;
        GameManager.Instance.score = cherries;

    }

    public void Score()
    {
        collectSoundEffect.Play();
        cherries++;
        cherriesText.text = "��ǰ�÷֣�" + cherries;

        GameManager.Instance.score = cherries;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cherry"))
        {
            collectSoundEffect.Play();
            Destroy(collision.gameObject);
            cherries++;
            cherriesText.text = "��ǰ�÷֣�" + cherries;

            GameManager.Instance.score = cherries;


        }
    }
}
