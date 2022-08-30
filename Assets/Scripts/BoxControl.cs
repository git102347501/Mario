using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 direction = collision.gameObject.transform.position - transform.position;
        Console.WriteLine(collision.gameObject.name);
        if (collision.transform.tag == "Player" && direction.y < 0)
        {
            //Destroy(gameObject, 1f);
            //// 给玩家一个向上的力
            //collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 200f);
            // 删除敌人身上的物理组件
            //Destroy(GetComponent<Collider2D>());
            //Destroy(GetComponent<Rigidbody2D>());
        }
    }
}
