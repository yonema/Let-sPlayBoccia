using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief �u�{�b�`�����悤���v�̃l�[���X�y�[�X
 */
namespace nsLetsPlayBoccia
{
    /**
     * @brief �{�[���N���X�B�{�[���̓����̏����������B
     * @author yoneji
     */
    public class Ball : MonoBehaviour
    {


        Rigidbody m_rigibody;   //!< ���W�b�h�{�f�B

        bool m_isTouthed = false;   //!< ��ʂ���G����Ă��邩�H

        float m_killY = -10.0f;   //!< �L��Y�B���W�����̍�����艺�ɂȂ������~


        // Start is called before the first frame update
        // ���N���X��Start�֐��͔h���N���X�̕��ŌĂ΂Ȃ��Ƃ����Ȃ��B
        protected void Start()
        {
            // ���W�b�h�{�f�B�̃R���|�[�l���g���擾
            m_rigibody = GetComponent<Rigidbody>();
            // �ŏ��͏d�͂�K�p���Ȃ�
            m_rigibody.useGravity = false;
            // �ő��]���x�𖳌��ɂ���
            m_rigibody.maxAngularVelocity = Mathf.Infinity;

            return;
        }

        // Update is called once per frame
        void Update()
        {
            // �L��Y�`�F�b�N
            if (transform.position.y < m_killY)
            {
                Debug.Log("KillY");
                // ���W���L��Y�M���M���̂Ƃ���ŌŒ肷��
                Vector3 pos = transform.position;
                pos.y = m_killY;
                transform.position = pos;
                // �����Ȃ��悤�ɂ���
                m_rigibody.isKinematic = true;
            }
        }

        /**
         * @brief �����鏈��
         * @param power ������� 
         * @param torque �g���N
         */
        public void Throw(float power, Vector3 torque)
        {
            // ������u�Ԃɏd�͂�K�p����
            m_rigibody.useGravity = true;

            // �͂�������
            // Impulse:���ʂ��l�����āA�u�ԓI�ɗ͂�^���郂�[�h
            m_rigibody.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);

            m_rigibody.AddTorque(torque,ForceMode.VelocityChange);

            return;
        }

        /**
         * @brief �G����Ă��邩�H�𓾂�
         * @return �G����Ă��邩�H
         */
        public bool IsTouched()
        {
            return m_isTouthed;
        }

        /**
         * @brief �{�[�����Î~��ԂɂȂ������H�𓾂�
         * @return �Î~��Ԃ��H
         */
        public bool IsSleeping()
        {
            return m_rigibody.IsSleeping();
        }

        /**
        * @brief ��ʂ��牟���ꂽ�Ƃ��ɌĂ΂��C�x���g
        * @warning ���̊֐��̖��O��ύX�����ꍇ�APrefab��Ball��EventTrigger�R���|�[�l���g��
        * �p�����[�^���ύX���Ȃ���΂����Ȃ��B
        */
        public void OnPointerDownEvent()
        {
            Debug.Log("PointerDown");
            // ��ʂ���G����Ă���
            m_isTouthed = true;
        }

        /**
        * @brief ��ʂ��牟����Ă�����Ԃ��痣�ꂽ�Ƃ��ɌĂ΂��C�x���g
        * @warning ���̊֐��̖��O��ύX�����ꍇ�APrefab��Ball��EventTrigger�R���|�[�l���g��
        * �p�����[�^���ύX���Ȃ���΂����Ȃ��B
        */
        public void OnPointerExitEvent()
        {
            Debug.Log("PointerExit");
            // ��ʂ���G����Ă��Ȃ�
            m_isTouthed = false;
        }


    }
}