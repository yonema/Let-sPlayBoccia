using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;


/**
 * @brief �u�{�b�`�����悤���v�̃l�[���X�y�[�X
 */
namespace nsLetsPlayBoccia
{
    /**
     * @brief �{�[���}�l�[�W���[�N���X
     * @author �Ēn�^��
     */
    public class BallManager : MonoBehaviour
    {
        [SerializeField]
        [Header("SetBallPrefab")]
        GameObject m_ballPrefab;            //!< �{�[���̃v���n�u

        [SerializeField]
        [Header("SetBallThrowerPrefab")]
        GameObject m_ballThrowerPrefab;     //!< �{�[���𓊂���̃v���n�u

        [SerializeField]
        [Header("SetScreenInputPrefab")]
        GameObject m_screenInputPrefab;     //!< �X�N���[���̓��͏��̃v���n�u

        [SerializeField]
        [Header("SetCanvas")]
        Canvas m_canvas;                    //!< �L�����o�X

        //!< �f�t�H���g�̃{�[���̐������W
        static readonly Vector3 m_kDefaultBallGeneratPos = new Vector3(0.0f, 1.3f, 2.5f);

        [SerializeField]
        [Header("SetballGeneratPosition")]
        Vector3 m_ballGeneratPos = m_kDefaultBallGeneratPos;  //!< �{�[���̐������W



        // Start is called before the first frame update
        void Start()
        {
            // ������
            Init();
            
            return;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /**
         * @brief ������
         */
        void Init()
        {
            // �{�[���𓊂���v���n�u�𐶐�
            BallThrower ballthrower = Instantiate(m_ballThrowerPrefab).GetComponent<BallThrower>();
            // �X�N���[�����͏��̃v���n�u�𐶐�
            ScreenInput screenInput = Instantiate(m_screenInputPrefab).GetComponent<ScreenInput>();

            // �{�[���𓊂���v���n�u��������
            ballthrower.Init(this, screenInput,m_canvas.GetComponent<ThrowPowerController>());

            // �{�[���𐶐����āA�Z�b�g����
            ballthrower.SetBall(GenerateBallPrefab());

            return;
        }

        /**
         * @brief �{�[���𐶐�����
         * @return �{�[���̃R���|�[�l���g
         */
        public Ball GenerateBallPrefab()
        {
            // �{�[���̃v���n�u�𐶐�����
            GameObject ball = Instantiate(m_ballPrefab);
            // �{�[���̍��W��ݒ肷��
            ball.transform.position = m_ballGeneratPos;

            // �v���n�u����R���|�[�l���g��߂�
            return ball.GetComponent<Ball>();
        }
    }
}