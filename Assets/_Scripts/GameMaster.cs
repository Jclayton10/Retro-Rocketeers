using UnityEngine;
using UnityEngine.InputSystem;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    

    [Header("Gameplay Based")]
    public float AudioMaster = 0.8f;
    public float AudioMusic = 0.8f;
    public float AudioSFX = 0.8f;
    public float MouseSensitiviy = 1.0f;

    public Vector2 MoveInput { get; private set; }
    public bool JumpJustPressed { get; private set; }
    public bool BuildModePressed { get; private set; }
    public bool InvPressed { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool SprintBeingPressed { get; private set; }
    public bool SheathJustPressed { get; private set; }
    public bool RotateJustPressed { get; private set; }
    public bool BuildJustPressed { get; private set; }
    public bool AttackJustPressed { get; private set; }
    public bool PauseJustPressed { get; private set; }

    private PlayerInput _playerInput;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _buildmodeAction;
    private InputAction _invAction;
    private InputAction _interactAction;
    private InputAction _sprintAction;
    private InputAction _sheathSwordAction;
    private InputAction _rotateAction;
    private InputAction _buildAction;
    private InputAction _attackAction;
    private InputAction _pauseAction;


    private void Awake()
    {
       if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
       

        _playerInput = GetComponent<PlayerInput>();
        SetupInputActions();


        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void SetupInputActions()
    {
        _pauseAction = _playerInput.actions["Pause"];
        _jumpAction = _playerInput.actions["Jump"];
        _buildmodeAction = _playerInput.actions["BuildMode"];
        _invAction = _playerInput.actions["Inventory"];
        _interactAction = _playerInput.actions["Interact"];
        _sprintAction = _playerInput.actions["Sprint"];
        _sheathSwordAction = _playerInput.actions["Sheath"];
        _rotateAction = _playerInput.actions["Rotate"];
        _buildAction = _playerInput.actions["Build"];
        _attackAction = _playerInput.actions["Attack"];
        _moveAction = _playerInput.actions["Move"];
    }

    private void UpdateInputs()
    {
        PauseJustPressed = _pauseAction.WasPressedThisFrame();
        JumpJustPressed = _jumpAction.WasPressedThisFrame();
        BuildModePressed = _buildmodeAction.WasPressedThisFrame();
        InvPressed = _invAction.WasPressedThisFrame();
        InteractPressed = _interactAction.WasPressedThisFrame();
        SprintBeingPressed = _sprintAction.WasPressedThisFrame();
        SheathJustPressed = _sheathSwordAction.WasPressedThisFrame();
        RotateJustPressed = _rotateAction.WasPressedThisFrame();
        BuildJustPressed = _buildAction.WasPressedThisFrame();
        AttackJustPressed = _attackAction.WasPressedThisFrame();
        MoveInput = _moveAction.ReadValue<Vector2>();
    }

}
