using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 「ボッチャしようぜ」のネームスペース
 */
namespace nsLetsPlayBoccia
{

    /**
     * @brief ゲーム中使用するボールクラス
     * @author 米地真央
     */
    public class GameBall : BallBase
    {

        ScreenInput m_screenInput;  //!< スクリーンの入力情報

        float m_throwPower = 0.0f;      //!< 投げるパワー
        bool m_canThrow = false;        //!< 投球可能か？

        // Start is called before the first frame update
        void Start()
        {
            // 基底クラスのStart関数は派生クラスの方で呼ばないといけない。
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {

            // 上にフリック入力があったら、かつ、投球可能なら
            if (m_screenInput.GetNowFlick() == ScreenInput.FlickDirection.UP && m_canThrow)
            {
                Debug.Log("FlickUp");

                m_throwPower = 1.0f;

                // 投げる
                Throw(m_throwPower);
            }

            // 画面に指が触れていない時
            if (MyGodTouch.GetPhase() == GodPhase.None)
            {
                // 投球不可能にする
                m_canThrow = false;
            }

            return;
        }

        /**
         * @brief スクリーンの入力情報をセット
         */
        public void SetScreenInput(ScreenInput screenInput)
        {
            m_screenInput = screenInput.GetComponent<ScreenInput>();

            return;
        }

        /**
         * @brief 画面から押されたときに呼ばれるイベント
         * @warning この関数の名前を変更した場合、PrefabのGameBallのEventTriggerコンポーネントの
         * パラメータも変更しなければいけない。
         */
        public void OnPointerDownEvent()
        {
            Debug.Log("PointerDown");
            // 投球可能にする
            m_canThrow = true;
        }
    }
}