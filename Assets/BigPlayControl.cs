using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPlayControl : MonoBehaviour
{
     // 血量
    public int Hp = 1;
    // 刚体组件
    public Rigidbody2D rBody { get; set; }
    // 动画组件
    public Animator ani { get; set; }
    // 是否在地面
    private bool isGround;
    // 是否死亡
    public bool isDeach;
    // 是否无敌
    public bool isInvincible = false;
    // 死亡变化对象
    public GameObject nextPrefab;
    private GameObject gameObject1;

    public float fireRate = 0.5F;//0.5秒实例化一个子弹
    private float nextFire = 0.0F;
    public GameObject bulletPrefab;
    public bool Right = false;


    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        gameObject1 = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0)
        {
            return;
        }
        // 移动
        if (Input.GetKeyUp(KeyCode.S) && isGround)
        {
            ani.SetBool("isDown", false);
        }
        // 水平轴 -1
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0)
        {
            ani.SetBool("isDown", false);
            isGround = true;
            // 结束跳跃动画
            ani.SetBool("isJump", false);
            // 在移动
            var v = rBody.velocity;
            v.x = horizontal * 1;
            rBody.velocity = v;
            Right = horizontal > 0;
            bulletPrefab.gameObject.GetComponent<BulletControl>().Right = horizontal > 0;
            // 转向
            GetComponent<SpriteRenderer>().flipX = horizontal > 0 ? false : true;
            // 播放跑步动画
            ani.SetBool("isRun", true);
        }
        else
        {
            // 停止
            ani.SetBool("isRun", false);
        }
        // 跳跃
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rBody.AddForce(Vector2.up * 200);
            AudioManager.Instance.PlaySound("big_jump");
        }
        // 掉出地图死亡
        if (rBody.position.y < -1.4f && !isDeach)
        {
            Narrow();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Shoot(horizontal > 0);
        }
        if (Input.GetKeyDown(KeyCode.S) && isGround)
        {
            ani.SetBool("isDown", true);
        }
    }

    public void Shoot(bool fx)
    {
        if (Time.time > nextFire)//让子弹发射有间隔
        {
            // 播放声音
            AudioManager.Instance.PlaySound("kick");
            nextFire = Time.time + fireRate;//Time.time表示从游戏开发到现在的时间，会随着游戏的暂停而停止计算。
            var pos = transform.position;
            Debug.Log(pos.x + "|" + pos.y);
            if (Right)
            {
                pos.x += 0.1f;
            } else
            {
                pos.x -= 0.1f;
            }
            Instantiate(bulletPrefab, pos, transform.rotation);
        }
    }


    // 进入触发 脚下踩东西
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 direction = collision.gameObject.transform.position - transform.position;
        // 如果踩到地面
        if (collision.tag != "Enemy")
        {
            isGround = true;
            // 结束跳跃动画
            ani.SetBool("isJump", false);
        }
    }

    // 离开触发 脚下离开东西
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGround = false;
        // 跳跃动画
        ani.SetBool("isJump", true);
    }

    // 如果碰到敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果碰到敌人，大玛丽变小
        if (collision.collider.tag == "Enemy")
        {
            Hp--;
            if (Hp <= 0)
            {
                Narrow();
            }
        }
    }

    /// <summary>
    /// 变成小玛利
    /// </summary>
    private void Narrow()
    {
        // 静止
        rBody.velocity = Vector2.zero;
        // 播放缩小动画
        ani.SetBool("isDie", true);
        // 播放缩小音效
        AudioManager.Instance.PlaySound("pipe");
        // 创建小玛丽
        var obj = Instantiate(nextPrefab, transform.position, Quaternion.identity);
        obj.SendMessage("StartInvincible", 2f);
        if (gameObject1 != null)
        {
            // 转换镜头追踪对象
            var control = gameObject1.GetComponent<CameraControl>();
            if (control != null)
            {
                control.target = obj.transform;
            }
        }
        // 销毁自身
        Destroy(gameObject);
    }
}
