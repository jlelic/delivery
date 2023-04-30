
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegMover : MonoBehaviour
{
    [SerializeField] private Transform pegToMove;
    [SerializeField] private List<Transform> travelPoints;
    [Tooltip("Speed to move from one travel point to another")]
    [SerializeField] private float time = 1f;
    [Tooltip("Whether to continously loop peg travel")]
    [SerializeField] private bool loop = true;

    private void Start()
    {
        StartCoroutine(MovePeg());
    }

    private IEnumerator MovePeg()
    {
        do
        {
            yield return TraversePoints();
        } while (loop);
    }

    private IEnumerator TraversePoints()
    {
        for (int i = 0; i < travelPoints.Count; i++)
        {
            yield return MovePegToNextPoint(i);
        }
    }

    private IEnumerator MovePegToNextPoint(int index)
    {
        do
        {
            if (pegToMove != null)
            {
                pegToMove.localPosition = Vector3.MoveTowards(pegToMove.localPosition, travelPoints[index].localPosition, Time.deltaTime / time);
            }
            yield return null;
        }
        while (pegToMove != null && pegToMove.localPosition != travelPoints[index].localPosition);

        if (pegToMove == null)
        {
            Destroy(gameObject);

        }
    }
}
