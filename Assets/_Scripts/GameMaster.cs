using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }





    public float AudioMaster = 0.8f;
    public float AudioMusic = 0.8f;
    public float AudioSFX = 0.8f;

    public float MouseSensitiviy = 1.0f;

    public KeyCode rightKey = KeyCode.D;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backKey = KeyCode.S;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;

    public KeyCode buildModeKey = KeyCode.B;
    public KeyCode invKey = KeyCode.Tab;
    public KeyCode sheathKey = KeyCode.F;
    public KeyCode rotateKey = KeyCode.R;
    public KeyCode buildingKey = KeyCode.Mouse1;

    public KeyCode attackKey = KeyCode.Mouse0;
}
