using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ボールのジェネレーター。
public class GameBallGenerator : MonoBehaviour
{
    // ボールプレハブ
    [SerializeField]
    [Header("SetBallPrefab")]
    GameObject m_ballPrefab;

    // スクリーンの入力情報
    [SerializeField]
    ScreenInput m_screenInput;

    // Start is called before the first frame update
    void Start()
    {
        GenerateBallPrefab();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ボールを生成する
    public void GenerateBallPrefab()
    {
        // ボールのプレハブを生成する
        GameObject ball = Instantiate(m_ballPrefab);
        // ボールの座標を設定する
        ball.transform.position = this.transform.position;
        // ボールにスクリーンの入力情報をセットする
        ball.GetComponent<GameBall>().SetScreenInput(m_screenInput);

        return;
    }
}
