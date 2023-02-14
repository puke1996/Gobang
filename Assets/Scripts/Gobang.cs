using System;
using System.Linq;

/// <summary>
/// 位置状态,空,黑,白
/// </summary>
public enum PositoinState
{
    Empty,
    Black,
    White
}

/// <summary>
/// 玩家,黑,白
/// </summary>
public enum Player
{
    Black,
    White
}

/// <summary>
/// 结局,黑胜,白胜,平局
/// </summary>
public enum Result
{
    BlackWin,
    WhiteWin,
    Draw
}

/// <summary>
/// 五子棋
/// </summary>
public sealed class Gobang
{
    private PositoinState[,] _current;

    public delegate void GameStartHandler();

    public event GameStartHandler GameStart;

    public delegate void PlayerMoveHandler(Player player, int x, int y);

    public event PlayerMoveHandler PlayerMove;

    public delegate void GameOverHandler(Result result);

    public event GameOverHandler GameOver;
    public Player NextPlayer { get; private set; }
    public bool IsPlaying { get; private set; } = false;

    public void Start()
    {
        IsPlaying = true;
        _current = new PositoinState[15, 15];
        for (var i = 0; i < 15; i++)
        {
            for (var j = 0; j < 15; j++)
            {
                _current[i, j] = PositoinState.Empty;
            }
        }

        NextPlayer = Player.Black;
        OnGameStart();
    }

    /// <summary>
    /// 落子
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="player"></param>
    public void Move(Player player, int x, int y)
    {
        // 游戏还未开始
        if (IsPlaying == false)
        {
            return;
        }

        // 不能落子在棋盘外
        if (x < 0 || x > 14 || y < 0 || y > 14)
        {
            return;
        }

        // 不能落子在有棋子的位置
        if (_current[x, y] != PositoinState.Empty)
        {
            return;
        }

        // 不能一方连续落子
        if (player != NextPlayer)
        {
            return;
        }

        // 落子
        if (player == Player.Black)
        {
            _current[x, y] = PositoinState.Black;
        }

        if (player == Player.White)
        {
            _current[x, y] = PositoinState.White;
        }

        // 交换棋手
        switch (NextPlayer)
        {
            case Player.White:
                NextPlayer = Player.Black;
                break;
            case Player.Black:
                NextPlayer = Player.White;
                break;
        }

        OnMove(player, x, y);
        Judgement(x, y);
    }

    /// <summary>
    /// 判断胜负
    /// </summary>
    /// <param name="positoinStates"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Judgement(int x, int y)
    {
        var placeState = _current[x, y];
        {
            var startX = x - 4;
            for (var i = 0; i < 5; i++)
            {
                // 如果超过数组范围,就跳过
                try
                {
                    // 如果棋色一致
                    if (_current[startX + i, y] == placeState &&
                        _current[startX + i + 1, y] == placeState &&
                        _current[startX + i + 2, y] == placeState &&
                        _current[startX + i + 3, y] == placeState &&
                        _current[startX + i + 4, y] == placeState)
                    {
                        switch (placeState)
                        {
                            case PositoinState.Black:
                                IsPlaying = false;
                                OnGameOver(Result.BlackWin);
                                return;
                            case PositoinState.White:
                                IsPlaying = false;
                                OnGameOver(Result.WhiteWin);
                                return;
                        }
                    }
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
        }
        // 竖
        {
            var startY = y - 4;

            for (var i = 0; i < 5; i++)
            {
                try
                {
                    // 如果棋色一致
                    if (_current[x, startY + i] == placeState &&
                        _current[x, startY + i + 1] == placeState &&
                        _current[x, startY + i + 2] == placeState &&
                        _current[x, startY + i + 3] == placeState &&
                        _current[x, startY + i + 4] == placeState)
                    {
                        switch (placeState)
                        {
                            case PositoinState.Black:
                                IsPlaying = false;
                                OnGameOver(Result.BlackWin);
                                return;
                            case PositoinState.White:
                                IsPlaying = false;
                                OnGameOver(Result.WhiteWin);
                                return;
                        }
                    }
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
        }
        // 撇
        {
            var startX = x - 4;
            var startY = y - 4;
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    // 如果棋色一致
                    if (_current[startX + i, startY + i] == placeState &&
                        _current[startX + i + 1, startY + i + 1] == placeState &&
                        _current[startX + i + 2, startY + i + 2] == placeState &&
                        _current[startX + i + 3, startY + i + 3] == placeState &&
                        _current[startX + i + 4, startY + i + 4] == placeState)
                    {
                        switch (placeState)
                        {
                            case PositoinState.Black:
                                IsPlaying = false;
                                OnGameOver(Result.BlackWin);
                                return;
                            case PositoinState.White:
                                IsPlaying = false;
                                OnGameOver(Result.WhiteWin);
                                return;
                        }
                    }
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
        }
        // 捺
        {
            var startX = x - 4;
            var startY = y + 4;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    // 如果棋色一致
                    if (_current[startX + i, startY - i] == placeState &&
                        _current[startX + i + 1, startY - i - 1] == placeState &&
                        _current[startX + i + 2, startY - i - 2] == placeState &&
                        _current[startX + i + 3, startY - i - 3] == placeState &&
                        _current[startX + i + 4, startY - i - 4] == placeState)
                    {
                        switch (placeState)
                        {
                            case PositoinState.Black:
                                IsPlaying = false;
                                OnGameOver(Result.BlackWin);
                                return;
                            case PositoinState.White:
                                IsPlaying = false;
                                OnGameOver(Result.WhiteWin);
                                return;
                        }
                    }
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
        }
        var existEmpty = _current.Cast<PositoinState>().Any(state => state == PositoinState.Empty);
        if (!existEmpty)
        {
            IsPlaying = false;
            OnGameOver(Result.Draw);
        }
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <param name="result"></param>
    private void OnGameOver(Result result)
    {
        GameOver?.Invoke(result);
    }

    private void OnMove(Player player, int x, int y)
    {
        PlayerMove?.Invoke(player, x, y);
    }

    private void OnGameStart()
    {
        GameStart?.Invoke();
    }
}