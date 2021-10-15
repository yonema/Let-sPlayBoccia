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
     * @brief ボタンの切り替え処理
     * @author yoneji
     */
    public class SwitchButton : MonoBehaviour
    {
        [SerializeField]
        [Header("SetUpRotationImage")]
        Image m_upRotationImage;            //!< 上回転イメージ

        [SerializeField]
        [Header("SetDownRotationImage")]
        Image m_downRotationImage;          //!< 下回転イメージ

        [SerializeField]
        [Header("SetFastBallImage")]
        Image m_fastBallImage;              //!< 直球イメージ

        [SerializeField]
        [Header("SetArtchingBallImage")]
        Image m_artchingBallImage;          //!< 山なりイメージ

        BallThrower m_ballThrower;          //!< ボールを投げるクラス


        // Start is called before the first frame update
        void Start()
        {
            // 最初は上回転モード
            UpRotationMode();
            // 最初は直球
            FastBallType();
        }
        

        // Update is called once per frame
        void Update()
        {

        }

        /**
         * @brief ボールを投げるクラスを設定する
         * @param ballThrower ボールを投げるクラス
         */
        public void SetBallThrower(BallThrower ballThrower)
        {
            m_ballThrower = ballThrower;
            return;
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
         * @brief 投げ方ボタンに投げ方を設定する
         * @param throwType 投げ方
         */
        public void ThrowType(BallThrower.EnThrowType throwType)
        {
            // 直球か？
            if (throwType == BallThrower.EnThrowType.enFastBall)
            {
                // 直球に変更
                FastBallType();
            }
            else
            {
                // 山なりに変更
                ArchingBallType();
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
            ImageVisible(m_upRotationImage);
            // 下回転イメージを不可視化する
            ImageHidden(m_downRotationImage);

            return;
        }

        /**
         * @brief 下回転モードに変更する処理
         */
        void DownRotationMode()
        {
            Debug.Log("DownRotationMode");
            // 下回転イメージを可視化する
            ImageVisible(m_downRotationImage);
            // 上回転イメージを不可視化する
            ImageHidden(m_upRotationImage);
            return;
        }

        /**
         * @brief 直球に変更
         */
        void FastBallType()
        {
            Debug.Log("FastBallType");
            // 直球イメージを可視化する
            ImageVisible(m_fastBallImage);
            // 山なりイメージを不可視化する
            ImageHidden(m_artchingBallImage);
            return;
        }

        /**
         * @brief 山なりに変更
         */
        void ArchingBallType()
        {
            Debug.Log("ArchingBallType");
            // 山なりイメージを可視化する
            ImageVisible(m_artchingBallImage);
            // 直球イメージを不可視化する
            ImageHidden(m_fastBallImage);
            return;
        }

        /**
         * @biref イメージを可視化する
         */
        void ImageVisible(Image image)
        {
            // イメージの不透明にする
            Color color = image.color;
            color.a = 0.5f;
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

        /**
         * @brief 回転ボタンがクリックされたときに呼ばれる処理
         * @warning この関数の名前を変更した場合、CanvasのUpRotationのEventTriggerコンポーネントの
         * パラメータを変更しなければいけない。
         */
        public void OnClickRotationButton()
        {
            // 回転モードを現在のものから変更する処理
            m_ballThrower.ChangeRotation();
            return;
        }

        /**
         * @brief 投げ方ボタンがクリックされたときに呼ばれる処理
         * @warning この関数の名前を変更した場合、CanvasのFastBallのEventTriggerコンポーネントの
         * パラメータを変更しなければいけない。
         */
        public void OnClickThrowTypeButton()
        {
            // 投げ方を現在のものから変更する
            m_ballThrower.ChangeThorwType();
            return;
        }
    }
}