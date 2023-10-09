using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum GameStartUpState
{
    InitManagers,
    GeneratingWorld,
    InitPlayer,
    GenerateRivers,
    GenerateWeather,
    CompletedStartUp
};

public class GameManager : PersistentSingleton<GameManager>
{
    #region Variables
    #region Private
    [Header("States")]
    [SerializeField] private GameStartUpState[] m_startUpOrder; 
    public Action<GameStartUpState> OnNewGameState;
    public Action OnCurrentStateEnd;
    [ReadOnlyInspector] public GameStartUpState CurrentGameState;

    [Header("Character & Controller")]
    [SerializeField] private GameObject m_defaultPlayerPrefab;
    [SerializeField] private GameObject m_defaultControllerPrefab;

    public GridManager GridManager { get; private set; }
    public ChunkManager ChunkManager { get; private set; }
    public RiverManager RiverManager { get; private set; }
    public WeatherManager WeatherManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public UIManager UIManager { get; private set; }
    #endregion

    #region Public
    public TopDownCharacter Player { get; private set; }
    public Controller Control { get; private set; }
    #endregion
    #endregion


    #region OnStart
    private void Start()
    {
        for(int i = 0; i < m_startUpOrder.Length; i++)
        {
            BeginNewState(m_startUpOrder[i]);
        }
    }

    #region States
    public void BeginNewState(GameStartUpState newState)
    {
        OnNewGameState?.Invoke(newState);
        CurrentGameState = newState;
        switch (newState)
        {
            case GameStartUpState.InitManagers:
                InitManagers();
                break;

            case GameStartUpState.GeneratingWorld:
                GridManager.GenrateWorldTileGrid();
                break;

            case GameStartUpState.InitPlayer:
                SetupPlayer();
                break;

            case GameStartUpState.GenerateRivers:
                RiverManager.GenerateRivers();
                break;

            case GameStartUpState.GenerateWeather:
                WeatherManager.StartWeatherCycle();
                break;
        }

        OnCurrentStateEnd?.Invoke();
    }

    #endregion

    #region Managers
    private void InitManagers()
    {
        ChunkManager = SetUpManager<ChunkManager>();
        GridManager = SetUpManager<GridManager>();
        RiverManager = SetUpManager<RiverManager>();
        AudioManager = SetUpManager<AudioManager>();
        WeatherManager = SetUpManager<WeatherManager>();
        UIManager = SetUpManager<UIManager>();
    }

    private T SetUpManager<T>()
    {
        T mangerType = transform.parent.gameObject.GetComponentInChildren<T>();

        if (mangerType == null)
            throw new System.NullReferenceException("Manager of type " + mangerType + "is not set in Game Manager");

        IInitalizable initManager = mangerType as IInitalizable;
        
        if (initManager != null)
            initManager.Init();

        return mangerType;
    }

    #endregion

    private void SetupPlayer()
    {
        CreateController();
        CreatePlayerCharacter();

        Control.Possess(Player);
    }

    private Controller CreateController()
    {
        CheckInspectorFieldForScript<Controller>(m_defaultControllerPrefab);

        GameObject controlPrefab = Instantiate(m_defaultControllerPrefab);
        Control = controlPrefab.GetComponent<Controller>();
        return Control;
    }

    private GameObject CreatePlayerCharacter()
    {
        CheckInspectorFieldForScript<Character<Input_PlayerControls>>(m_defaultPlayerPrefab);

        Vector3 playerSpawnPosition = GridManager.centreTile.m_worldPos;
        GameObject playerObj = Instantiate(m_defaultPlayerPrefab, playerSpawnPosition, Quaternion.identity);

        IInitalizable[] scriptsOnPlayer = playerObj.GetComponentsInChildren<IInitalizable>();
        if (scriptsOnPlayer.Length > 0)
        {
            foreach (IInitalizable initalizable in scriptsOnPlayer)
            {
                initalizable.Init();
            }
        }

        Player = playerObj.GetComponent<TopDownCharacter>();

        return playerObj;
    }
    #endregion

    #region Helper Methods
    private void CheckInspectorFieldForScript<T>(GameObject prefab)
    {
        if(prefab == null)
            throw new System.NullReferenceException("Prefab in Game Manager must be set to a GameObject Prefab");

        T component = prefab.GetComponentInChildren<T>();

        if (component == null)
            throw new System.NullReferenceException("Prefab in Game Manager must have component of type: " + typeof(T).ToString());
    }

    #endregion
}
