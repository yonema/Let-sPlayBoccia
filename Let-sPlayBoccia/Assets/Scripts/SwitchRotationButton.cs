using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * @brief 「ボッチャしようぜ」のネームスペース
 */
namespace nsLetsPlayBoccia
{
    /**
     * @brief 回転ボタンの切り替え処理
     * @author yoneji
     */
    public class SwitchRotationButton : MonoBehaviour
    {
        [SerializeField]
        [Header("SetUpRotationImage")]
        Image m_UpRotationImage;           //!< 上回転イメージ

        [SerializeField]
        [Header("SetDownRotationImage")]
        Image m_DownRotationImage;         //!< 下回転イメージ

        BallThrower m_ballThrower;


        // Start is called before the first frame update
        void Start()
        {

            // 最初は上回転モード
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
         * @brief 回転ボタンに回転モードを設定する
         * @param ballRotation ボールの回転
         */
        public void RotationMode(BallThrower.EnBallRotation ballRotation)
        {
            // 上回転か？
            if (ballRotation == BallThrower.EnBallRotation.enUpRotation)
            {
                // 上回転モードに変更
                UpRotationMode();
            }
            else
            {
                // 下回転モードに変更
                DownRotationMode();
            }

            return;
        }

        /**
         * @brief 上回転モードに変更する処理
         */
        void UpRotationMode()
        {
            Debug.Log("UpRotationMode");
            // 上回転イメージを可視化する
            ImageVisible(m_UpRotationImage);
            // 下回転イメージを不可視化する
            ImageHidden(m_DownRotationImage);

            return;
        }

        /**
         * @brief 下回転モードに変更する処理
         */
        void DownRotationMode()
        {
            Debug.Log("DownRotationMode");
            // 下回転イメージを可視化する
            ImageVisible(m_DownRotationImage);
            // 上回転イメージを不可視化する
            ImageHidden(m_UpRotationImage);
            return;
        }

        /**
         * @biref イメージを可視化する
         */
        void ImageVisible(Image image)
        {
            // イメージの不透明にする
            Color color = image.color;
            color.a = 1.0f;
            image.color = color;

            return;
        }

        /**
         * @brief イメージを不可視化する
         */
        void ImageHidden(Image image)
        {
            // イメージを透明にする
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