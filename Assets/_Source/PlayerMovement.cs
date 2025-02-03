using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Скорость движения игрока, настраивается в инспекторе
    [SerializeField] private float speed = 5f;

    // Ссылка на Rigidbody2D компонента игрока
    [SerializeField] private Rigidbody2D rb;

    // Вектор для хранения направления движения
    private Vector2 movement;

    // Получаем компонент Rigidbody2D при запуске
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // В Update получаем входные данные от игрока
    private void Update()
    {
        // Получаем оси для горизонтального (A/D или стрелки влево/вправо) и вертикального (W/S или стрелки вверх/вниз) ввода
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Нормализуем вектор, чтобы диагональное движение не было быстрее по сравнению с движением по осям
        movement = movement.normalized;
    }

    // Физические расчёты выполняем в FixedUpdate
    private void FixedUpdate()
    {
        // Перемещаем игрока с учётом заданной скорости и времени фиксированного обновления
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
