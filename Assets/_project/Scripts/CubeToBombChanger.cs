using System;
using UnityEngine;

public class CubeToBombChanger : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private BombSpawner _bombSpawner;

    private void OnEnable() =>
        _cubeSpawner.OnReleasedCube += SpawnBomb;

    private void OnDisable() =>
        _cubeSpawner.OnReleasedCube -= SpawnBomb;

    private void SpawnBomb(Transform target) =>
        _bombSpawner.CreateBomb(target);
}
