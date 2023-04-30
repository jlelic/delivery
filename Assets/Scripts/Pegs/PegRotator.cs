using UnityEngine;

public class PegRotator : MonoBehaviour
{
    private enum LOOP_DIRECTION
    {
        ROTATE_BACK,
        ROTATE_FORWARD
    }

    [SerializeField] private float angle = 360f;
    [Tooltip("Time to rotate, in seconds")]
    [SerializeField] private float time = 2f;
    [Tooltip("Whether to continously rotate pegs")]
    [SerializeField] private bool loop = true;
    [Tooltip("If looping, dictate rotate direction of the next loop, BACK = return to angle of previous loop, FORWARD = continue applying angle")]
    [SerializeField] private LOOP_DIRECTION loopDirection = LOOP_DIRECTION.ROTATE_FORWARD;

    private void Start()
    {
        foreach (Transform item in transform)
        {
            item.gameObject.AddComponent<PegLockLocalRotation>();
        }
        RotatePegs();
    }

    private LTDescr RotatePegs()
    {
        if (loop)
        {
            if (loopDirection == LOOP_DIRECTION.ROTATE_BACK)
            {
                return RotateAround().setLoopPingPong();
            }
            return RotateAround().setLoopCount(-1);
        }
        return RotateAround();
    }

    private LTDescr RotateAround()
    {
        return LeanTween.rotateAround(gameObject, new Vector3(0f, 0f, 1f), angle, time);
    }
}
