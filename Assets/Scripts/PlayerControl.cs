using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
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
    public bool isInvincible;
    private float timeSpentInvincible;
    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // 如果无敌，开启闪烁效果
        if (isInvincible)
        {
#warning 无敌期间不可碰撞

            //2
            timeSpentInvincible += Time.deltaTime;

            //3
            if (timeSpentInvincible < 3f)
            {
                float remainder = timeSpentInvincible % 0.3f;
                renderer.enabled = remainder > 0.15f;
            }
            //4
            else
            {
                renderer.enabled = true;
                isInvincible = false;
            }
        }


        if (Hp <= 0)
        {
            return;
        }
        // 移动

        // 水平轴 -1
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0)
        {
            // 在移动
            var v = rBody.velocity;
            v.x = horizontal * 1;
            rBody.velocity = v;
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
            rBody.AddForce(Vector2.up * 190);
            AudioManager.Instance.PlaySound("big_jump");
        }

        if (rBody.position.y < -1.4f && !isDeach)
        {
            Death();
        }
    }

    /// <summary>
    /// 开启无敌
    /// </summary>
    void StartInvincible(float time)
    {
        this.isInvincible = true;
        //GetComponent<CapsuleCollider2D>().enabled = false;
        //GetComponent<BoxCollider2D>().isTrigger = false;
        Invoke("EndInvincible", time);
    }

    void EndInvincible()
    {
        //GetComponent<CapsuleCollider2D>().enabled = true;
        //GetComponent<BoxCollider2D>().isTrigger = true;
        this.isInvincible = false;
    }

    // 进入触发 脚下踩东西
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        if (collision.tag == "Ground")
        {
            isGround = false;
            // 跳跃动画
            ani.SetBool("isJump", true);
        }
    }

    // 如果碰到敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 direction = collision.gameObject.transform.position - transform.position;
        // 如果碰到敌人，玩家死亡
        Debug.Log("Player:"+ direction.y);
        if (collision.collider.tag.Contains("Enemy") && direction.y > -0.005f && !isDeach && !isInvincible)
        {
            Hp--;
            if (Hp <= 0)
            {
                Death();
            }
        }

    }

    private void Death()
    {
        isDeach = true;
        // 删除碰撞器
        Destroy(GetComponent<CapsuleCollider2D>());
        // 删除碰撞器
        Destroy(GetComponent<BoxCollider2D>());
        // 播放死亡动画
        ani.SetTrigger("Die");
        // 静止
        rBody.velocity = Vector2.zero;
        // 向上力
        rBody.AddForce(Vector2.up * 200f);
        // 停止播放声音
        AudioManager.Instance.StopSound();
        // 播放死亡声音
        AudioManager.Instance.PlaySound("death");
        //// 1s后死亡声音2
        //Invoke("PlayDieSound", 1f);
    }

    //void PlayDieSound()
    //{
    //    AudioManager.Instance.PlaySound("");
    //}
}
