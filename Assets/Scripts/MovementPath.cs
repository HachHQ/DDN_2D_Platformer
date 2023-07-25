using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementPath : MonoBehaviour
{
	public enum PathTypes //виды путей
	{
		linear,
		loop
	}

	public PathTypes PathType;
	public int movementDirection = 1; //направление вперед или назад
	public int movingTo = 0;
	public Transform[] PathElements; //массив точек движения

	public void OnDrawGizmos() // отображение линий и точек во вьюпорте
	{
		if (PathElements == null || PathElements.Length < 2) return;

		for (var i = 1; i < PathElements.Length; i++) //проходится по всем точкам массива
		{
			Gizmos.DrawLine(PathElements[i - 1].position, PathElements[i].position); //рисует линии от точки к точке
		}

		if (PathType == PathTypes.loop) //если путь замкнется
		{
			Gizmos.DrawLine(PathElements[0].position, PathElements[PathElements.Length - 1].position); //замыкается первая и последня точки
		}
	}

	public IEnumerator<Transform> GetNextPathPoint()// получаем положение следующей точки
	{
		if(PathElements == null || PathElements.Length < 1)// проверяет есть ли точки которым нужно проверить положение 
		{
			yield break;
		}

		while(true)
		{
			yield return PathElements[movingTo];// возвращаем текущее положение точки

			if(PathElements.Length == 1)// если точка всего одна
			{
				continue;
			}

			if(PathType == PathTypes.linear) // если линия не замкнута
			{
				if(movingTo <= 0)// если двигаемся по нарастающей
				{
					movementDirection = 1;
				}
				else if (movingTo > PathElements.Length - 1) // если двигаемя по убывающей
				{
					movementDirection = -1;
				}
			}

			movingTo = movingTo + movementDirection; //диапазон движения от 1 до -1

			if (PathType == PathTypes.loop) // если линия замкнута
			{
				if(movingTo >= PathElements.Length) //если мы дошли до последней точки
				{
					movingTo = 0;//то надо идти не в обратную сторону, а к первой точке
				}
				else if(movingTo < 0)//если мы дошли до первой точки, двигаясь в обратную сторону
				{
					movingTo = PathElements.Length - 1;// то надо первой точке двинуться к последней
				}
			}
		}
	}
}
