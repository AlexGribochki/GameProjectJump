using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;

    // Смещение от центра экрана
    public float offsetX = -1.0f;
    public float offsetY = -3.0f;

    private void Awake()
    {
        if (!player)
            player = FindObjectOfType<Hero>().transform;
    }

    private void Update()
    {
        pos = player.position;
        pos.z = -10f;

        // Применение смещения
        pos.x -= offsetX;
        pos.y -= offsetY;

        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
    }
}

