using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    // 产生金币对象
    public GameObject spawnPrefab;
    // 变成石头对象
    public GameObject nextPrefab;


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && collision.contacts[0].point.y < transform.position.y)
        {
            // 撞击产生金币
            if (spawnPrefab)
            {
                Instantiate(spawnPrefab, transform.position, Quaternion.identity);
            }
            // 被撞击后变成石头盒子
            if (nextPrefab)
            {
                Instantiate(nextPrefab, transform.position, Quaternion.identity);
            }
            // 销毁自身
            Destroy(gameObject);
        }
    }
}
