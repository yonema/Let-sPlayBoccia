using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowPowerController : MonoBehaviour
{
    [SerializeField]
    [Header("SetThrowPowerGauseImage")]
    Image m_throwPowerGauseImage;           //!< 投げる強さゲージのイメージ

    [SerializeField]
    [Header("SetThrowPowerArrowImage")]
    Image m_throwPowerArrowImage;           //!< 投げる強さの矢印のイメージ

    [SerializeField]
    [Header("SetGauseWidthOffset")]
    float m_gauseWidthOffset = -10.0f;      //!< ゲージの幅のオフセット

    [SerializeField]
    [Header("SetGaugeSpeed")]
    float m_gaugeSpeed = 10.0f;             //!< ゲージの増減のスピード


    float m_gaugeMinPosX = 0.0f;        //!< ゲージの最小X座標
    float m_gaugeMaxPosX = 0.0f;        //!< ゲージの最大X座標
    Vector2 m_arrowPos = Vector2.zero;  //!< 矢印の座標

    bool m_isGaugeChanging = false;     //!< ゲージが増減するか？

    /**
     * @brief ゲージの増減モード
     */
    enum EnGaugeMode
    {
        enIncrease,     //!< 増量モード
        enDecrease      //!< 減量モード
    }

    EnGaugeMode m_gaugeMode = EnGaugeMode.enIncrease;   //!< ゲージの増減モード


    // Start is called before the first frame update
    void Start()
    {
        // ゲージの幅+オフセット
        float gaugeWidht = 
            m_throwPowerGauseImage.GetComponent<RectTransform>().sizeDelta.x + m_gauseWidthOffset;
        // 矢印の座標を取得
        m_arrowPos = m_throwPowerArrowImage.GetComponent<RectTransform>().anchoredPosition;

        // ゲージの最小X座標を最初の矢印の座標にする
        m_gaugeMinPosX = m_arrowPos.x;
        // ゲージの最大のX座標をゲージの最小のX座標とゲージの幅から計算する
        m_gaugeMaxPosX = m_gaugeMinPosX + gaugeWidht;

        return;
    }

    // Update is called once per frame
    void Update()
    {
        // ゲージが増減するか？
        if (m_isGaugeChanging != true)
        {
            // しない
            // 早期リターン
            return;
        }

        // 増減モードで割り振る
        switch (m_gaugeMode)
        {
            // 増量モード
            case EnGaugeMode.enIncrease:
                IncreaseGauge();
                break;

            // 減量モード
            case EnGaugeMode.enDecrease:
                DecreaseGauge();
                break;
        }

        return;
    }

    /**
     * @brief ゲージの増量処理
     */
    void IncreaseGauge()
    {
        // X座標を増やす
        m_arrowPos.x += m_gaugeSpeed;

        // 最大X座標以上か？
        if (m_arrowPos.x >= m_gaugeMaxPosX)
        {
            // 最大X座標の座標にする
            m_arrowPos.x = m_gaugeMaxPosX;
            // 減量モードに変更する
            m_gaugeMode = EnGaugeMode.enDecrease;
        }

        // 矢印の座標を設定する
        m_throwPowerArrowImage.GetComponent<RectTransform>().anchoredPosition = m_arrowPos;

        return;
    }

    /**
     * @ゲージの減量処理
     */
    void DecreaseGauge()
    {
        // X座標を減らす
        m_arrowPos.x -= m_gaugeSpeed;

        // 最小X座標以下か？
        if (m_arrowPos.x <= m_gaugeMinPosX)
        {
            // 最小X座標の座標にする
            m_arrowPos.x = m_gaugeMinPosX;
            // 増量モードにする
            m_gaugeMode = EnGaugeMode.enIncrease;
        }

        // 矢印の座標を設定する
        m_throwPowerArrowImage.GetComponent<RectTransform>().anchoredPosition = m_arrowPos;

        return;
    }

    /**
     * @brief ゲージの増減を開始する
     */
    public void StartGauge()
    {
        // ゲージの増減を行う
        m_isGaugeChanging = true;
    }

    /**
     * @brief ゲージの増減を終了して、ゲージのパワー率を取得する。
     * @return ゲージのパワー率（0.0f～1.0f）
     */
    public float EndGauge()
    {
        // ゲージの増減を行わない
        m_isGaugeChanging = false;

        // ゲージのパワー率を計算
        float gaugePowerRate = m_arrowPos.x - m_gaugeMinPosX;
        gaugePowerRate /= (m_gaugeMaxPosX - m_gaugeMinPosX);

        return gaugePowerRate;
    }

    /**
     * @brief パワーゲージをリセットする
     */
    public void ResetGauge()
    {
        // 矢印の位置を初期位置に戻す
        m_arrowPos.x = m_gaugeMinPosX;

        // 矢印の座標を設定する
        m_throwPowerArrowImage.GetComponent<RectTransform>().anchoredPosition = m_arrowPos;

        return;
    }

    /**
     * @brief ゲージが増減するか？を得る
     * @return ゲージが増減するか？
     */
    public bool IsGaugeChanging()
    {
        return m_isGaugeChanging;
    }

}
