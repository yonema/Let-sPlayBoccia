using UnityEngine;

/**
 * @brief 「ボッチャしようぜ」のネームスペース
 */
namespace nsLetsPlayBoccia
{
    /**
     * @brief Unityのエディター上と端末上でのタッチ共通化処理
     * @author yoneji
     * @details こちらのサイトから引用してきたコードをちょっと改造
     * https://qiita.com/tempura/items/4a5482ff6247ec8873df
     */
    public static class MyGodTouch
    {
        /// <summary>
        /// デルタポジション判定用・前回のポジション
        /// </summary>
        static Vector3 prebPosition;

        /// <summary>
        /// タッチ情報を取得(エディタとスマホを考慮)
        /// </summary>
        /// <returns>タッチ情報</returns>
        public static GodPhase GetPhase()
        {

#if UNITY_EDITOR    // Unityエディター上での操作

            if (Input.GetMouseButtonDown(0))
            {
                prebPosition = Input.mousePosition;
                return GodPhase.Began;
            }
            else if (Input.GetMouseButton(0))
            {
                return GodPhase.Moved;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                return GodPhase.Ended;
            }
#else   // 端末上での操作

            if (Input.touchCount > 0) return (GodPhase)((int)Input.GetTouch(0).phase);
            
#endif
            return GodPhase.None;
        }

        /// <summary>
        /// タッチポジションを取得(エディタとスマホを考慮)
        /// </summary>
        /// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
        public static Vector3 GetPosition()
        {
#if UNITY_EDITOR    // Unityエディター上での操作

            if (GetPhase() != GodPhase.None) return Input.mousePosition;

#else   // 端末上での操作

            if (Input.touchCount > 0) return Input.GetTouch(0).position;
#endif

            return Vector3.zero;
        }

        /// <summary>
        /// タッチデルタポジションを取得(エディタとスマホを考慮)
        /// </summary>
        /// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
        public static Vector3 GetDeltaPosition()
        {
#if UNITY_EDITOR    // Unityエディター上での操作

            var phase = GetPhase();
            if (phase != GodPhase.None)
            {
                var now = Input.mousePosition;
                var delta = now - prebPosition;
                prebPosition = now;
                return delta;
            }
#else   // 端末上での操作

            if (Input.touchCount > 0) return Input.GetTouch(0).deltaPosition;
#endif

            return Vector3.zero;
        }
    }

    /// <summary>
    /// タッチ情報。UnityEngine.TouchPhase に None の情報を追加拡張。
    /// </summary>
    public enum GodPhase
    {
        None = -1,          // 画面に指が触れていない時
        Began = 0,          // 画面に指が触れた時
        Moved = 1,          // 画面上で指が動いている時
        Stationary = 2,     // 指が画面に触れているが、動いてはいない時
        Ended = 3,          // 画面から指が離れた時
        Canceled = 4        // システムがタッチの追跡をキャンセルしました
    }
}