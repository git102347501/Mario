using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PropContol : MonoBehaviour
{
    // 运动线
    public AnimationCurve curve;
    public Rigidbody2D rBody { get; set; }
    public SpringJoint2D SpringJoint { get; set; }

    /// <summary>
    /// 盒子跳动协程函数
    /// </summary>
    /// <returns></returns>
    IEnumerator sample()
    {
        Vector2 pos = transform.position;
        for (float t = 0; t < curve.keys[curve.length - 1].time; t += Time.deltaTime)
        {
            transform.position = new Vector2(pos.x, pos.y + curve.Evaluate(t));
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        SpringJoint = GetComponent<SpringJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Hurt()
    {
        Death();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 direction = collision.gameObject.transform.position - transform.position;

        Vector3 center = collision.collider.bounds.center;
        Vector3 contactPoint = collision.contacts[0].point;

        bool right = contactPoint.x > center.x;
        bool top = contactPoint.y > center.y;
        //Debug.Log(right + "|" + top);
#warning 上下方向判断不准确
        // 如果是马里奥且被从底部击中
        if (collision.transform.tag == "Player" && !right && top)
        {
            switch (collision.transform.gameObject.name)
            {
                // 小玛丽
                case "mario(Clone)":
                    StartCoroutine("sample");
                    break;
                // 大玛丽
                case "bigMario(Clone)":
                    // 大玛丽清除
                    Death();
                    break;
                default:
                    break;
            }
        }
        if (collision.gameObject.tag == "Bullet")//如果碰撞对象是敌人
        {
            Death();
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 direction = collision.gameObject.transform.position - transform.position;
        if (collision.tag == "Player" && direction.y < 0)
        {
            AudioManager.Instance.PlaySound("bump");
            //Invoke("PlayDieSound", 1f);
        } 
    }

    // 碎裂
    private void Death()
    {
        // 播放死亡声音
        AudioManager.Instance.PlaySound("brick_smash");
        Destroy(gameObject);
        //// 给玩家一个向上的力
        //collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 200f);
        // 删除敌人身上的物理组件
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Rigidbody2D>());
    }
}
