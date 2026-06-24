using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField] private TypewriterEffect typewriter;
    [SerializeField] private TutorialCollider rightZone;
    [SerializeField] private Enemy enemy;

    [SerializeField] private float endDelay = 1.5f;

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
        rightZone.OnPlayerEnter += HandleRightZoneEnter;
        enemy.OnDead += HandleEnemyDead;
    }

    private void OnDisable()
    {
        Player.OnPlayerMove -= HandleMove;
        Player.OnRopeReady -= HandleRopeReady;
        Player.OnRopeConnected -= HandleRopeConnect;
        Player.OnRopeUsed -= HandleRopeUse;
        Player.OnKnotCollected -= HandleKnot;
        rightZone.OnPlayerEnter -= HandleRightZoneEnter;
        enemy.OnDead -= HandleEnemyDead;
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
                typewriter.StartTyping("A, D 키를 눌러 걸어보려무나.");
                break;

            case 1:
                typewriter.StartTyping("무서~운 못 근처에서 F 키를 눌러 네 매듭을 묶어보려무나.");
                break;

            case 2:
                typewriter.StartTyping("근처의 다른 못을 클릭하여 네 매듭을 이어보려무나\n" +
                    "수직 못은 수직으로만, 수평 못은 수평으로만 이을 수 있단다.");
                break;

            case 3:
                typewriter.StartTyping("연결된 매듭에 커서를 대고 F 키를 눌러보려무나.\n" +
                    "수직 로프는 네 팔 매듭이 네 몸에 제대로 붙어있을 때만 탈 수 있단다.\n" +
                    "매듭이 묶인 못에 커서를 대고, F키를 누르면 네 매듭을 다시 가져올 수 있단다.");
                break;

            case 4:
                typewriter.StartTyping("매듭을 회수해보렴.");
                break;
            case 5:
                typewriter.StartTyping("수평 못을 연결해서 오른쪽으로 넘어가보려무나.\n" +
                    "수평 로프는 네 다리 매듭이 네 몸에 제대로 붙어있을 때만 탈 수 있단다.");
                break;
            case 6:
                typewriter.StartTyping("도착했구나! 이제 사악한 저 악마를 죽여보자꾸나.");
                break;
            case 7:
                typewriter.StartTyping("아주 잘했단다.\n" +
                    "지금부터 펼쳐질 이야기를 충분히 즐기고 돌아오렴!");
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

    private void HandleRightZoneEnter()
    {
        if(step == 5) Next();
    }

    private void HandleEnemyDead()
    {
        if (step == 6) Next();

        StartCoroutine(CoTutorialEnd());
    }

    private IEnumerator CoTutorialEnd()
    {
        yield return new WaitForSeconds(endDelay);

        FadeManager.Instance.LoadSceneWithFade("Chapter1");

    }
}