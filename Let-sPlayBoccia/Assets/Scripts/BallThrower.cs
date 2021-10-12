using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 「ボッチャしようぜ」のネームスペース
 */
namespace nsLetsPlayBoccia
{

    /**
     * @brief ボールを投げるクラス
     * @author 米地真央
     */
    public class BallThrower : MonoBehaviour
    {

        //!< デフォルトの投げる強さの最小値
        static readonly float m_kDefaultMinThrowPower = 0.5f;
        [SerializeField]
        [Header("SetMinThrowPower")]
        float m_MinThorwPower = m_kDefaultMinThrowPower;  //!< 投げる強さの最小値

        //!< デフォルトの投げる強さの最大値
        static readonly float m_kDefaultMaxThrowPower = 5.0f;
        [SerializeField]
        [Header("SetMaxThrowPower")]
        float m_MaxThorwPower = m_kDefaultMaxThrowPower;  //!< 投げる強さの最小値

        ScreenInput m_screenInput;  //!< スクリーンの入力情報
        Ball m_ball;                //!< ボール
        BallManager m_ballManager;  //!< ボールマネージャー
        ThrowPowerController m_throwPowerController;    //!< 投げるパワーのコントローラー

        bool m_canThrow = false;        //!< 投球可能か？

        /**
         * @brief ボールのステート
         */
        enum EnBallThrowerState
        {
            enBeforeThrow,        //!< 投げる前
            enWaitIsSleeping,     //!< 静止状態を待つ
            enWaitNextTurn,       //!< 次のターンを待つ
        }
        EnBallThrowerState m_state = EnBallThrowerState.enBeforeThrow;  //!< ステート

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // ステートで処理を振り分け
            switch (m_state)
            {
                // 投げる前
                case EnBallThrowerState.enBeforeThrow:
                    // 投げる前のアップデート
                    BeforeThrowUpdate();
                    break;
                // 静止状態を待つ
                case EnBallThrowerState.enWaitIsSleeping:
                    // 静止状態になるまで待つ
                    WaitSleeping();
                    break;
                // 次のターンを待つ
                case EnBallThrowerState.enWaitNextTurn:
                    SetBall(m_ballManager.GenerateBallPrefab());
                    m_state = EnBallThrowerState.enBeforeThrow;
                    break;
            }


            return;
        }

        /**
         * @brief 投げる前の更新
         */
        void BeforeThrowUpdate()
        {
            // ボールがタッチされたか？
            if (m_ball.IsTouched())
            {
                // タッチされたら投球可能にする
                m_canThrow = true;
                // パワーゲージの増減を開始する
                m_throwPowerController.StartGauge();
            }

            // 上にフリック入力があったら、かつ、投球可能なら
            if (m_screenInput.GetNowFlick() == ScreenInput.FlickDirection.UP && m_canThrow)
            {
                Debug.Log("FlickUp");

                // パワーゲージの増減を止めて、パワー率を得る
                float powerRate = m_throwPowerController.EndGauge();

                // パワー率から投げるパワーを計算する
                float power = Mathf.Lerp(m_MinThorwPower, m_MaxThorwPower, powerRate);

                Debug.Log("rate:" + powerRate + "," + "power:" + power);

                // 投げる
                m_ball.Throw(power);

                // ボールの静止状態を待つステートへ
                m_state = EnBallThrowerState.enWaitIsSleeping;
            }


            // 画面に指が触れていない時
            if (MyGodTouch.GetPhase() == GodPhase.None)
            {
                // まだゲージが増減しているか？
                if (m_throwPowerController.IsGaugeChanging())
                {
                    // まだゲージが止まっていないから
                    // パワーゲージの増減を止める
                    m_throwPowerController.EndGauge();
                    // パワーゲージをリセットする
                    m_throwPowerController.ResetGauge();
                }
                // 投球不可能にする
                m_canThrow = false;
            }

            return;
        }

        /**
         * @brief ボールが静止状態になるまで待つ処理
         */
        void WaitSleeping()
        {
            // ボールが静止状態か？
            if (m_ball.IsSleeping())
            {
                Debug.Log("IsSleeping");
                // パワーゲージをリセットする
                m_throwPowerController.ResetGauge();
                // 次のターンを待つステートへ
                m_state = EnBallThrowerState.enWaitNextTurn;
            }
        }

        /**
         * @brief 初期化
         * @param ballManager ボールのマネージャー
         * @param スクリーンの入力情報
         * @param 投げるパワーのコントローラー
         */
        public void Init(
            BallManager ballManager,
            ScreenInput screenInput,
            ThrowPowerController throwPowerController
            )
        {
            m_ballManager = ballManager;
            m_screenInput = screenInput;
            m_throwPowerController = throwPowerController;
            return;
        }

        /**
         * @brief ボールをセットする
         * @param ボール
         */
        public void SetBall(Ball ball)
        {
            m_ball = ball;
        }

    }
}