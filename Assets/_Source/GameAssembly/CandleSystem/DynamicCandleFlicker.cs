using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace GameAssembly.CandleSystem
{
  public class DynamicCandleFlicker : MonoBehaviour
  {
    [SerializeField] private Light2D candleLight;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float baseIntensity = 1f;
    [SerializeField] private float staticVariance = 0.05f;
    [SerializeField] private float movingVariance = 0.3f;
    [SerializeField] private float movementThreshold = 0.1f;
    [SerializeField] private float directionChangeIntensityMultiplier = 0.5f;
    [SerializeField] private float recoverySpeed = 2f;
    [SerializeField] private float flickerSpeed = 2f; //perlin noise multiplier
    [SerializeField] private Gradient candleColorGradient;
    private float _noiseOffset;
    private Vector2 _previousMoveDirection = Vector2.zero;
    private float _currentDirectionMultiplier = 1f; //temporary less tha 1 on a sharp turn
    private const float SharpTurnCosThreshold = .7f;

    public bool IsFlickering { get; private set; }

    private void Awake()
    {
      if (candleLight == null)
        candleLight = GetComponent<Light2D>();

      if (playerRb == null)
        playerRb = GetComponent<Rigidbody2D>();

      _noiseOffset = Random.Range(0f, 1000f);
    }

    private void Update()
    {
      PerformFlicker();
#if UNITY_EDITOR
      DebugTurnOff();
#endif
    }

    public void TurnOn(bool enable)
    {
      IsFlickering = enable;
      candleLight.intensity = enable ? baseIntensity : 0;
    }

    private void PerformFlicker()
    {
      if (IsFlickering)
      {
        Vector2 velocity = playerRb.velocity;
        float speed = velocity.magnitude;
        float variance = (speed < movementThreshold) ? staticVariance : movingVariance;

        if (speed >= movementThreshold)
        {
          Vector2 currentDirection = velocity.normalized;
          if (_previousMoveDirection == Vector2.zero)
            _previousMoveDirection = currentDirection;

          float dot = Vector2.Dot(_previousMoveDirection, currentDirection);
          if (dot < SharpTurnCosThreshold)
          {
            _currentDirectionMultiplier = directionChangeIntensityMultiplier;
          }

          _previousMoveDirection = currentDirection;
        }
        else
        {
          _previousMoveDirection = Vector2.zero;
        }

        _currentDirectionMultiplier = Mathf.Lerp(_currentDirectionMultiplier, 1f, Time.deltaTime * recoverySpeed);

        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, _noiseOffset);
        candleLight.intensity = baseIntensity * _currentDirectionMultiplier + (noise - 0.5f) * variance;
        candleLight.color = candleColorGradient.Evaluate(noise);
      }
    }

    private void DebugTurnOff()
    {
      if (Input.GetKeyDown(KeyCode.P))
        TurnOn(false);
    }
  }
}