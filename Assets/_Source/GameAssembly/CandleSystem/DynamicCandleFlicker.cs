using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GameAssembly.CandleSystem
{
    public class DynamicCandleFlicker : MonoBehaviour
    {
        // Компонент Light2D для свечного света
        [SerializeField] private Light2D candleLight;

        // Компонент Rigidbody2D для отслеживания движения персонажа
        [SerializeField] private Rigidbody2D rb;

        // Базовая интенсивность света
        [SerializeField] private float baseIntensity = 1f;

        // Вариация интенсивности: когда персонаж неподвижен
        [SerializeField] private float staticVariance = 0.05f;

        // Вариация интенсивности: когда персонаж движется
        [SerializeField] private float movingVariance = 0.3f;

        // Порог, ниже которого считаем, что персонаж не двигается (например, 0.1f)
        [SerializeField] private float movementThreshold = 0.1f;

        // Фактор резкого гашения при смене направления (от 0 до 1)
        [SerializeField] private float directionChangeIntensityMultiplier = 0.5f;

        // Скорость восстановления интенсивности после смены направления
        [SerializeField] private float recoverySpeed = 2f;

        // Скорость мерцания (множитель для Perlin Noise)
        [SerializeField] private float flickerSpeed = 2f;

        // Градиент для небольшого изменения цвета пламени
        [SerializeField] private Gradient candleColorGradient;

        // Внутреннее смещение для Perlin Noise, чтобы разные свечи не синхронизировались
        private float noiseOffset;

        // Для отслеживания предыдущего направления движения
        private Vector2 previousDirection = Vector2.zero;

        // Текущий множитель, влияющий на интенсивность (при резком повороте временно меньше 1)
        private float currentDirectionMultiplier = 1f;

        private void Awake()
        {
            if (candleLight == null)
                candleLight = GetComponent<Light2D>();

            if (rb == null)
                rb = GetComponent<Rigidbody2D>();

            noiseOffset = Random.Range(0f, 1000f);
        }

        private void Update()
        {
            // Получаем текущую скорость и направление движения
            Vector2 velocity = rb.velocity;
            float speed = velocity.magnitude;

            // Определяем нужную вариацию интенсивности в зависимости от того, движется персонаж или нет
            float variance = (speed < movementThreshold) ? staticVariance : movingVariance;

            // Если персонаж двигается, анализируем смену направления
            if (speed >= movementThreshold)
            {
                Vector2 currentDirection = velocity.normalized;
                // Если предыдущего направления не было или оно почти нулевое, инициализируем его
                if (previousDirection == Vector2.zero)
                    previousDirection = currentDirection;

                // Вычисляем угол между предыдущим и текущим направлением (используя скалярное произведение)
                float dot = Vector2.Dot(previousDirection, currentDirection);
                // Если разница значительная (например, косинус угла меньше 0.7), считаем, что направление резко изменилось
                if (dot < 0.7f)
                {
                    currentDirectionMultiplier = directionChangeIntensityMultiplier;
                }
                // Сохраняем текущее направление для следующего сравнения
                previousDirection = currentDirection;
            }
            else
            {
                // Если персонаж стоит, сбрасываем направление
                previousDirection = Vector2.zero;
            }

            // Восстанавливаем множитель до 1 с течением времени
            currentDirectionMultiplier = Mathf.Lerp(currentDirectionMultiplier, 1f, Time.deltaTime * recoverySpeed);

            // Получаем значение Perlin Noise для плавного случайного колебания
            float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, noiseOffset);

            // Рассчитываем новую интенсивность: базовая интенсивность + смещение от шума и с учетом текущего множителя
            candleLight.intensity = baseIntensity * currentDirectionMultiplier + (noise - 0.5f) * variance;

            // Применяем небольшой градиент цвета, используя значение шума для вариации оттенка
            // Например, если базовый цвет – это середина градиента, то небольшое смещение шума создаст вариацию
            candleLight.color = candleColorGradient.Evaluate(noise);
        }
    }
}
