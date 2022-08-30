using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 蘑菇控制器
/// </summary>
public class mushroomControl : MonoBehaviour
{
    // 方向
    private int dir = 1;
    // 触碰后对象
    public GameObject nextPrefab;
    // 刚体组件
    public Rigidbody2D rBody { get; set; }
    // 动画组件
    public Animator ani { get; set; }
    private bool isRun = false;
    private GameObject gameObject1;

    void Start()
    {
        Invoke("Run", 1f);
        AudioManager.Instance.PlaySound("powerup_appears");
        rBody = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        gameObject1 = GameObject.Find("Main Camera");
    }
    void Run()
    {
        this.isRun = true;
    }

    void Update()
    {
        if (isRun)
        {
            transform.Translate(Vector2.right * dir * 0.4f * Time.deltaTime);
        }
    }

    // 发生碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // 蘑菇静止
            rBody.velocity = Vector2.zero;
            // 销毁小玛丽
            Destroy(collision.gameObject);

            // 播放长大音效
            AudioManager.Instance.PlaySound("powerup");
            // 创建大玛丽
            var obj = Instantiate(nextPrefab, transform.position, Quaternion.identity);

            if (gameObject1 != null)
            {
                var control = gameObject1.GetComponent<CameraControl>();
                if (control != null)
                {
                    control.target = obj.transform;
                }
            }
            // 销毁自身
            Destroy(gameObject);
        } else
        {
            // 反向
            dir = -dir;
        }
    }

    /// <summary>
    /// 触碰变成大玛丽
    /// </summary>
    /// <param name="collision"></param>
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
    }
}
