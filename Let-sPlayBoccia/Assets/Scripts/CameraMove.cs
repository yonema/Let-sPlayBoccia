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

        ScreenInput m_screenInput;
        float m_angle = 0.0f;
        float m_touchAngle = 0.0f;
        bool m_turnFlag = false;
        Vector3 m_fromBallVec = Vector3.zero;
        Vector3 m_ballGeneratPosition = Vector3.zero;

        Quaternion m_startQRot = Quaternion.identity;
        Vector3 m_touchPosition = Vector3.zero;

        [SerializeField]
        [Header("SetTurnSpeed")]
        float m_turnSpeed = 2.0f;

        [SerializeField]
        [Header("SetMaxAngle")]
        float m_maxAngle = 45.0f;

        // Start is called before the first frame update
        void Start()
        {
            m_startQRot = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {

            // 画面に指が触れていない時
            if (MyGodTouch.GetPhase() == GodPhase.None)
            {
                m_turnFlag = false;
            }

            if (m_turnFlag != true)
            {
                return;
            }
            Vector3 nowPos = MyGodTouch.GetPosition();
            Vector3 SwipeVec = nowPos - m_touchPosition;
            ScreenInput.SwipeDirection swipeDir = m_screenInput.GetNowSwipe();
            float rate = SwipeVec.magnitude / 1000.0f;
            m_angle = Mathf.Lerp(0.0f, m_maxAngle, Mathf.Min(rate, 1.0f));
            if (SwipeVec.x < 0.0f)
            {
                m_angle = -m_angle;
            }
            m_angle += m_touchAngle;
            if (m_angle > m_maxAngle)
            {
                m_angle = m_maxAngle;
            }
            else if (m_angle < -m_maxAngle)
            {
                m_angle = -m_maxAngle;
            }
            Quaternion VecQRot = Quaternion.AngleAxis(m_angle, Vector3.up)/* * m_startQRot*/;
            Vector3 fromBallVec = VecQRot * m_fromBallVec;
            transform.position = m_ballGeneratPosition + fromBallVec;
            Quaternion qRot = Quaternion.AngleAxis(m_angle, Vector3.up) * m_startQRot;
            transform.rotation = qRot;


        }

        public void Init(ScreenInput screenInput, Vector3 ballPos)
        {
            m_screenInput = screenInput;
            m_ballGeneratPosition = ballPos;
            m_fromBallVec = transform.position - m_ballGeneratPosition;
            Debug.Log(m_fromBallVec);
        }

        public void OnDragEvent()
        {
            ScreenInput.SwipeDirection swipeDir = m_screenInput.GetNowSwipe();
            switch (swipeDir)
            {
                case ScreenInput.SwipeDirection.LEFT:
                    TurnLeft();
                    break;
                case ScreenInput.SwipeDirection.RIGHT:
                    TurnRight();
                    break;
            }
        }

        public void OnPointerDownEvent()
        {
            Debug.Log("CameraMoveOnPointerDown");
            m_turnFlag = true;
            m_touchPosition = MyGodTouch.GetPosition();
            m_touchAngle = m_angle;
        }


        void TurnLeft()
        {
            m_angle -= m_turnSpeed;
            if (m_angle < -m_maxAngle)
            {
                m_angle = -m_maxAngle;
            }

            transform.rotation = Quaternion.AngleAxis(m_angle, Vector3.up) * m_startQRot;


            return;
        }


        void TurnRight()
        {
            m_angle += m_turnSpeed;
            if (m_angle > m_maxAngle)
            {
                m_angle = m_maxAngle;
            }
            transform.rotation = Quaternion.AngleAxis(m_angle, Vector3.up) * m_startQRot;
            return;
        }
    }
}