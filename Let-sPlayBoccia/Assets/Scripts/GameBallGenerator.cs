using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �{�[���̃W�F�l���[�^�[�B
public class GameBallGenerator : MonoBehaviour
{
    // �{�[���v���n�u
    [SerializeField]
    [Header("SetBallPrefab")]
    GameObject m_ballPrefab;

    // �X�N���[���̓��͏��
    [SerializeField]
    ScreenInput m_screenInput;

    // Start is called before the first frame update
    void Start()
    {
        GenerateBallPrefab();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �{�[���𐶐�����
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
