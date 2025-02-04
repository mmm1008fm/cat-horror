using UnityEngine;
using DG.Tweening;

public class MatchDrag : MonoBehaviour
{
    [Header("Настройки спички")]
    // Для работы с UI-элементом или объектом на сцене (если спичка – RectTransform, например, на Canvas)
    [SerializeField] private RectTransform matchRect;
    // Минимальное расстояние по оси Y (в пикселях или единицах экрана) для успешного удара
    [SerializeField] private float requiredDragDistance = 100f;
    // Продолжительность анимации перемещения спички
    [SerializeField] private float animationDuration = 0.5f;
    
    [Header("Целевая область")]
    // Объект, к которому должна переместиться спичка при успешном ударе (например, позиция на коробке)
    [SerializeField] private Transform targetArea;

    [Header("Эффекты")]
    // Частицы искр, которые проигрываются при удачном ударе
    [SerializeField] private ParticleSystem sparkParticles;

    // Исходная позиция спички (запоминается при старте)
    private Vector3 initialPosition;
    // Позиция мыши при начале перетаскивания
    private Vector3 dragStartPosition;
    // Флаг, что спичка в процессе перетаскивания
    private bool isDragging = false;

    private void Start()
    {
        // Запоминаем исходное положение спички (в мировых координатах)
        if (matchRect != null)
            initialPosition = matchRect.position;
        else
            initialPosition = transform.position;
    }

    // Вызывается при нажатии ЛКМ на коллайдере объекта
    private void OnMouseDown()
    {
        isDragging = true;
        dragStartPosition = Input.mousePosition;
    }

    // Вызывается при удержании ЛКМ и движении мыши
    private void OnMouseDrag()
    {
        if (!isDragging)
            return;

        // Обновляем позицию спички по положению мыши
        Vector3 currentMousePos = Input.mousePosition;
        if (matchRect != null)
        {
            matchRect.position = currentMousePos;
        }
        else
        {
            // Если объект не UI, можно преобразовать позицию из экрана в мировую
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(currentMousePos);
            worldPos.z = transform.position.z;
            transform.position = worldPos;
        }
    }

    // Вызывается при отпускании ЛКМ
    private void OnMouseUp()
    {
        isDragging = false;

        Vector3 dragEndPosition = Input.mousePosition;
        float dragDeltaY = dragStartPosition.y - dragEndPosition.y; // вычисляем перемещение вниз

        // Проверяем, что движение было направлено вниз и достаточно длинное
        if (dragDeltaY >= requiredDragDistance)
        {
            // Успешное движение – запускаем анимацию DoTween для перемещения спички в целевую область
            ProcessSuccessfulDrag();
        }
        else
        {
            // Если движение не удовлетворяет условию – возвращаем спичку в исходное положение
            ReturnMatchToInitial();
        }
    }

    // Анимация успешного удара
    private void ProcessSuccessfulDrag()
    {
        // Запускаем частиц искр
        if (sparkParticles != null)
            sparkParticles.Play();

        // Анимация перемещения спички в позицию целевой области
        if (matchRect != null)
        {
            matchRect.DOMove(targetArea.position, animationDuration)
                     .SetEase(Ease.OutBack)
                     .OnComplete(() =>
                     {
                         // Здесь можно вызвать метод, инициирующий анимацию поджигания свечи
                         Debug.Log("Спичка успешно проведена по целевой области!");
                         // Пример: CandleController.Instance.IgniteCandle();
                     });
        }
        else
        {
            transform.DOMove(targetArea.position, animationDuration)
                     .SetEase(Ease.OutBack)
                     .OnComplete(() =>
                     {
                         Debug.Log("Спичка успешно проведена по целевой области!");
                     });
        }
    }

    // Анимация возврата спички в исходное положение
    private void ReturnMatchToInitial()
    {
        if (matchRect != null)
        {
            matchRect.DOMove(initialPosition, animationDuration).SetEase(Ease.InOutQuad);
        }
        else
        {
            transform.DOMove(initialPosition, animationDuration).SetEase(Ease.InOutQuad);
        }
    }
}
