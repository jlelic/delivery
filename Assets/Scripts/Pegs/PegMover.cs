
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

    private Vector3[] positions;

    private void Start()
    {
        positions = new Vector3[travelPoints.Count];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = travelPoints[i].position;
        };


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
        for (int i = 0; i < positions.Length; i++)
        {
            yield return MovePegToNextPoint(i);
        }
    }

    private IEnumerator MovePegToNextPoint(int index)
    {
        while (pegToMove.position != positions[index])
        {
            if (pegToMove.gameObject != null)
            {
                pegToMove.localPosition = Vector3.MoveTowards(pegToMove.localPosition, positions[index], Time.deltaTime / time);
            }
            else
            {
                Destroy(gameObject);
            }
            yield return null;
        }
    }
}
