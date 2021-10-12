using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;


/**
 * @brief 「ボッチャしようぜ」のネームスペース
 */
namespace nsLetsPlayBoccia
{
    /**
     * @brief ボールマネージャークラス
     * @author 米地真央
     */
    public class BallManager : MonoBehaviour
    {
        [SerializeField]
        [Header("SetBallPrefab")]
        GameObject m_ballPrefab;            //!< ボールのプレハブ

        [SerializeField]
        [Header("SetBallThrowerPrefab")]
        GameObject m_ballThrowerPrefab;     //!< ボールを投げるのプレハブ

        [SerializeField]
        [Header("SetScreenInputPrefab")]
        GameObject m_screenInputPrefab;     //!< スクリーンの入力情報のプレハブ

        [SerializeField]
        [Header("SetCanvas")]
        Canvas m_canvas;                    //!< キャンバス

        //!< デフォルトのボールの生成座標
        static readonly Vector3 m_kDefaultBallGeneratPos = new Vector3(0.0f, 1.3f, 2.5f);

        [SerializeField]
        [Header("SetballGeneratPosition")]
        Vector3 m_ballGeneratPos = m_kDefaultBallGeneratPos;  //!< ボールの生成座標



        // Start is called before the first frame update
        void Start()
        {
            // 初期化
            Init();
            
            return;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /**
         * @brief 初期化
         */
        void Init()
        {
            // ボールを投げるプレハブを生成
            BallThrower ballthrower = Instantiate(m_ballThrowerPrefab).GetComponent<BallThrower>();
            // スクリーン入力情報のプレハブを生成
            ScreenInput screenInput = Instantiate(m_screenInputPrefab).GetComponent<ScreenInput>();

            // ボールを投げるプレハブを初期化
            ballthrower.Init(this, screenInput,m_canvas.GetComponent<ThrowPowerController>());

            // ボールを生成して、セットする
            ballthrower.SetBall(GenerateBallPrefab());

            return;
        }

        /**
         * @brief ボールを生成する
         * @return ボールのコンポーネント
         */
        public Ball GenerateBallPrefab()
        {
            // ボールのプレハブを生成する
            GameObject ball = Instantiate(m_ballPrefab);
            // ボールの座標を設定する
            ball.transform.position = m_ballGeneratPos;

            // プレハブからコンポーネントを戻す
            return ball.GetComponent<Ball>();
        }
    }
}