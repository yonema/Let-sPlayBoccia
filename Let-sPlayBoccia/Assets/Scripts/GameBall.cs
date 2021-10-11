using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief �u�{�b�`�����悤���v�̃l�[���X�y�[�X
 */
namespace nsLetsPlayBoccia
{

    /**
     * @brief �Q�[�����g�p����{�[���N���X
     * @author �Ēn�^��
     */
    public class GameBall : BallBase
    {

        ScreenInput m_screenInput;  //!< �X�N���[���̓��͏��

        float m_throwPower = 0.0f;      //!< ������p���[
        bool m_canThrow = false;        //!< �����\���H

        // Start is called before the first frame update
        void Start()
        {
            // ���N���X��Start�֐��͔h���N���X�̕��ŌĂ΂Ȃ��Ƃ����Ȃ��B
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {

            // ��Ƀt���b�N���͂���������A���A�����\�Ȃ�
            if (m_screenInput.GetNowFlick() == ScreenInput.FlickDirection.UP && m_canThrow)
            {
                Debug.Log("FlickUp");

                m_throwPower = 1.0f;

                // ������
                Throw(m_throwPower);
            }

            // ��ʂɎw���G��Ă��Ȃ���
            if (MyGodTouch.GetPhase() == GodPhase.None)
            {
                // �����s�\�ɂ���
                m_canThrow = false;
            }

            return;
        }

        /**
         * @brief �X�N���[���̓��͏����Z�b�g
         */
        public void SetScreenInput(ScreenInput screenInput)
        {
            m_screenInput = screenInput.GetComponent<ScreenInput>();

            return;
        }

        /**
         * @brief ��ʂ��牟���ꂽ�Ƃ��ɌĂ΂��C�x���g
         * @warning ���̊֐��̖��O��ύX�����ꍇ�APrefab��GameBall��EventTrigger�R���|�[�l���g��
         * �p�����[�^���ύX���Ȃ���΂����Ȃ��B
         */
        public void OnPointerDownEvent()
        {
            Debug.Log("PointerDown");
            // �����\�ɂ���
            m_canThrow = true;
        }
    }
}