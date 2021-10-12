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
     * @author �Ēn�^��
     */
    public class BallThrower : MonoBehaviour
    {

        //!< �f�t�H���g�̓����鋭���̍ŏ��l
        static readonly float m_kDefaultMinThrowPower = 0.5f;
        [SerializeField]
        [Header("SetMinThrowPower")]
        float m_MinThorwPower = m_kDefaultMinThrowPower;  //!< �����鋭���̍ŏ��l

        //!< �f�t�H���g�̓����鋭���̍ő�l
        static readonly float m_kDefaultMaxThrowPower = 5.0f;
        [SerializeField]
        [Header("SetMaxThrowPower")]
        float m_MaxThorwPower = m_kDefaultMaxThrowPower;  //!< �����鋭���̍ŏ��l

        ScreenInput m_screenInput;  //!< �X�N���[���̓��͏��
        Ball m_ball;                //!< �{�[��
        BallManager m_ballManager;  //!< �{�[���}�l�[�W���[
        ThrowPowerController m_throwPowerController;    //!< ������p���[�̃R���g���[���[

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
                // �p���[�Q�[�W�̑������J�n����
                m_throwPowerController.StartGauge();
            }

            // ��Ƀt���b�N���͂���������A���A�����\�Ȃ�
            if (m_screenInput.GetNowFlick() == ScreenInput.FlickDirection.UP && m_canThrow)
            {
                Debug.Log("FlickUp");

                // �p���[�Q�[�W�̑������~�߂āA�p���[���𓾂�
                float powerRate = m_throwPowerController.EndGauge();

                // �p���[�����瓊����p���[���v�Z����
                float power = Mathf.Lerp(m_MinThorwPower, m_MaxThorwPower, powerRate);

                Debug.Log("rate:" + powerRate + "," + "power:" + power);

                // ������
                m_ball.Throw(power);

                // �{�[���̐Î~��Ԃ�҂X�e�[�g��
                m_state = EnBallThrowerState.enWaitIsSleeping;
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
        }

        /**
         * @brief ������
         * @param ballManager �{�[���̃}�l�[�W���[
         * @param �X�N���[���̓��͏��
         * @param ������p���[�̃R���g���[���[
         */
        public void Init(
            BallManager ballManager,
            ScreenInput screenInput,
            ThrowPowerController throwPowerController
            )
        {
            m_ballManager = ballManager;
            m_screenInput = screenInput;
            m_throwPowerController = throwPowerController;
            return;
        }

        /**
         * @brief �{�[�����Z�b�g����
         * @param �{�[��
         */
        public void SetBall(Ball ball)
        {
            m_ball = ball;
        }

    }
}