using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief �u�{�b�`�����悤���v�̃l�[���X�y�[�X
 */
namespace nsLetsPlayBoccia
{
    /**
     * @brief �{�[���̊��N���X�B�{�[���̓����̏����������B
     * @author �Ēn�^��
     */
    public class BallBase : MonoBehaviour
    {


        private Rigidbody m_rigibody;   //!< ���W�b�h�{�f�B

        // Start is called before the first frame update
        // ���N���X��Start�֐��͔h���N���X�̕��ŌĂ΂Ȃ��Ƃ����Ȃ��B
        protected void Start()
        {
            //Debug.Log("BaseStart");
            // ���W�b�h�{�f�B�̃R���|�[�l���g���擾
            m_rigibody = GetComponent<Rigidbody>();
            // �ŏ��͏d�͂�K�p���Ȃ�
            m_rigibody.useGravity = false;

            return;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /**
         * @brief �����鏈��
         * @param[in] power ������� 
         */
        protected void Throw(float power)
        {
            // ������u�Ԃɏd�͂�K�p����
            m_rigibody.useGravity = true;

            // �͂�������
            // Impulse:���ʂ��l�����āA�u�ԓI�ɗ͂�^���郂�[�h
            m_rigibody.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);

            return;
        }
    }
}