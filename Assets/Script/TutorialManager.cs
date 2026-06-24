using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField] private TypewriterEffect typewriter;

    private int step = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Player.OnPlayerMove += HandleMove;
        Player.OnRopeReady += HandleRopeReady;
        Player.OnRopeConnected += HandleRopeConnect;
        Player.OnRopeUsed += HandleRopeUse;
        Player.OnKnotCollected += HandleKnot;
    }

    private void OnDisable()
    {
        Player.OnPlayerMove -= HandleMove;
        Player.OnRopeReady -= HandleRopeReady;
        Player.OnRopeConnected -= HandleRopeConnect;
        Player.OnRopeUsed -= HandleRopeUse;
        Player.OnKnotCollected -= HandleKnot;
    }

    private void Start()
    {
        ShowStep();
    }

    private void ShowStep()
    {
        switch (step)
        {
            case 0:
                typewriter.StartTyping("A,D로 이동해보세요.");
                break;

            case 1:
                typewriter.StartTyping("못 근처에서 F를 눌러 로프를 준비하세요.");
                break;

            case 2:
                typewriter.StartTyping("다른 못을 클릭해 로프를 연결하세요.\n" +
                    "수직 못은 수직 못끼리만, 수평 못은 수평 못끼리만 연결 가능합니다.");
                break;

            case 3:
                typewriter.StartTyping("연결된 로프에 마우스를 가져다대고 F를 눌러보세요.\n" +
                    "수직 로프는 팔 매듭이 있어야만 탈 수 있습니다. 매듭이 묶인 못에 마우스를 대고 F를 누르면 회수할 수 있습니다.");
                break;

            case 4:
                typewriter.StartTyping("매듭을 회수해보세요. 매듭은 ");
                break;
            case 5:
                typewriter.StartTyping("매듭을 회수해보세요.");
                break;
        }
    }

    private void Next()
    {
        step++;
        ShowStep();
    }

    private void HandleMove()
    {
        if (step == 0) Next();
    }

    private void HandleRopeReady()
    {
        if (step == 1) Next();
    }

    private void HandleRopeConnect()
    {
        if (step == 2) Next();
    }

    private void HandleRopeUse()
    {
        if (step == 3) Next();
    }

    private void HandleKnot()
    {
        if (step == 4) Next();
    }
}