using Mouse;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    /// <summary>
    /// 轮流落子
    /// </summary>
    private void Update()
    {
        // 鼠标点下立即落子
        if (WorldMouse.Instance.LeftMouseButtonDown)
        {
            Debug.Log("Update");
            var raycastResult = RaycastUtil.RaycastFromScreen(Camera.main, Input.mousePosition, float.MaxValue);
            // 如果落在棋盘上
            if (raycastResult.Item1)
            {
                Debug.Log("Click");
                var result = GameManager.Instance.GobangGUI.GetIndexByPos(raycastResult.Item2.point);
                // 如果id是有效的,0-14
                if (result.Item1)
                {
                    GameManager.Instance.Gobang.Move(GameManager.Instance.Gobang.NextPlayer, result.Item2,
                        result.Item3);
                }
            }
        }
    }
}