using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class posionMushRoomControl : MonoBehaviour
{
    // 触碰后对象
    public GameObject nextPrefab;
    // 刚体组件
    public Rigidbody2D rBody { get; set; }
    // 动画组件
    public Animator ani { get; set; }

    private GameObject gameObject1;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        gameObject1 = GameObject.Find("Main Camera");
    }

    /// <summary>
    /// 触碰变成大玛丽
    /// </summary>
    /// <param name="collision"></param>
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
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
    }
}
