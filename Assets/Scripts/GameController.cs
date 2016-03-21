using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    Vector3[] startingPos = new Vector3[4] {
            new Vector3(3, 2, 3),
            new Vector3(3, 2, -3),
            new Vector3(-3, 2, 3),
            new Vector3(-3, 2, -3)
        };

    static GameController _instance;
    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }


    public GameObject playerPrefab;
    public GameObject ennemyAIprefab;

    public int numberOfPlayers = 1;
    public int numberOfFighters = 4;


    public Fighter[] Fighters;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LaunchGame();
    }

    #region Private methods
    void SetupSceneForPlayers(int n)
    {
        Fighters = new Fighter[numberOfFighters];

        for (int i = 0; i < n; i++) {
            Fighters[i] = ((GameObject)(Instantiate(playerPrefab, startingPos[i], Quaternion.identity))).GetComponent<InputController>();
            Fighters[i].fighterName = "Player " + i.ToString();
        }

        for(int i = n; i < numberOfFighters; i++)
        {
            Fighters[i] = ((GameObject)(Instantiate(ennemyAIprefab, startingPos[i], Quaternion.identity))).GetComponent<AIController>();
            Fighters[i].fighterName = "AI " + i;
        }

        for(int j = n; j < Fighters.Length; j++)
        {
            Fighters[j].GetComponent<AIController>().Init();
        }
    }

    void Cleanup()
    {
        for (int i = 0; i < Fighters.Length; i++)
        {
            Destroy(Fighters[i].gameObject);
        }
        Fighters = null;
    }
    #endregion

    #region Public methods
    public void ResetGame()
    {
        Cleanup();

        LaunchGame();
    }

    public void LaunchGame()
    {
        SetupSceneForPlayers(numberOfPlayers);

        GetComponent<BalanceController>().Initialize();
    }
    #endregion


}
