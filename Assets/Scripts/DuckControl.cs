using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckControl : MonoBehaviour
{
    // 血量
    private int Hp = 1;
    // 方向
    private int dir = 1;
    // 动画
    private Animator ani;
    // 刚体组件
    public Rigidbody2D rBody { get; set; }
    // 是否睡眠
    private bool IsSleep = false;
    private BoxCollider2D box;
    private int test = 0;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsSleep)
        {
            return;
        }
        if (Hp <= 0)
        {
            return;
        }
        Turn(dir);
    }
    private void Turn(float dir)
    {
        if (dir < 0)
        {
            ani.SetBool("isRightRun", false);
            transform.Translate(Vector2.right * dir * 0.2f * Time.deltaTime);
        }
        else
        {
            ani.SetBool("isRightRun", true);
            transform.Translate(Vector2.right * dir * 0.2f * Time.deltaTime);
        }
    }
    // 发生碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 遇到子弹死亡
        if (collision.gameObject.tag == "Bullet")
        {
            Hp--;
            if (Hp <= 0)
            {
                Death1();
            }
            Destroy(collision.gameObject);
        }

        Vector3 direction = collision.gameObject.transform.position - transform.position;
        if (rBody.tag == "Box")
        {
            test++;
            // 人物踩鸭子壳施加力
            if ((direction.x > -0.12f && direction.x < 0.12f) && direction.y > 0.02f && collision.transform.tag == "Player" && test > 2)
            {
                // 改变材质，设置摩擦力0可以无限滚动
                box.sharedMaterial = new PhysicsMaterial2D("EnemyBulletMaterial");
                rBody.tag = "EnemyBullet";
            }
            if (direction.x > 0)
            {
                rBody.AddForce(Vector2.left * 100f);
            }
            else
            {
                rBody.AddForce(Vector2.right * 100f);
            }
        } 
        // 反向
        dir = -dir;
        Turn(dir);
    }

    public void Hurt()
    {
        this.Hp--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 direction = collision.gameObject.transform.position - transform.position;
        var control = collision.gameObject.GetComponent<PlayerControl>();
        // 如果获取到马里奥且鸭子没有被踩扁
        if (control == null && rBody.tag != "EnemyBullet")
        {
            var control1 = collision.gameObject.GetComponent<BigPlayControl>();
            if (control1 != null && collision.tag == "Player" && direction.y > 0.2 && !control1.isDeach && !control1.isInvincible)
            {
                Hp--;
                if (Hp <= 0)
                {
                    Sleep(collision);
                }
            }

            if (collision.tag == "Bullet")
            {
                Hp--;
                if (Hp <= 0)
                {
                    Death1();
                }
            }
        }
        else
        {
            if (collision.tag == "Player" && direction.y > 0 && !control.isDeach && !control.isInvincible && Hp > 0)
            {
                Hp--;
                if (Hp == 0)
                {
                    Sleep(collision);
                }
            }
        }
    }

    /// <summary>
    /// 睡眠
    /// </summary>
    /// <param name="collision"></param>
    private void Sleep(Collider2D collision)
    {
        Debug.Log("Sleep");
        rBody.tag = "Box";
        this.IsSleep = true;
        box.size = new Vector2(box.size.x, 0.1f);
        box.offset = new Vector2(box.offset.x, -0.06f);
        // 播放死亡声音
        AudioManager.Instance.PlaySound("stomp");
        // 死亡动画
        ani.SetBool("isSleep",true);
        ani.SetBool("isRightRun", false);
        // 4秒后复活
        Invoke("ResurRection", 5f);
    }

    public void Death1()
    {
        rBody.AddForce(Vector2.up * 100f);
        rBody.AddForce(Vector2.right * 30f);
        Destroy(GetComponent<Collider2D>());
        // 死亡
        //Destroy(gameObject, 1f);
        // 1s后死亡声音2
        Destroy(gameObject, 2f);
    }

    private void ResurRection()
    {
        this.IsSleep = false;
        this.Hp++;
        box.size = new Vector2(box.size.x, 0.24f);
        rBody.tag = "Enemy";
        box.offset = new Vector2(box.offset.x, 0.0003f);
        ani.SetBool("isSleep", false);
        ani.SetBool("isRightRun", dir > 0);
    }
}
