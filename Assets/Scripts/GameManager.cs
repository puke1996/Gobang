using Singleton;

public class GameManager : MonoSingleton<GameManager>
{
    public Gobang Gobang { get; private set; }
    public GobangGUI GobangGUI { get; private set; }

    private void Start()
    {
        // 创建围棋程序
        Gobang = new Gobang();
        // 创建围棋客户端
        GobangGUI = new GobangGUI();
        // 绑定
        GobangGUI.Bind(Gobang);
        // 开始
        Gobang.Start();
    }
}