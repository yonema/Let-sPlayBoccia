using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * @brief �u�{�b�`�����悤���v�̃l�[���X�y�[�X
 */
namespace nsLetsPlayBoccia
{
    /**
     * @brief ��]�{�^���̐؂�ւ�����
     * @author yoneji
     */
    public class SwitchRotationButton : MonoBehaviour
    {
        [SerializeField]
        [Header("SetUpRotationImage")]
        Image m_UpRotationImage;           //!< ���]�C���[�W

        [SerializeField]
        [Header("SetDownRotationImage")]
        Image m_DownRotationImage;         //!< ����]�C���[�W

        BallThrower m_ballThrower;


        // Start is called before the first frame update
        void Start()
        {

            // �ŏ��͏��]���[�h
            UpRotationMode();
        }
        

        // Update is called once per frame
        void Update()
        {

        }

        public void SetBallThrower(BallThrower ballThrower)
        {
            m_ballThrower = ballThrower;
        }

        /**
         * @brief ��]�{�^���ɉ�]���[�h��ݒ肷��
         * @param ballRotation �{�[���̉�]
         */
        public void RotationMode(BallThrower.EnBallRotation ballRotation)
        {
            // ���]���H
            if (ballRotation == BallThrower.EnBallRotation.enUpRotation)
            {
                // ���]���[�h�ɕύX
                UpRotationMode();
            }
            else
            {
                // ����]���[�h�ɕύX
                DownRotationMode();
            }

            return;
        }

        /**
         * @brief ���]���[�h�ɕύX���鏈��
         */
        void UpRotationMode()
        {
            Debug.Log("UpRotationMode");
            // ���]�C���[�W����������
            ImageVisible(m_UpRotationImage);
            // ����]�C���[�W��s��������
            ImageHidden(m_DownRotationImage);

            return;
        }

        /**
         * @brief ����]���[�h�ɕύX���鏈��
         */
        void DownRotationMode()
        {
            Debug.Log("DownRotationMode");
            // ����]�C���[�W����������
            ImageVisible(m_DownRotationImage);
            // ���]�C���[�W��s��������
            ImageHidden(m_UpRotationImage);
            return;
        }

        /**
         * @biref �C���[�W����������
         */
        void ImageVisible(Image image)
        {
            // �C���[�W�̕s�����ɂ���
            Color color = image.color;
            color.a = 1.0f;
            image.color = color;

            return;
        }

        /**
         * @brief �C���[�W��s��������
         */
        void ImageHidden(Image image)
        {
            // �C���[�W�𓧖��ɂ���
            Color color = image.color;
            color.a = 0.0f;
            image.color = color;

            return;
        }

        public void OnClickRotationButton()
        {
            m_ballThrower.ChangeRotation();
        }
    }
}