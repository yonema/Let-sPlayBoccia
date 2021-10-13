using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 「ボッチャしようぜ」のネームスペース
 */
namespace nsLetsPlayBoccia
{

    /**
     * @brief カメラの移動処理
     * @author yoneji
     */
    public class CameraMove : MonoBehaviour
    {
        float m_angle = 0.0f;       //!< 現在のカメラの角度
        float m_touchAngle = 0.0f;  //!< 画面にタッチした時のカメラの角度
        bool m_turnFlag = false;    //!< カメラが回転するか？のフラグ
        Vector3 m_fromBallVec = Vector3.zero;   //!< ボールからカメラ（自身）へのベクトル
        Vector3 m_ballGeneratPosition = Vector3.zero;   //!< ボールが生成される座標
        Quaternion m_startQRot = Quaternion.identity;   //!< 開始時のカメラの回転
        float m_touchXPosition = 0.0f;  //!< 画面にタッチ時の触れた座標

        [SerializeField]
        [Header("SetMaxAngle")]
        float m_maxAngle = 45.0f;   //!< カメラの最大角度

        [SerializeField]
        [Header("SetMaxWipeVecLen")]
        float m_maxWipeVecLen = 1000.0f;   //!< スワイプの最大の大きさ

        // Start is called before the first frame update
        void Start()
        {
            // 開始時のカメラの回転を保持
            m_startQRot = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {

            // 画面に指が触れていない時
            if (MyGodTouch.GetPhase() == GodPhase.None)
            {
                // 触れていないため、回転できない
                m_turnFlag = false;
            }

            // 回転ができないか？
            if (m_turnFlag != true)
            {
                // できないため早期リターン
                return;
            }

            // カメラの回転処理
            TurnCamera();

            return;
        }

        public void Init(Vector3 ballPos)
        {
            m_ballGeneratPosition = ballPos;
            m_fromBallVec = transform.position - m_ballGeneratPosition;
            Debug.Log(m_fromBallVec);
        }

        /**
         * @brief カメラの回転処理
         */
        void TurnCamera()
        {
            // 今の座標
            float nowXPos = MyGodTouch.GetPosition().x;
            // スワイプのベクトル
            float SwipeVec = nowXPos - m_touchXPosition;

            //Debug.Log(Mathf.Abs(SwipeVec));
            // スワイプ率、どのくらいスワイプしているか。
            float rate = Mathf.Abs(SwipeVec) / m_maxWipeVecLen;
            // スワイプ率の分だけ、回転させる
            m_angle = Mathf.Lerp(0.0f, m_maxAngle, Mathf.Min(rate, 1.0f));

            // スワイプベクトルが負の値、つまり、左へのスワイプか？
            if (SwipeVec < 0.0f)
            {
                // 角度を反転させる
                m_angle = -m_angle;
            }

            // 画面をタッチした時の角度を加算する
            m_angle += m_touchAngle;

            // 角度が最大角度をオーバーしているか？
            if (m_angle > m_maxAngle)
            {
                // 最大角度に設定する
                m_angle = m_maxAngle;
            }
            // 角度が-最大角度をオーバーしているか？
            else if (m_angle < -m_maxAngle)
            {
                // -最大角度を設定する
                m_angle = -m_maxAngle;
            }

            // ボールからのベクトルを回転させる回転クォータニオン
            Quaternion VecQRot = Quaternion.AngleAxis(m_angle, Vector3.up);
            // ボールからのベクトルを回転させる
            Vector3 fromBallVec = VecQRot * m_fromBallVec;
            // 自身（カメラ）の位置を設定する
            transform.position = m_ballGeneratPosition + fromBallVec;

            // カメラの回転クォータニオン
            Quaternion qRot = Quaternion.AngleAxis(m_angle, Vector3.up) * m_startQRot;
            // 自身（カメラ）の回転を設定する
            transform.rotation = qRot;
        }

        /**
         * @brief カメラ移動エリアが押されたときの処理
         * @warning この関数の名前を変更した場合、CanvasのCameraMoveInputAreaのEventTriggerコンポーネントの
         * パラメータも変更しなければいけない。
         */
        public void OnPointerDownEvent()
        {
            Debug.Log("CameraMoveOnPointerDown");
            // 回転可能にする
            m_turnFlag = true;
            // 画面にタッチした時のX座標を設定
            m_touchXPosition = MyGodTouch.GetPosition().x;
            // 画面にタッチした時の回転を設定
            m_touchAngle = m_angle;

            return;
        }

    }
}