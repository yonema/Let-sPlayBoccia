using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 「ボッチャしようぜ」のネームスペース
 */
namespace nsLetsPlayBoccia
{
    /**
     * @brief ボールのジェネレータークラス
     * @author 米地真央
     */
    public class GameBallGenerator : MonoBehaviour
    {

        [SerializeField]
        [Header("SetBallPrefab")]
        GameObject m_ballPrefab;    //!< ボールのプレハブ

        [SerializeField]
        [Header("SetScreenInput")]
        ScreenInput m_screenInput;  //!< スクリーンの入力情報


        // Start is called before the first frame update
        void Start()
        {
            GenerateBallPrefab();

            return;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /**
         * @brief ボールを生成する
         */
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
}