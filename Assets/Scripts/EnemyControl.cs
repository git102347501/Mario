using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyControl : MonoBehaviour
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
        if (collision.gameObject.tag.Contains("Bullet"))
        {
            Hp--;
            if (Hp <= 0)
            {
                Death1();
            }
            if (collision.gameObject.tag == "Bullet")
            {
                Destroy(collision.gameObject);
            }
        }
        var list = new List<string>() { "Box", "Prop" };
        if (!list.Contains(collision.gameObject.tag))
        {
            // 反向
            dir = -dir;
        }
    }

    public void Hurt()
    {
        this.Hp--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
   {
        Vector3 direction = collision.gameObject.transform.position - transform.position;
        var control = collision.gameObject.GetComponent<PlayerControl>();
        if (control == null)
        {
            var control1 = collision.gameObject.GetComponent<BigPlayControl>();
            if (control1 != null && collision.tag == "Player" && direction.y > 0.2 && !control1.isDeach && !control1.isInvincible)
            {
                Hp--;
                if (Hp <= 0)
                {
                    Death(collision);
                }
            }
        } 
        else
        {
            Debug.Log(direction.y);
            if ((collision.tag == "Player" || collision.tag == "EnemyBullet") && direction.y > 0 && !control.isDeach && !control.isInvincible)
            {
                Hp--;
                if (Hp <= 0)
                {
                    Death(collision);
                }
            }
        }
    }

    private void Death(Collider2D collision)
    {
        // 死亡动画
        ani.SetTrigger("Die");
        // 死亡
        Destroy(gameObject, 1f);
        // 播放死亡声音
        AudioManager.Instance.PlaySound("stomp");
        // 给玩家一个向上的力
        collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 170f);
        // 删除敌人身上的物理组件
        Destroy(GetComponent<Collider2D>());
        Destroy(rBody);
    }

    public void Death1()
    {
        AudioManager.Instance.PlaySound("kick");
        rBody.AddForce(Vector2.up * 140f);
        rBody.AddForce(Vector2.right * 50f);
        Destroy(GetComponent<Collider2D>());
        // 死亡
        //Destroy(gameObject, 1f);
        // 1s后死亡声音2
        Destroy(gameObject, 2f);
    }
}
