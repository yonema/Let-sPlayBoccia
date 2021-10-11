using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲーム中使用するボールクラス
public class GameBall : BallBase
{
    // スクリーンの入力情報
    ScreenInput m_screenInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_screenInput.GetNowFlick() == ScreenInput.FlickDirection.UP)
        {
            Debug.Log("Up");
        }
    }

    private void FixedUpdate()
    {
        
    }

    // スクリーンの入力情報をセット
    public void SetScreenInput(ScreenInput screenInput)
    {
        m_screenInput = screenInput.GetComponent<ScreenInput>();
    }
}
