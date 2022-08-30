using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackEggControl : MonoBehaviour
{
    // 血量
    private int Hp = 1;
    // 方向
    private int dir = 1;
    // 动画
    private Animator ani;
    // 刚体组件
    public Rigidbody2D rBody { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0)
        {
            return;
        }

        transform.Translate(Vector2.right * dir * 0.2f * Time.deltaTime);
    }

    // 发生碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 反向
        dir = -dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 direction = collision.gameObject.transform.position - transform.position;
        if (collision.tag == "Player" && direction.y > 0)
        {
            Hp--;
            if (Hp <= 0)
            {
                var s = rBody.velocity;
         
                ani.SetBool("isDie", true);
                //// 3s后复活
                Invoke("PlayDieSound", 3f);
            }
        }
    }

    void PlayDieSound()
    {
        Hp++;
        ani.SetBool("isDie", false);
    }
}
