using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskCheckEnemyInFOVRange : Node
{
    [Header("Transform Settings")]
    [SerializeField] private Transform _transform;
    [SerializeField] private float _FOVRange = 8f;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private float _noCheckTime = 5f;
    [SerializeField] private float _noCheckTimer;



    public TaskCheckEnemyInFOVRange(Transform transform, float fovrange, LayerMask enemyLayerMask,  float noCheckTime)
    { 
        _transform = transform;
        _FOVRange = fovrange;
        _enemyLayerMask = enemyLayerMask;
        _noCheckTime = noCheckTime;
    }

    public override NodeState Evaluate()
    {
        if (!AnimalBT.checkForEnemy)
        {
            _noCheckTimer += Time.deltaTime;
            if(_noCheckTimer >= _noCheckTime)
            {
                _noCheckTimer = 0;
                AnimalBT.checkForEnemy = true;
            }

            state = NodeState.FAILURE;
            return state;
        }

        object enemy = GetData("target");
        if (enemy == null)
        {
            Collider[] colliders = Physics.OverlapSphere(
                _transform.position, _FOVRange, _enemyLayerMask);

            if (colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
