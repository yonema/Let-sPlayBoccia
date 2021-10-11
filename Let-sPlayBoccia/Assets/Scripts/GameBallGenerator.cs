using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief �u�{�b�`�����悤���v�̃l�[���X�y�[�X
 */
namespace nsLetsPlayBoccia
{
    /**
     * @brief �{�[���̃W�F�l���[�^�[�N���X
     * @author �Ēn�^��
     */
    public class GameBallGenerator : MonoBehaviour
    {

        [SerializeField]
        [Header("SetBallPrefab")]
        GameObject m_ballPrefab;    //!< �{�[���̃v���n�u

        [SerializeField]
        [Header("SetScreenInput")]
        ScreenInput m_screenInput;  //!< �X�N���[���̓��͏��


        // Start is called before the first frame update
        void Start()
        {
            GenerateBallPrefab();

            return;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /**
         * @brief �{�[���𐶐�����
         */
        public void GenerateBallPrefab()
        {
            // �{�[���̃v���n�u�𐶐�����
            GameObject ball = Instantiate(m_ballPrefab);
            // �{�[���̍��W��ݒ肷��
            ball.transform.position = this.transform.position;
            // �{�[���ɃX�N���[���̓��͏����Z�b�g����
            ball.GetComponent<GameBall>().SetScreenInput(m_screenInput);

            return;
        }
    }
}