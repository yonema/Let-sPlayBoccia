using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 「ボッチャしようぜ」のネームスペース
 */
namespace nsLetsPlayBoccia
{

    /**
     * @brief スクリーンの入力情報クラス
     * @author 米地真央
     * @details こちらのサイトから引用してきたコードをちょっと改造
     * https://deve-cat.com/unity-flick-swipe/
     * Unityのエディタ上と端末上でタッチの処理を共通化するためにMyGodTouchクラスを使用
     * @see MyGodTouch
     */
    public class ScreenInput : MonoBehaviour
    {
        // フリック最小移動距離
        [SerializeField]
        private Vector2 FlickMinRange = new Vector2(30.0f, 30.0f);
        // スワイプ最小移動距離
        [SerializeField]
        private Vector2 SwipeMinRange = new Vector2(50.0f, 50.0f);
        // TAPをNONEに戻すまでのカウント
        [SerializeField]
        private int NoneCountMax = 1;
        private int NoneCountNow = 0;
        // スワイプ入力距離
        private Vector2 SwipeRange;
        // 入力方向記録用
        private Vector2 InputSTART;
        private Vector2 InputMOVE;
        private Vector2 InputEND;
        // フリックの方向
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
        // スワイプの方向
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

        private void Start()
        {
            // FPS固定
            Application.targetFrameRate = 30;

#if UNITY_EDITOR    // Unityエディター上での操作
            Debug.Log("Unityエディター上での操作");
#else   // 端末上での操作
            Debug.Log("端末上での操作");
#endif
        }


        // Update is called once per frame
        void Update()
        {
            GetInputVector();
        }

        // 入力の取得
        private void GetInputVector()
        {
            // タッチ情報を取得して、処理を振り分ける
            switch (MyGodTouch.GetPhase())
            {
                // 画面に指が触れた時
                case GodPhase.Began:
                    // 画面に指が触れた時の座標を保持
                    InputSTART = MyGodTouch.GetPosition();
                    break;

                // 画面上で指が動いている時
                case GodPhase.Moved:
                    // 画面上で指が動いている時の座標を保持
                    InputMOVE = MyGodTouch.GetPosition();//Input.mousePosition;
                    // 入力内容からワイプの方向を計算
                    SwipeCLC();
                    break;
                
                // 画面から指が離れた時
                case GodPhase.Ended:
                    // 画面から指が離れた時の座標を保持
                    InputEND = MyGodTouch.GetPosition();
                    // 入力内容からフリック方向を計算
                    FlickCLC();
                    break;
               
                // 画面に指が触れていない時
                case GodPhase.None:
                    // NONEにリセット
                    ResetParameter();
                    break;
            }

            return;
        }

        // 入力内容からフリック方向を計算
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

        // 入力内容からスワイプ方向を計算
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

        // NONEにリセット
        private void ResetParameter()
        {
            //Debug.Log("リセット呼び出し");
            NoneCountNow++;
            if (NoneCountNow >= NoneCountMax)
            {
                //Debug.Log("リセット実行");

                NoneCountNow = 0;
                NowFlick = FlickDirection.NONE;
                NowSwipe = SwipeDirection.NONE;
                SwipeRange = new Vector2(0, 0);
            }
        }

        // フリック方向の取得
        public FlickDirection GetNowFlick()
        {
            return NowFlick;
        }

        // スワイプ方向の取得
        public SwipeDirection GetNowSwipe()
        {
            return NowSwipe;
        }

        // スワイプ量の取得
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

        // スワイプ量の取得
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
}