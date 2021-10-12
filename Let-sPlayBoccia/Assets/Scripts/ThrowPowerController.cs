using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowPowerController : MonoBehaviour
{
    [SerializeField]
    [Header("SetThrowPowerGauseImage")]
    Image m_throwPowerGauseImage;           //!< �����鋭���Q�[�W�̃C���[�W

    [SerializeField]
    [Header("SetThrowPowerArrowImage")]
    Image m_throwPowerArrowImage;           //!< �����鋭���̖��̃C���[�W

    [SerializeField]
    [Header("SetGauseWidthOffset")]
    float m_gauseWidthOffset = -10.0f;      //!< �Q�[�W�̕��̃I�t�Z�b�g

    [SerializeField]
    [Header("SetGaugeSpeed")]
    float m_gaugeSpeed = 10.0f;             //!< �Q�[�W�̑����̃X�s�[�h


    float m_gaugeMinPosX = 0.0f;        //!< �Q�[�W�̍ŏ�X���W
    float m_gaugeMaxPosX = 0.0f;        //!< �Q�[�W�̍ő�X���W
    Vector2 m_arrowPos = Vector2.zero;  //!< ���̍��W

    bool m_isGaugeChanging = false;     //!< �Q�[�W���������邩�H

    /**
     * @brief �Q�[�W�̑������[�h
     */
    enum EnGaugeMode
    {
        enIncrease,     //!< ���ʃ��[�h
        enDecrease      //!< ���ʃ��[�h
    }

    EnGaugeMode m_gaugeMode = EnGaugeMode.enIncrease;   //!< �Q�[�W�̑������[�h


    // Start is called before the first frame update
    void Start()
    {
        // �Q�[�W�̕�+�I�t�Z�b�g
        float gaugeWidht = 
            m_throwPowerGauseImage.GetComponent<RectTransform>().sizeDelta.x + m_gauseWidthOffset;
        // ���̍��W���擾
        m_arrowPos = m_throwPowerArrowImage.GetComponent<RectTransform>().anchoredPosition;

        // �Q�[�W�̍ŏ�X���W���ŏ��̖��̍��W�ɂ���
        m_gaugeMinPosX = m_arrowPos.x;
        // �Q�[�W�̍ő��X���W���Q�[�W�̍ŏ���X���W�ƃQ�[�W�̕�����v�Z����
        m_gaugeMaxPosX = m_gaugeMinPosX + gaugeWidht;

        return;
    }

    // Update is called once per frame
    void Update()
    {
        // �Q�[�W���������邩�H
        if (m_isGaugeChanging != true)
        {
            // ���Ȃ�
            // �������^�[��
            return;
        }

        // �������[�h�Ŋ���U��
        switch (m_gaugeMode)
        {
            // ���ʃ��[�h
            case EnGaugeMode.enIncrease:
                IncreaseGauge();
                break;

            // ���ʃ��[�h
            case EnGaugeMode.enDecrease:
                DecreaseGauge();
                break;
        }

        return;
    }

    /**
     * @brief �Q�[�W�̑��ʏ���
     */
    void IncreaseGauge()
    {
        // X���W�𑝂₷
        m_arrowPos.x += m_gaugeSpeed;

        // �ő�X���W�ȏォ�H
        if (m_arrowPos.x >= m_gaugeMaxPosX)
        {
            // �ő�X���W�̍��W�ɂ���
            m_arrowPos.x = m_gaugeMaxPosX;
            // ���ʃ��[�h�ɕύX����
            m_gaugeMode = EnGaugeMode.enDecrease;
        }

        // ���̍��W��ݒ肷��
        m_throwPowerArrowImage.GetComponent<RectTransform>().anchoredPosition = m_arrowPos;

        return;
    }

    /**
     * @�Q�[�W�̌��ʏ���
     */
    void DecreaseGauge()
    {
        // X���W�����炷
        m_arrowPos.x -= m_gaugeSpeed;

        // �ŏ�X���W�ȉ����H
        if (m_arrowPos.x <= m_gaugeMinPosX)
        {
            // �ŏ�X���W�̍��W�ɂ���
            m_arrowPos.x = m_gaugeMinPosX;
            // ���ʃ��[�h�ɂ���
            m_gaugeMode = EnGaugeMode.enIncrease;
        }

        // ���̍��W��ݒ肷��
        m_throwPowerArrowImage.GetComponent<RectTransform>().anchoredPosition = m_arrowPos;

        return;
    }

    /**
     * @brief �Q�[�W�̑������J�n����
     */
    public void StartGauge()
    {
        // �Q�[�W�̑������s��
        m_isGaugeChanging = true;
    }

    /**
     * @brief �Q�[�W�̑������I�����āA�Q�[�W�̃p���[�����擾����B
     * @return �Q�[�W�̃p���[���i0.0f�`1.0f�j
     */
    public float EndGauge()
    {
        // �Q�[�W�̑������s��Ȃ�
        m_isGaugeChanging = false;

        // �Q�[�W�̃p���[�����v�Z
        float gaugePowerRate = m_arrowPos.x - m_gaugeMinPosX;
        gaugePowerRate /= (m_gaugeMaxPosX - m_gaugeMinPosX);

        return gaugePowerRate;
    }

    /**
     * @brief �p���[�Q�[�W�����Z�b�g����
     */
    public void ResetGauge()
    {
        // ���̈ʒu�������ʒu�ɖ߂�
        m_arrowPos.x = m_gaugeMinPosX;

        // ���̍��W��ݒ肷��
        m_throwPowerArrowImage.GetComponent<RectTransform>().anchoredPosition = m_arrowPos;

        return;
    }

    /**
     * @brief �Q�[�W���������邩�H�𓾂�
     * @return �Q�[�W���������邩�H
     */
    public bool IsGaugeChanging()
    {
        return m_isGaugeChanging;
    }

}
