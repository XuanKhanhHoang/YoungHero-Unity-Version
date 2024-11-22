using UnityEngine;

public enum GAME_STATE { FREE_ROAM, DIALOG, BATTLE, STORY_STATE, CHARACTER_STATE, TRADING_STATE, DIED, SETTING }

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    private Player_d_invt_State_Manager player_D_Invt_State_Manager;
    private MainCameraController mainCameraController;
    private GAME_STATE state;
    private PlayerController playerController;
    private DIedUIManager dIedUIManager;
    private GameSettingUIManager gameSettingUIManager;

    public GameStoryManager gameStoryManager;
    public bool isHandleInventory = true;

    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        playerController = PlayerController.Instance;
        mainCameraController = MainCameraController.instance;
        dIedUIManager = DIedUIManager.instance;
        player_D_Invt_State_Manager = DDOL_GameObj_Manager.instance.PlayerDetailAndInvetoryManager;
        gameSettingUIManager = DDOL_GameObj_Manager.instance.GameSettingUIManager;

        playerController.gameObject.SetActive(true);
        UIManager.instance.gameObject.SetActive(true);
        gameSettingUIManager.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (state == GAME_STATE.FREE_ROAM)
        {
            playerController.HandleFixedUpdate();

        }
    }
    void Update()
    {

        if (state == GAME_STATE.FREE_ROAM)
        {
            if (isHandleInventory && Input.GetKeyDown(KeyCode.Escape))
            {
                state = GAME_STATE.CHARACTER_STATE;
                return;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                state = GAME_STATE.SETTING;
                return;
            }
            DialogManager.instance.HideDialog();
            playerController.HandleUpdate();

        }
        else if (state == GAME_STATE.DIALOG)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                state = GAME_STATE.FREE_ROAM;

            }
        }
        else if (state == GAME_STATE.CHARACTER_STATE)
        {
            player_D_Invt_State_Manager.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                state = GAME_STATE.FREE_ROAM;
                player_D_Invt_State_Manager.gameObject.SetActive(false);

                return;
            }
            player_D_Invt_State_Manager.HandleUpdate();

        }
        else if (state == GAME_STATE.SETTING)
        {
            gameSettingUIManager.HandleUpdate();
        }
        else if (state == GAME_STATE.STORY_STATE)
        {
            gameStoryManager.HandleUpdate();
        }
        else if (state == GAME_STATE.TRADING_STATE)
        {
            TradingManager.instance.HandleUpdate();
        }
        else if (state == GAME_STATE.DIED)
        {
            dIedUIManager.HandleUpdate();
        }
    }


    //Game State
    public void SetGameState(GAME_STATE state)
    {
        this.state = state;
    }
    public GAME_STATE GetGameState() => state;
}
