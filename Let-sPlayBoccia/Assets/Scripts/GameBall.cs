using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Q�[�����g�p����{�[���N���X
public class GameBall : BallBase
{
    // �X�N���[���̓��͏��
    ScreenInput m_screenInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_screenInput.GetNowFlick() == ScreenInput.FlickDirection.UP)
        {
            Debug.Log("Up");
        }
    }

    private void FixedUpdate()
    {
        
    }

    // �X�N���[���̓��͏����Z�b�g
    public void SetScreenInput(ScreenInput screenInput)
    {
        m_screenInput = screenInput.GetComponent<ScreenInput>();
    }
}
