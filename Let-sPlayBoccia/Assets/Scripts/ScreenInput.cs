using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenInput : MonoBehaviour
{
    // �t���b�N�ŏ��ړ�����
    [SerializeField]
    private Vector2 FlickMinRange = new Vector2(30.0f, 30.0f);
    // �X���C�v�ŏ��ړ�����
    [SerializeField]
    private Vector2 SwipeMinRange = new Vector2(50.0f, 50.0f);
    // TAP��NONE�ɖ߂��܂ł̃J�E���g
    [SerializeField]
    private int NoneCountMax = 2;
    private int NoneCountNow = 0;
    // �X���C�v���͋���
    private Vector2 SwipeRange;
    // ���͕����L�^�p
    private Vector2 InputSTART;
    private Vector2 InputMOVE;
    private Vector2 InputEND;
    // �t���b�N�̕���
    public enum FlickDirection
    {
        NONE,
        TAP,
        UP,
        RIGHT,
        DOWN,
        LEFT,
    }
    private FlickDirection NowFlick = FlickDirection.NONE;
    // �X���C�v�̕���
    public enum SwipeDirection
    {
        NONE,
        TAP,
        UP,
        RIGHT,
        DOWN,
        LEFT,
    }
    private SwipeDirection NowSwipe = SwipeDirection.NONE;


    // Update is called once per frame
    void Update()
    {
        GetInputVector();
    }

    // ���͂̎擾
    private void GetInputVector()
    {
        // Unity��ł̑���擾
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                InputSTART = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                InputMOVE = Input.mousePosition;
                SwipeCLC();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                InputEND = Input.mousePosition;
                FlickCLC();
            }
            else if (NowFlick != FlickDirection.NONE || NowSwipe != SwipeDirection.NONE)
            {
                ResetParameter();
            }
        }
        // �[����ł̑���擾
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];
                if (touch.phase == TouchPhase.Began)
                {
                    InputSTART = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    InputMOVE = Input.mousePosition;
                    SwipeCLC();
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    InputEND = touch.position;
                    FlickCLC();
                }
            }
            else if (NowFlick != FlickDirection.NONE || NowSwipe != SwipeDirection.NONE)
            {
                ResetParameter();
            }
        }
    }

    // ���͓��e����t���b�N�������v�Z
    private void FlickCLC()
    {
        Vector2 _work = new Vector2((new Vector3(InputEND.x, 0, 0) - new Vector3(InputSTART.x, 0, 0)).magnitude, (new Vector3(0, InputEND.y, 0) - new Vector3(0, InputSTART.y, 0)).magnitude);

        if (_work.x <= FlickMinRange.x && _work.y <= FlickMinRange.y)
        {
            NowFlick = FlickDirection.TAP;
        }
        else if (_work.x > _work.y)
        {
            float _x = Mathf.Sign(InputEND.x - InputSTART.x);
            if (_x > 0) NowFlick = FlickDirection.RIGHT;
            else if (_x < 0) NowFlick = FlickDirection.LEFT;
        }
        else
        {
            float _y = Mathf.Sign(InputEND.y - InputSTART.y);
            if (_y > 0) NowFlick = FlickDirection.UP;
            else if (_y < 0) NowFlick = FlickDirection.DOWN;
        }
    }

    // ���͓��e����X���C�v�������v�Z
    private void SwipeCLC()
    {
        SwipeRange = new Vector2((new Vector3(InputMOVE.x, 0, 0) - new Vector3(InputSTART.x, 0, 0)).magnitude, (new Vector3(0, InputMOVE.y, 0) - new Vector3(0, InputSTART.y, 0)).magnitude);

        if (SwipeRange.x <= SwipeMinRange.x && SwipeRange.y <= SwipeMinRange.y)
        {
            NowSwipe = SwipeDirection.TAP;
        }
        else if (SwipeRange.x > SwipeRange.y)
        {
            float _x = Mathf.Sign(InputMOVE.x - InputSTART.x);
            if (_x > 0) NowSwipe = SwipeDirection.RIGHT;
            else if (_x < 0) NowSwipe = SwipeDirection.LEFT;
        }
        else
        {
            float _y = Mathf.Sign(InputMOVE.y - InputSTART.y);
            if (_y > 0) NowSwipe = SwipeDirection.UP;
            else if (_y < 0) NowSwipe = SwipeDirection.DOWN;
        }
    }

    // NONE�Ƀ��Z�b�g
    private void ResetParameter()
    {
        NoneCountNow++;
        if (NoneCountNow >= NoneCountMax)
        {
            NoneCountNow = 0;
            NowFlick = FlickDirection.NONE;
            NowSwipe = SwipeDirection.NONE;
            SwipeRange = new Vector2(0, 0);
        }
    }

    // �t���b�N�����̎擾
    public FlickDirection GetNowFlick()
    {
        return NowFlick;
    }

    // �X���C�v�����̎擾
    public SwipeDirection GetNowSwipe()
    {
        return NowSwipe;
    }

    // �X���C�v�ʂ̎擾
    public float GetSwipeRange()
    {
        if (SwipeRange.x > SwipeRange.y)
        {
            return SwipeRange.x;
        }
        else
        {
            return SwipeRange.y;
        }
    }

    // �X���C�v�ʂ̎擾
    public Vector2 GetSwipeRangeVec()
    {
        if (NowSwipe != SwipeDirection.NONE)
        {
            return new Vector2(InputMOVE.x - InputSTART.x, InputMOVE.y - InputSTART.y);
        }
        else
        {
            return new Vector2(0, 0);
        }
    }
}
