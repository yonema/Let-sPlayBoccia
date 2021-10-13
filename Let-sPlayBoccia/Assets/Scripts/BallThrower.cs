using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief �u�{�b�`�����悤���v�̃l�[���X�y�[�X
 */
namespace nsLetsPlayBoccia
{

    /**
     * @brief �{�[���𓊂���N���X
     * @author yoneji
     */
    public class BallThrower : MonoBehaviour
    {
        [SerializeField]
        [Header("SetMinThrowPower")]
        float m_MinThorwPower = 0.5f;  //!< �����鋭���̍ŏ��l

        [SerializeField]
        [Header("SetMaxThrowPower")]
        float m_MaxThorwPower = 5.0f;  //!< �����鋭���̍ŏ��l

        [SerializeField]
        [Header("SetDownTorqueLen")]
        float m_downTorqueLen = 2500.0f;  //!< ����]�̃g���N�̑傫��

        [SerializeField]
        [Header("SetArchingAngle")]
        float m_archingAgle = 45.0f;  //!< �R�Ȃ�̊p�x�̑傫��


        ScreenInput m_screenInput;  //!< �X�N���[���̓��͏��
        Ball m_ball;                //!< �{�[��
        BallManager m_ballManager;  //!< �{�[���}�l�[�W���[
        ThrowPowerController m_throwPowerController;    //!< ������p���[�̃R���g���[���[
        SwitchButton m_switchButton;                    //!< �{�^���؂�ւ�

        bool m_canThrow = false;        //!< �����\���H

        /**
         * @brief �{�[���̃X�e�[�g
         */
        enum EnBallThrowerState
        {
            enBeforeThrow,        //!< ������O
            enWaitIsSleeping,     //!< �Î~��Ԃ�҂�
            enWaitNextTurn,       //!< ���̃^�[����҂�
        }
        EnBallThrowerState m_state = EnBallThrowerState.enBeforeThrow;  //!< �X�e�[�g

        /**
         * @brief �{�[���̉�]
         */
        public enum EnBallRotation
        {
            enUpRotation,   //!< ���]
            enDownRotation  //!< ����]
        }
        EnBallRotation m_ballRotation = EnBallRotation.enUpRotation;    //!< �{�[���̉�]

        /**
         * @brief ������
         */
        public enum EnThrowType
        {
            enFastBall,     //!< ����
            enArchingBall   //!< �R�Ȃ�
        }
        EnThrowType m_thorwType = EnThrowType.enFastBall;   //!< ������

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            // �X�e�[�g�ŏ�����U�蕪��
            switch (m_state)
            {
                // ������O
                case EnBallThrowerState.enBeforeThrow:
                    // ������O�̃A�b�v�f�[�g
                    BeforeThrowUpdate();
                    break;
                // �Î~��Ԃ�҂�
                case EnBallThrowerState.enWaitIsSleeping:
                    // �Î~��ԂɂȂ�܂ő҂�
                    WaitSleeping();
                    break;
                // ���̃^�[����҂�
                case EnBallThrowerState.enWaitNextTurn:
                    SetBall(m_ballManager.GenerateBallPrefab());
                    m_state = EnBallThrowerState.enBeforeThrow;
                    break;
            }


            return;
        }

        /**
         * @brief ������O�̍X�V
         */
        void BeforeThrowUpdate()
        {
            // �{�[�����^�b�`���ꂽ���H
            if (m_ball.IsTouched())
            {
                // �^�b�`���ꂽ�瓊���\�ɂ���
                m_canThrow = true;
                // �p���[�Q�[�W�̑������J�n����B
                m_throwPowerController.StartGauge();
            }

            // ��Ƀt���b�N���͂���������A���A�����\�Ȃ�
            if (m_screenInput.GetNowFlick() == ScreenInput.FlickDirection.UP && m_canThrow)
            {
                Debug.Log("FlickUp");

                // ��������
                Throw();

            }

            // ��ʂɎw���G��Ă��Ȃ���
            if (MyGodTouch.GetPhase() == GodPhase.None)
            {
                // �܂��Q�[�W���������Ă��邩�H
                if (m_throwPowerController.IsGaugeChanging())
                {
                    // �܂��Q�[�W���~�܂��Ă��Ȃ�����
                    // �p���[�Q�[�W�̑������~�߂�
                    m_throwPowerController.EndGauge();
                    // �p���[�Q�[�W�����Z�b�g����
                    m_throwPowerController.ResetGauge();
                }
                // �����s�\�ɂ���
                m_canThrow = false;
            }

            return;
        }

        /**
         * @brief �{�[�����Î~��ԂɂȂ�܂ő҂���
         */
        void WaitSleeping()
        {
            // �{�[�����Î~��Ԃ��H
            if (m_ball.IsSleeping())
            {
                Debug.Log("IsSleeping");
                // �p���[�Q�[�W�����Z�b�g����
                m_throwPowerController.ResetGauge();
                // ���̃^�[����҂X�e�[�g��
                m_state = EnBallThrowerState.enWaitNextTurn;
            }

            return;
        }

        /**
         * @brief ��������
         */
        void Throw()
        {
            // �p���[�Q�[�W�̑������~�߂āA�p���[���𓾂�
            float powerRate = m_throwPowerController.EndGauge();

            // �p���[�����瓊����p���[���v�Z����
            float power = Mathf.Lerp(m_MinThorwPower, m_MaxThorwPower, powerRate);

            Debug.Log("rate:" + powerRate + "," + "power:" + power);

            // �g���N
            Vector3 torque = Vector3.zero;
            // ����]���H
            if (m_ballRotation == EnBallRotation.enDownRotation)
            {
                // ����]�̃g���N���v�Z
                torque = Vector3.Cross(Camera.main.transform.forward, Vector3.up) * m_downTorqueLen;
            }
            Debug.Log("torque" + torque);


            // ���������
            Vector3 throwDir = Camera.main.transform.forward;
            // Y�������Ȃ����Ă���
            throwDir.y = 0.0f;
            // �R�Ȃ肩�H
            if (m_thorwType == EnThrowType.enArchingBall)
            {
                // �R�Ȃ�̕������v�Z
                Vector3 rotAxis = Vector3.Cross(throwDir, Vector3.up);
                throwDir = Quaternion.AngleAxis(m_archingAgle, rotAxis) * throwDir;
            }
            // ���K������
            throwDir.Normalize();

            // �{�[���𓊂���
            m_ball.Throw(power, torque, throwDir);

            // �{�[���̐Î~��Ԃ�҂X�e�[�g��
            m_state = EnBallThrowerState.enWaitIsSleeping;

            return;
        }

        /**
         * @brief ������
         * @param ballManager �{�[���̃}�l�[�W���[
         * @param screenInput �X�N���[���̓��͏��
         * @param canvas UI�̃L�����o�X
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
         * @brief �{�[�����Z�b�g����
         * @param �{�[��
         */
        public void SetBall(Ball ball)
        {
            m_ball = ball;

            return;
        }

        /**
         * @brief ��]���[�h�����݂̂��̂���ύX���鏈��
         */
        public void ChangeRotation()
        {
            // ���]���H
            if (m_ballRotation == EnBallRotation.enUpRotation)
            {
                // ���]�Ȃ�A����]�ɕύX����
                m_ballRotation = EnBallRotation.enDownRotation;

            }
            else
            {
                // ����]�Ȃ�A���]�ɕύX����
                m_ballRotation = EnBallRotation.enUpRotation;
            }
            Debug.Log("ChangeRotation:" + m_ballRotation);


            // ��]�{�^����؂�ւ���
            m_switchButton.RotationMode(m_ballRotation);

            return;
        }

        /**
         * @brief �����������݂̂��̂���ύX����
         */
        public void ChangeThorwType()
        {
            // �������H
            if (m_thorwType == EnThrowType.enFastBall)
            {
                // �����Ȃ�A�R�Ȃ�ɕύX����
                m_thorwType = EnThrowType.enArchingBall;
            }
            else
            {
                // �R�Ȃ�Ȃ�A�����ɕύX����
                m_thorwType = EnThrowType.enFastBall;
            }
            Debug.Log("ChangeThorwType:" + m_thorwType);

            // �������̃{�^����؂�ւ���
            m_switchButton.ThrowType(m_thorwType);

            return;
        }

    }
}