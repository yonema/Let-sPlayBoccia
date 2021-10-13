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
     * @author yoneji
     */
    public class BallThrower : MonoBehaviour
    {
        [SerializeField]
        [Header("SetMinThrowPower")]
        float m_MinThorwPower = 0.5f;  //!< 投げる強さの最小値

        [SerializeField]
        [Header("SetMaxThrowPower")]
        float m_MaxThorwPower = 5.0f;  //!< 投げる強さの最小値

        [SerializeField]
        [Header("SetDownTorqueLen")]
        float m_downTorqueLen = 2500.0f;  //!< 下回転のトルクの大きさ

        [SerializeField]
        [Header("SetArchingAngle")]
        float m_archingAgle = 45.0f;  //!< 山なりの角度の大きさ


        ScreenInput m_screenInput;  //!< スクリーンの入力情報
        Ball m_ball;                //!< ボール
        BallManager m_ballManager;  //!< ボールマネージャー
        ThrowPowerController m_throwPowerController;    //!< 投げるパワーのコントローラー
        SwitchButton m_switchButton;                    //!< ボタン切り替え

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

        /**
         * @brief ボールの回転
         */
        public enum EnBallRotation
        {
            enUpRotation,   //!< 上回転
            enDownRotation  //!< 下回転
        }
        EnBallRotation m_ballRotation = EnBallRotation.enUpRotation;    //!< ボールの回転

        /**
         * @brief 投げ方
         */
        public enum EnThrowType
        {
            enFastBall,     //!< 直球
            enArchingBall   //!< 山なり
        }
        EnThrowType m_thorwType = EnThrowType.enFastBall;   //!< 投げ方

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
                // パワーゲージの増減を開始する。
                m_throwPowerController.StartGauge();
            }

            // 上にフリック入力があったら、かつ、投球可能なら
            if (m_screenInput.GetNowFlick() == ScreenInput.FlickDirection.UP && m_canThrow)
            {
                Debug.Log("FlickUp");

                // 投球する
                Throw();

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

            return;
        }

        /**
         * @brief 投球する
         */
        void Throw()
        {
            // パワーゲージの増減を止めて、パワー率を得る
            float powerRate = m_throwPowerController.EndGauge();

            // パワー率から投げるパワーを計算する
            float power = Mathf.Lerp(m_MinThorwPower, m_MaxThorwPower, powerRate);

            Debug.Log("rate:" + powerRate + "," + "power:" + power);

            // トルク
            Vector3 torque = Vector3.zero;
            // 下回転か？
            if (m_ballRotation == EnBallRotation.enDownRotation)
            {
                // 下回転のトルクを計算
                torque = Vector3.Cross(Camera.main.transform.forward, Vector3.up) * m_downTorqueLen;
            }
            Debug.Log("torque" + torque);


            // 投げる方向
            Vector3 throwDir = Camera.main.transform.forward;
            // Y成分をなくしておく
            throwDir.y = 0.0f;
            // 山なりか？
            if (m_thorwType == EnThrowType.enArchingBall)
            {
                // 山なりの方向を計算
                Vector3 rotAxis = Vector3.Cross(throwDir, Vector3.up);
                throwDir = Quaternion.AngleAxis(m_archingAgle, rotAxis) * throwDir;
            }
            // 正規化する
            throwDir.Normalize();

            // ボールを投げる
            m_ball.Throw(power, torque, throwDir);

            // ボールの静止状態を待つステートへ
            m_state = EnBallThrowerState.enWaitIsSleeping;

            return;
        }

        /**
         * @brief 初期化
         * @param ballManager ボールのマネージャー
         * @param screenInput スクリーンの入力情報
         * @param canvas UIのキャンバス
         */
        public void Init(
            BallManager ballManager,
            ScreenInput screenInput,
            Canvas canvas
            )
        {
            m_ballManager = ballManager;
            m_screenInput = screenInput;
            m_throwPowerController = canvas.GetComponent<ThrowPowerController>();
            m_switchButton = canvas.GetComponent<SwitchButton>();
            m_switchButton.SetBallThrower(this);
            return;
        }

        /**
         * @brief ボールをセットする
         * @param ボール
         */
        public void SetBall(Ball ball)
        {
            m_ball = ball;

            return;
        }

        /**
         * @brief 回転モードを現在のものから変更する処理
         */
        public void ChangeRotation()
        {
            // 上回転か？
            if (m_ballRotation == EnBallRotation.enUpRotation)
            {
                // 上回転なら、下回転に変更する
                m_ballRotation = EnBallRotation.enDownRotation;

            }
            else
            {
                // 下回転なら、上回転に変更する
                m_ballRotation = EnBallRotation.enUpRotation;
            }
            Debug.Log("ChangeRotation:" + m_ballRotation);


            // 回転ボタンを切り替える
            m_switchButton.RotationMode(m_ballRotation);

            return;
        }

        /**
         * @brief 投げ方を現在のものから変更する
         */
        public void ChangeThorwType()
        {
            // 直球か？
            if (m_thorwType == EnThrowType.enFastBall)
            {
                // 直球なら、山なりに変更する
                m_thorwType = EnThrowType.enArchingBall;
            }
            else
            {
                // 山なりなら、直球に変更する
                m_thorwType = EnThrowType.enFastBall;
            }
            Debug.Log("ChangeThorwType:" + m_thorwType);

            // 投げ方のボタンを切り替える
            m_switchButton.ThrowType(m_thorwType);

            return;
        }

    }
}