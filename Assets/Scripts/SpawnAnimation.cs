using UnityEngine;

public class SpawnAnimation : MonoBehaviour
{
    private readonly float _duration = 0.5f;
    private float _time;

    private void Start()
    {
        Animate(0);
    }

    private void FixedUpdate()
    {
        _time += Time.deltaTime;
        if (_time > _duration)
        {
            Animate(1);
            Destroy(this);
        }

        Animate(_time / _duration);
    }

    private float Easing(float x)
    {
        return Mathf.Sin(x * Mathf.PI / 2);
    }

    private void Animate(float value)
    {
        var v = Easing(value);
        var y = v - 1;
        var pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }
}