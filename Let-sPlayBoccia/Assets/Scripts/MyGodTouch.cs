using UnityEngine;

/**
 * @brief �u�{�b�`�����悤���v�̃l�[���X�y�[�X
 */
namespace nsLetsPlayBoccia
{
    /**
     * @brief Unity�̃G�f�B�^�[��ƒ[����ł̃^�b�`���ʉ�����
     * @author yoneji
     * @details ������̃T�C�g������p���Ă����R�[�h��������Ɖ���
     * https://qiita.com/tempura/items/4a5482ff6247ec8873df
     */
    public static class MyGodTouch
    {
        /// <summary>
        /// �f���^�|�W�V��������p�E�O��̃|�W�V����
        /// </summary>
        static Vector3 prebPosition;

        /// <summary>
        /// �^�b�`�����擾(�G�f�B�^�ƃX�}�z���l��)
        /// </summary>
        /// <returns>�^�b�`���</returns>
        public static GodPhase GetPhase()
        {

#if UNITY_EDITOR    // Unity�G�f�B�^�[��ł̑���

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
#else   // �[����ł̑���

            if (Input.touchCount > 0) return (GodPhase)((int)Input.GetTouch(0).phase);
            
#endif
            return GodPhase.None;
        }

        /// <summary>
        /// �^�b�`�|�W�V�������擾(�G�f�B�^�ƃX�}�z���l��)
        /// </summary>
        /// <returns>�^�b�`�|�W�V�����B�^�b�`����Ă��Ȃ��ꍇ�� (0, 0, 0)</returns>
        public static Vector3 GetPosition()
        {
#if UNITY_EDITOR    // Unity�G�f�B�^�[��ł̑���

            if (GetPhase() != GodPhase.None) return Input.mousePosition;

#else   // �[����ł̑���

            if (Input.touchCount > 0) return Input.GetTouch(0).position;
#endif

            return Vector3.zero;
        }

        /// <summary>
        /// �^�b�`�f���^�|�W�V�������擾(�G�f�B�^�ƃX�}�z���l��)
        /// </summary>
        /// <returns>�^�b�`�|�W�V�����B�^�b�`����Ă��Ȃ��ꍇ�� (0, 0, 0)</returns>
        public static Vector3 GetDeltaPosition()
        {
#if UNITY_EDITOR    // Unity�G�f�B�^�[��ł̑���

            var phase = GetPhase();
            if (phase != GodPhase.None)
            {
                var now = Input.mousePosition;
                var delta = now - prebPosition;
                prebPosition = now;
                return delta;
            }
#else   // �[����ł̑���

            if (Input.touchCount > 0) return Input.GetTouch(0).deltaPosition;
#endif

            return Vector3.zero;
        }
    }

    /// <summary>
    /// �^�b�`���BUnityEngine.TouchPhase �� None �̏���ǉ��g���B
    /// </summary>
    public enum GodPhase
    {
        None = -1,          // ��ʂɎw���G��Ă��Ȃ���
        Began = 0,          // ��ʂɎw���G�ꂽ��
        Moved = 1,          // ��ʏ�Ŏw�������Ă��鎞
        Stationary = 2,     // �w����ʂɐG��Ă��邪�A�����Ă͂��Ȃ���
        Ended = 3,          // ��ʂ���w�����ꂽ��
        Canceled = 4        // �V�X�e�����^�b�`�̒ǐՂ��L�����Z�����܂���
    }
}