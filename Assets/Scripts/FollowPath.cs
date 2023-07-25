using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public enum MovementType
    {
        Moving,
        Lerping
    }
    public MovementType Type = MovementType.Moving;
    public MovementPath MyPath;
    public float Speed = 1;
    public float maxDistance = 0.1f;

    private IEnumerator<Transform> PointInPath;

    private void Start()
    {
        if(MyPath == null)
        {
            Debug.Log("Выбери путь");
            return;
        }

        PointInPath = MyPath.GetNextPathPoint();

        PointInPath.MoveNext();

        if(PointInPath == null)
        {
            Debug.Log("В пути нет точек");
            return;
        }

        transform.position = PointInPath.Current.position;
    }

    private void Update()
    {
        if (PointInPath == null || PointInPath.Current == null)
        {
            return;
        }

        if (Type == MovementType.Moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, PointInPath.Current.position, Time.deltaTime * Speed);
        }
        else if (Type == MovementType.Lerping)
        {
			transform.position = Vector3.Lerp(transform.position, PointInPath.Current.position, Time.deltaTime * Speed);
		}

        var distanceSquare = (transform.position - PointInPath.Current.position).sqrMagnitude;
        if(distanceSquare < maxDistance * maxDistance)
        {
            PointInPath.MoveNext();
        }
    }
}
