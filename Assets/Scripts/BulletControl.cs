using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    [SerializeField] 
    private float speed = 3f;//子弹的速度
    public Rigidbody2D rig;
    // 动画
    private Animator ani;
    public bool Right;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();//获取子弹刚体组件
        rig.velocity = Right ? transform.right * speed : transform.right * (speed * -1);//移动
        Destroy(gameObject, 2f);//2秒后销毁子弹，不然子弹会无限多
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Bullet:"+ Right.ToString());
        transform.Translate((Right ? Vector2.right : Vector2.left) * 1f * Time.deltaTime);
    }

    public void Hurt()
    {
        ani.SetTrigger("isDie");
        Destroy(gameObject, 0.2f);
    }
}
