using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int score;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.transform.position = new Vector2(129, 480);
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "分数: " + score;
        //text.transform.position = new Vector2(Camera.current.transform.position.x, Camera.current.transform.position.y);
    }
}
