using System.Collections;
using TMPro;
using UnityEngine;

namespace GameAssembly.CandleSystem
{
    public class TestCandleController : MonoBehaviour
    {
        [Header("Настройки случайного гашения")]
        [SerializeField] private bool enableRandomExtinguish = true;
        [SerializeField] private float randomExtinguishIntervalMin = 5f;
        [SerializeField] private float randomExtinguishIntervalMax = 15f;

        [Header("Компоненты свечи")]
        [SerializeField] private UnityEngine.Rendering.Universal.Light2D candleLight;
        [SerializeField] private Animator candleAnimator;

        [Header("UI для разжигания")]
        [SerializeField] private Canvas extinguishCanvas; // Canvas, который появляется при тухлой свече
        [SerializeField] private TMP_Text extinguishPromptText; // Текст с подсказкой

        [Header("Настройки спички")]
        [SerializeField] private ParticleSystem sparkParticles; // Частицы искр
        [SerializeField] private int requiredStrikes = 3; // Сколько раз нужно ударить спичкой
        [SerializeField] private float strikeCooldown = 0.5f; // Задержка между ударами (если нужна)

        private int strikeCount = 0;
        private bool isExtinguished = false;
        private bool isInInteraction = false;

        private void Start()
        {
            if(enableRandomExtinguish)
                StartCoroutine(RandomExtinguishRoutine());
        
            if(extinguishCanvas != null)
                extinguishCanvas.enabled = false;
        }

        // Корутина для случайного гашения свечи
        private IEnumerator RandomExtinguishRoutine()
        {
            while(true)
            {
                float waitTime = Random.Range(randomExtinguishIntervalMin, randomExtinguishIntervalMax);
                yield return new WaitForSeconds(waitTime);
                if(!isExtinguished) // Гасим только если свеча горит
                {
                    ExtinguishCandle();
                }
            }
        }

        // Внешний вызов гашения (например, через триггер)
        public void TriggerExtinguish()
        {
            if(!isExtinguished)
                ExtinguishCandle();
        }

        // Гашение свечи: отключение света, проигрывание анимации и показ UI
        private void ExtinguishCandle()
        {
            isExtinguished = true;

            // Отключаем свет
            if(candleLight != null)
                candleLight.enabled = false;

            // Запускаем анимацию гашения (если настроена)
            if(candleAnimator != null)
                candleAnimator.SetTrigger("Extinguish");

            // Показываем UI-подсказку для взаимодействия
            if(extinguishCanvas != null)
                extinguishCanvas.enabled = true;

            // Сбрасываем счётчик ударов спичкой
            strikeCount = 0;
            if(extinguishPromptText != null)
                extinguishPromptText.text = $"Потрите спичку: {requiredStrikes} ударов осталось";

            isInInteraction = true;
        }

        // Метод, вызываемый при ударе спичкой (например, через событие кнопки или OnMouseDown на объекте спички)
        public void OnMatchStrike()
        {
            if(!isInInteraction)
                return;

            // Запускаем частицы искр
            if(sparkParticles != null)
                sparkParticles.Play();

            strikeCount++;

            // Обновляем UI-подсказку
            if(extinguishPromptText != null)
                extinguishPromptText.text = $"Потрите спичку: {Mathf.Max(0, requiredStrikes - strikeCount)} удар(ов) осталось";

            // Если достигнуто нужное количество ударов, запускаем зажигание свечи
            if(strikeCount >= requiredStrikes)
            {
                StartCoroutine(LightCandleRoutine());
            }
        }

        // Корутина для поджигания свечи
        private IEnumerator LightCandleRoutine()
        {
            // Задержка для синхронизации (например, проигрывание финальной части анимации искр)
            yield return new WaitForSeconds(0.5f);

            // Запускаем анимацию поджигания
            if(candleAnimator != null)
                candleAnimator.SetTrigger("Light");

            // Ждём окончания анимации (примерно 0.5 секунды, можно скорректировать)
            yield return new WaitForSeconds(0.5f);

            // Включаем свет
            if(candleLight != null)
                candleLight.enabled = true;

            // Скрываем UI-подсказку
            if(extinguishCanvas != null)
                extinguishCanvas.enabled = false;

            isExtinguished = false;
            isInInteraction = false;
        }
    }
}
