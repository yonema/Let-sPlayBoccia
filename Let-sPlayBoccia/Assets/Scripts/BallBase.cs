using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 「ボッチャしようぜ」のネームスペース
 */
namespace nsLetsPlayBoccia
{
    /**
     * @brief ボールの基底クラス。ボールの動きの処理を書く。
     * @author 米地真央
     */
    public class BallBase : MonoBehaviour
    {


        private Rigidbody m_rigibody;   //!< リジッドボディ

        // Start is called before the first frame update
        // 基底クラスのStart関数は派生クラスの方で呼ばないといけない。
        protected void Start()
        {
            //Debug.Log("BaseStart");
            // リジッドボディのコンポーネントを取得
            m_rigibody = GetComponent<Rigidbody>();
            // 最初は重力を適用しない
            m_rigibody.useGravity = false;

            return;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /**
         * @brief 投げる処理
         * @param[in] power 投げる力 
         */
        protected void Throw(float power)
        {
            // 投げる瞬間に重力を適用する
            m_rigibody.useGravity = true;

            // 力を加える
            // Impulse:質量を考慮して、瞬間的に力を与えるモード
            m_rigibody.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);

            return;
        }
    }
}