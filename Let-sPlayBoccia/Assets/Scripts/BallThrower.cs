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

        //!< デフォルトの下回転のトルクの大きさ
        static readonly float m_kDefaultDownTorqueLen = 2500.0f;
        [SerializeField]
        [Header("SetDownTorqueLen")]
        float m_downTorqueLen = m_kDefaultDownTorqueLen;  //!< 下回転のトルクの大きさ

        //!< デフォルトの山なりの角度
        static readonly float m_kDefaultArchingAngle = 45.0f;
        [SerializeField]
        [Header("SetArchingAngle")]
        float m_archingAgle = m_kDefaultArchingAngle;  //!< 山なりの角度の大きさ

        ////!< デフォルトの山なりの投げる高さ
        //static readonly float m_kDefaultArchingThrowHeight = 45.0f;
        //[SerializeField]
        //[Header("SetArchingThrowHeight")]
        //float m_archingThrowPos = m_kDefaultArchingThrowHeight;  //!< 山なりの角度の大きさ



        ScreenInput m_screenInput;  //!< スクリーンの入力情報
        Ball m_ball;                //!< ボール
        BallManager m_ballManager;  //!< ボールマネージャー
        ThrowPowerController m_throwPowerController;    //!< 投げるパワーのコントローラー
        SwitchButton m_switchButton;

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

        public enum EnThrowType
        {
            enFastBall,
            enArchingBall
        }
        EnThrowType m_thorwType = EnThrowType.enFastBall;

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

                // トルク
                Vector3 torque = Vector3.zero;
                if (m_ballRotation == EnBallRotation.enDownRotation)
                {
                    torque = Vector3.left * m_downTorqueLen;
                }
                Debug.Log("torque" + torque);


                Vector3 throwDir = Camera.main.transform.forward;
                throwDir.y = 0.0f;
                if (m_thorwType == EnThrowType.enArchingBall)
                {
                    Vector3 rotAxis = Vector3.Cross(throwDir, Vector3.up);
                    throwDir = Quaternion.AngleAxis(m_archingAgle, rotAxis) * throwDir;
                }
                throwDir.Normalize();

                // 投げる
                m_ball.Throw(power, torque, throwDir);

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
         * @param 回転ボタン切り替え処理
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
        }

        /**
         * @brief 回転ボタンを押したときの処理
         * @warning この関数の名前を変更した場合、GameMainシーンのRotationButtonのOnClickの
         * パラメータも変更しなければいけない。
         */
        public void ChangeRotation()
        {
            if (m_ballRotation == EnBallRotation.enUpRotation)
            {
                m_ballRotation = EnBallRotation.enDownRotation;

            }
            else
            {
                m_ballRotation = EnBallRotation.enUpRotation;
            }
            Debug.Log("ChangeRotation:" + m_ballRotation);


            // 回転ボタンを切り替える
            m_switchButton.RotationMode(m_ballRotation);

            return;
        }

        public void ChangeThorwType()
        {
            if (m_thorwType == EnThrowType.enFastBall)
            {
                m_thorwType = EnThrowType.enArchingBall;
            }
            else
            {
                m_thorwType = EnThrowType.enFastBall;
            }
            Debug.Log("ChangeThorwType:" + m_thorwType);

            m_switchButton.ThrowType(m_thorwType);

            return;
        }

    }
}