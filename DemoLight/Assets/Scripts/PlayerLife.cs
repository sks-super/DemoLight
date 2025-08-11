using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Transform rebirthPoint;

    [SerializeField] private Text cherriesText;
    private int dieNumber = 0;
    [SerializeField] private AudioSource deathSoundEffect;

    private bool deaded=false;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();


        cherriesText.text = "死亡次数：" + dieNumber;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Trap"))
    //    {
    //        deathSoundEffect.Play();
    //        Die();
    //    }
    //}

    public void Die()
    {
        if (!deaded)
        {
            deaded = true;
            dieNumber++;
            cherriesText.text = "死亡次数：" + dieNumber;

            deathSoundEffect.Play();

            rb.bodyType = RigidbodyType2D.Static;
            anim.SetTrigger("death");
        }
    }

    public void Rebirth()
    {
        deaded = false;
        transform.position = rebirthPoint.transform.position;
        rb.bodyType = RigidbodyType2D.Dynamic; 

    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}