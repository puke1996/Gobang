using System;
using ExtensionMethods;
using UnityEngine;
using UnityEngine.UI;

public class GobangGUI
{
    // 棋子间距
    private readonly float _interval = 0.478f;

    /// <summary>
    /// 获取棋子的实际位置
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private Vector3 GetPosByIndex(int x, int y)
    {
        return new Vector3(x * _interval, 0, y * _interval);
    }

    /// <summary>
    /// 获取玩家点击位置的棋子编号
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public (bool, int x, int y ) GetIndexByPos(Vector3 pos)
    {
        var root = Finder.FindGOByName("PiecesRoot").transform;
        // 世界空间转局部空间
        var localPos = CoordinateSystemConversionUtils.WorldToLocalPos(root, pos);
        // 加上一个间隔
        localPos += new Vector3(_interval / 2f, 0, _interval / 2f);
        var temp = localPos / _interval;
        var x = (int) Math.Floor(temp.x);
        var y = (int) Math.Floor(temp.z);
        if (x < 0 || y < 0 || x >= 15 || y >= 15)
        {
            return (false, default, default);
        }

        return (true, x, y);
    }

    /// <summary>
    /// 绑定
    /// </summary>
    /// <param name="gobang"></param>
    public void Bind(Gobang gobang)
    {
        gobang.GameStart += delegate
        {
            var root = Finder.FindGOByName("PiecesRoot").transform;
            root.DestroyAllChildren();
        };
        gobang.PlayerMove += delegate(Player player, int x, int y)
        {
            var root = Finder.FindGOByName("PiecesRoot").transform;
            if (player == Player.White)
            {
                var chess = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("WhiteChess"), root);
                chess.transform.localPosition = GetPosByIndex(x, y);
            }

            if (player == Player.Black)
            {
                var chess = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("BlackChess"), root);
                chess.transform.localPosition = GetPosByIndex(x, y);
            }
        };
        gobang.GameOver += delegate(Result result)
        {
            Finder.FindGOByName("Result").GetComponentInChildren<Text>().text = result.ToString();
        };
    }
}