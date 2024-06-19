using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{

    // set waypoints and platform speed in editor
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] public float platformSpeed;
    [SerializeField] private float _checkDistance = 0.05f;
    [SerializeField] GameObject blink;

    private Transform _targetWayPoint;
    private int _currentWaypointIndex;
    private Rigidbody2D platBody;
    private bool stopped;
    
    void Start()
    {
        platBody = GetComponent<Rigidbody2D>();
        _targetWayPoint = _waypoints[0];
        blink.SetActive(false);
        stopped = false;
    }

    void FixedUpdate()
    {
        if (!stopped)
        {
            Vector2 direction = (_targetWayPoint.position - transform.position).normalized;
            Vector2 vel = direction * platformSpeed;
            platBody.velocity = vel;
            if (Vector2.Distance(a:transform.position, b:_targetWayPoint.position) < _checkDistance)
            {
                _targetWayPoint = GetNextWaypoint();
            }
        }
        else
        platBody.velocity = Vector2.zero;
    }

    public Transform GetNextWaypoint()
    {
        if(transform.Find("Player") != null)
        {
            stopped = true;
        }
        StartCoroutine(BlinkNow());
        _currentWaypointIndex++;
        if (_currentWaypointIndex >= _waypoints.Length)
        {
            _currentWaypointIndex = 0;
        }
        return _waypoints[_currentWaypointIndex];
    }
    IEnumerator BlinkNow()
    {
        blink.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        blink.SetActive(false);

    }
}
