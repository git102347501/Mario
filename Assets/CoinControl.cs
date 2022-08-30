using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinControl : MonoBehaviour
{
    public AnimationCurve curve;
    public Animator ani { get; set; }
    /// <summary>
    /// 盒子跳动协程函数
    /// </summary>
    /// <returns></returns>
    IEnumerator coinsample()
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
        ani = GetComponent<Animator>();
        ani.SetBool("isRun", true);
        StartCoroutine("coinsample");
        AudioManager.Instance.PlaySound("coin");
        ani.SetBool("isRun", false);
        ScoreManager.score++;
        Destroy(gameObject, 0.4f);
    }
}
