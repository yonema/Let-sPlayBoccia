using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief �u�{�b�`�����悤���v�̃l�[���X�y�[�X
 */
namespace nsLetsPlayBoccia
{

    /**
     * @brief �J�����̈ړ�����
     * @author yoneji
     */
    public class CameraMove : MonoBehaviour
    {
        float m_angle = 0.0f;       //!< ���݂̃J�����̊p�x
        float m_touchAngle = 0.0f;  //!< ��ʂɃ^�b�`�������̃J�����̊p�x
        bool m_turnFlag = false;    //!< �J��������]���邩�H�̃t���O
        Vector3 m_fromBallVec = Vector3.zero;   //!< �{�[������J�����i���g�j�ւ̃x�N�g��
        Vector3 m_ballGeneratPosition = Vector3.zero;   //!< �{�[���������������W
        Quaternion m_startQRot = Quaternion.identity;   //!< �J�n���̃J�����̉�]
        float m_touchXPosition = 0.0f;  //!< ��ʂɃ^�b�`���̐G�ꂽ���W

        [SerializeField]
        [Header("SetMaxAngle")]
        float m_maxAngle = 45.0f;   //!< �J�����̍ő�p�x

        [SerializeField]
        [Header("SetMaxWipeVecLen")]
        float m_maxWipeVecLen = 1000.0f;   //!< �X���C�v�̍ő�̑傫��

        // Start is called before the first frame update
        void Start()
        {
            // �J�n���̃J�����̉�]��ێ�
            m_startQRot = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {

            // ��ʂɎw���G��Ă��Ȃ���
            if (MyGodTouch.GetPhase() == GodPhase.None)
            {
                // �G��Ă��Ȃ����߁A��]�ł��Ȃ�
                m_turnFlag = false;
            }

            // ��]���ł��Ȃ����H
            if (m_turnFlag != true)
            {
                // �ł��Ȃ����ߑ������^�[��
                return;
            }

            // �J�����̉�]����
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
         * @brief �J�����̉�]����
         */
        void TurnCamera()
        {
            // ���̍��W
            float nowXPos = MyGodTouch.GetPosition().x;
            // �X���C�v�̃x�N�g��
            float SwipeVec = nowXPos - m_touchXPosition;

            //Debug.Log(Mathf.Abs(SwipeVec));
            // �X���C�v���A�ǂ̂��炢�X���C�v���Ă��邩�B
            float rate = Mathf.Abs(SwipeVec) / m_maxWipeVecLen;
            // �X���C�v���̕������A��]������
            m_angle = Mathf.Lerp(0.0f, m_maxAngle, Mathf.Min(rate, 1.0f));

            // �X���C�v�x�N�g�������̒l�A�܂�A���ւ̃X���C�v���H
            if (SwipeVec < 0.0f)
            {
                // �p�x�𔽓]������
                m_angle = -m_angle;
            }

            // ��ʂ��^�b�`�������̊p�x�����Z����
            m_angle += m_touchAngle;

            // �p�x���ő�p�x���I�[�o�[���Ă��邩�H
            if (m_angle > m_maxAngle)
            {
                // �ő�p�x�ɐݒ肷��
                m_angle = m_maxAngle;
            }
            // �p�x��-�ő�p�x���I�[�o�[���Ă��邩�H
            else if (m_angle < -m_maxAngle)
            {
                // -�ő�p�x��ݒ肷��
                m_angle = -m_maxAngle;
            }

            // �{�[������̃x�N�g������]�������]�N�H�[�^�j�I��
            Quaternion VecQRot = Quaternion.AngleAxis(m_angle, Vector3.up);
            // �{�[������̃x�N�g������]������
            Vector3 fromBallVec = VecQRot * m_fromBallVec;
            // ���g�i�J�����j�̈ʒu��ݒ肷��
            transform.position = m_ballGeneratPosition + fromBallVec;

            // �J�����̉�]�N�H�[�^�j�I��
            Quaternion qRot = Quaternion.AngleAxis(m_angle, Vector3.up) * m_startQRot;
            // ���g�i�J�����j�̉�]��ݒ肷��
            transform.rotation = qRot;
        }

        /**
         * @brief �J�����ړ��G���A�������ꂽ�Ƃ��̏���
         * @warning ���̊֐��̖��O��ύX�����ꍇ�ACanvas��CameraMoveInputArea��EventTrigger�R���|�[�l���g��
         * �p�����[�^���ύX���Ȃ���΂����Ȃ��B
         */
        public void OnPointerDownEvent()
        {
            Debug.Log("CameraMoveOnPointerDown");
            // ��]�\�ɂ���
            m_turnFlag = true;
            // ��ʂɃ^�b�`��������X���W��ݒ�
            m_touchXPosition = MyGodTouch.GetPosition().x;
            // ��ʂɃ^�b�`�������̉�]��ݒ�
            m_touchAngle = m_angle;

            return;
        }

    }
}