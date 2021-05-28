using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocomotionController : MonoBehaviour
{
    public virtual void MoveToGoal(string name, float speed)
    {
        print("[LocomotionController] name: " + name + " speed: " + speed);
        Vector3 pos = GameObject.Find(name).transform.position;
        StartCoroutine(Async_MoveTo(pos, speed));
    }

    private IEnumerator Async_MoveTo(Vector3 pos, float speed)
    {
        Vector3 start = transform.position;
        float duration = Vector3.Distance(start, pos) / speed;
        float elapsed = 0f;
        while (transform.position != pos)
        {
            var newPos = Vector3.Lerp(start, pos, elapsed / duration);
            elapsed += Time.deltaTime;
            transform.position = newPos;
            yield return null;
        }
    }
}
