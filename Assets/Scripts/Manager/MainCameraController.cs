using UnityEngine;
using UnityEngine.SceneManagement;

public enum CAMERA_OBJECT_FOLLOW_STATE { PLAYER, NPC, STATIC }
public class MainCameraController : MonoBehaviour
{
    public static MainCameraController instance;
    public Vector2 minPos, maxPos;

    private Transform target;
    private CAMERA_OBJECT_FOLLOW_STATE cameraObjectFollowState;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameObject.SetActive(scene.buildIndex != 4);

    }
    void OnDestroy() { SceneManager.sceneLoaded -= OnSceneLoaded; }
    public void SetNPCTarget(Transform newTarget)
    {
        target = newTarget;
        this.cameraObjectFollowState = CAMERA_OBJECT_FOLLOW_STATE.NPC;
    }
    public void SetPlayerTarget()
    {
        this.cameraObjectFollowState = CAMERA_OBJECT_FOLLOW_STATE.PLAYER;
    }
    public void SetCameraStaticPos(Vector3 des)
    {
        des.z = transform.position.z;
        transform.position = des;
        this.cameraObjectFollowState = CAMERA_OBJECT_FOLLOW_STATE.STATIC;
    }
    void LateUpdate()
    {
        if (cameraObjectFollowState == CAMERA_OBJECT_FOLLOW_STATE.NPC)
        {
            Vector3 targetPos = new(target.position.x, target.position.y, transform.position.z);
            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.6f);
        }
        else if (cameraObjectFollowState == CAMERA_OBJECT_FOLLOW_STATE.PLAYER)
        {
            Vector3 targetPos = new(PlayerController.Instance.transform.position.x, PlayerController.Instance.transform.position.y, transform.position.z);
            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.6f);
        }
    }

}

