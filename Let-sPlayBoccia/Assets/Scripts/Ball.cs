using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 「ボッチャしようぜ」のネームスペース
 */
namespace nsLetsPlayBoccia
{
    /**
     * @brief ボールクラス。ボールの動きの処理を書く。
     * @author yoneji
     */
    public class Ball : MonoBehaviour
    {


        Rigidbody m_rigibody;   //!< リジッドボディ

        bool m_isTouthed = false;   //!< 画面から触れられているか？

        float m_killY = -10.0f;   //!< キルY。座標がこの高さより下になったら停止


        // Start is called before the first frame update
        // 基底クラスのStart関数は派生クラスの方で呼ばないといけない。
        protected void Start()
        {
            // リジッドボディのコンポーネントを取得
            m_rigibody = GetComponent<Rigidbody>();
            // 最初は重力を適用しない
            m_rigibody.useGravity = false;
            // 最大回転速度を無限にする
            m_rigibody.maxAngularVelocity = Mathf.Infinity;

            return;
        }

        // Update is called once per frame
        void Update()
        {
            // キルYチェック
            if (transform.position.y < m_killY)
            {
                Debug.Log("KillY");
                // 座標をキルYギリギリのところで固定する
                Vector3 pos = transform.position;
                pos.y = m_killY;
                transform.position = pos;
                // 動かないようにする
                m_rigibody.isKinematic = true;
            }
        }

        /**
         * @brief 投げる処理
         * @param power 投げる力 
         * @param torque トルク
         */
        public void Throw(float power, Vector3 torque)
        {
            // 投げる瞬間に重力を適用する
            m_rigibody.useGravity = true;

            // 力を加える
            // Impulse:質量を考慮して、瞬間的に力を与えるモード
            m_rigibody.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);

            m_rigibody.AddTorque(torque,ForceMode.VelocityChange);

            return;
        }

        /**
         * @brief 触れられているか？を得る
         * @return 触れられているか？
         */
        public bool IsTouched()
        {
            return m_isTouthed;
        }

        /**
         * @brief ボールが静止状態になったか？を得る
         * @return 静止状態か？
         */
        public bool IsSleeping()
        {
            return m_rigibody.IsSleeping();
        }

        /**
        * @brief 画面から押されたときに呼ばれるイベント
        * @warning この関数の名前を変更した場合、PrefabのBallのEventTriggerコンポーネントの
        * パラメータも変更しなければいけない。
        */
        public void OnPointerDownEvent()
        {
            Debug.Log("PointerDown");
            // 画面から触れられている
            m_isTouthed = true;
        }

        /**
        * @brief 画面から押されていた状態から離れたときに呼ばれるイベント
        * @warning この関数の名前を変更した場合、PrefabのBallのEventTriggerコンポーネントの
        * パラメータも変更しなければいけない。
        */
        public void OnPointerExitEvent()
        {
            Debug.Log("PointerExit");
            // 画面から触れられていない
            m_isTouthed = false;
        }


    }
}