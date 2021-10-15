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
     * @brief �{�^���̐؂�ւ�����
     * @author yoneji
     */
    public class SwitchButton : MonoBehaviour
    {
        [SerializeField]
        [Header("SetUpRotationImage")]
        Image m_upRotationImage;            //!< ���]�C���[�W

        [SerializeField]
        [Header("SetDownRotationImage")]
        Image m_downRotationImage;          //!< ����]�C���[�W

        [SerializeField]
        [Header("SetFastBallImage")]
        Image m_fastBallImage;              //!< �����C���[�W

        [SerializeField]
        [Header("SetArtchingBallImage")]
        Image m_artchingBallImage;          //!< �R�Ȃ�C���[�W

        BallThrower m_ballThrower;          //!< �{�[���𓊂���N���X


        // Start is called before the first frame update
        void Start()
        {
            // �ŏ��͏��]���[�h
            UpRotationMode();
            // �ŏ��͒���
            FastBallType();
        }
        

        // Update is called once per frame
        void Update()
        {

        }

        /**
         * @brief �{�[���𓊂���N���X��ݒ肷��
         * @param ballThrower �{�[���𓊂���N���X
         */
        public void SetBallThrower(BallThrower ballThrower)
        {
            m_ballThrower = ballThrower;
            return;
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
         * @brief �������{�^���ɓ�������ݒ肷��
         * @param throwType ������
         */
        public void ThrowType(BallThrower.EnThrowType throwType)
        {
            // �������H
            if (throwType == BallThrower.EnThrowType.enFastBall)
            {
                // �����ɕύX
                FastBallType();
            }
            else
            {
                // �R�Ȃ�ɕύX
                ArchingBallType();
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
            ImageVisible(m_upRotationImage);
            // ����]�C���[�W��s��������
            ImageHidden(m_downRotationImage);

            return;
        }

        /**
         * @brief ����]���[�h�ɕύX���鏈��
         */
        void DownRotationMode()
        {
            Debug.Log("DownRotationMode");
            // ����]�C���[�W����������
            ImageVisible(m_downRotationImage);
            // ���]�C���[�W��s��������
            ImageHidden(m_upRotationImage);
            return;
        }

        /**
         * @brief �����ɕύX
         */
        void FastBallType()
        {
            Debug.Log("FastBallType");
            // �����C���[�W����������
            ImageVisible(m_fastBallImage);
            // �R�Ȃ�C���[�W��s��������
            ImageHidden(m_artchingBallImage);
            return;
        }

        /**
         * @brief �R�Ȃ�ɕύX
         */
        void ArchingBallType()
        {
            Debug.Log("ArchingBallType");
            // �R�Ȃ�C���[�W����������
            ImageVisible(m_artchingBallImage);
            // �����C���[�W��s��������
            ImageHidden(m_fastBallImage);
            return;
        }

        /**
         * @biref �C���[�W����������
         */
        void ImageVisible(Image image)
        {
            // �C���[�W�̕s�����ɂ���
            Color color = image.color;
            color.a = 0.5f;
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

        /**
         * @brief ��]�{�^�����N���b�N���ꂽ�Ƃ��ɌĂ΂�鏈��
         * @warning ���̊֐��̖��O��ύX�����ꍇ�ACanvas��UpRotation��EventTrigger�R���|�[�l���g��
         * �p�����[�^��ύX���Ȃ���΂����Ȃ��B
         */
        public void OnClickRotationButton()
        {
            // ��]���[�h�����݂̂��̂���ύX���鏈��
            m_ballThrower.ChangeRotation();
            return;
        }

        /**
         * @brief �������{�^�����N���b�N���ꂽ�Ƃ��ɌĂ΂�鏈��
         * @warning ���̊֐��̖��O��ύX�����ꍇ�ACanvas��FastBall��EventTrigger�R���|�[�l���g��
         * �p�����[�^��ύX���Ȃ���΂����Ȃ��B
         */
        public void OnClickThrowTypeButton()
        {
            // �����������݂̂��̂���ύX����
            m_ballThrower.ChangeThorwType();
            return;
        }
    }
}