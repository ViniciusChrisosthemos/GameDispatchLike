using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class DiceController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private List<Transform> _diceFaces;
    [SerializeField] private float _collisionForce = 10f;

    [Header("Events")]
    public UnityEvent<int,int> OnDiceResult;

    private int _diceFaceIndex = -1;
    private bool _hasStoppedRolling;
    private bool _delayFinished;

    private void Update()
    {
        if (!_delayFinished) return;

        if (!_hasStoppedRolling && _rigidbody.velocity.sqrMagnitude == 0)
        {
            _hasStoppedRolling = true;
            GetNumberOnTopFace();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            var vector = transform.position - collision.transform.position;
            Rigidbody.AddForce(vector, ForceMode.Impulse);
        }
        else if (collision.gameObject.TryGetComponent(out DiceController controller))
        {
            var vector = collision.transform.position - transform.position;
            controller.Rigidbody.AddForce(vector, ForceMode.Impulse);
        }
    }

    [ContextMenu("Get Dice Result")]
    private int GetNumberOnTopFace()
    {
        var topFace = 0;
        var maxDist = 0f;

        for (int i = 0; i < _diceFaces.Count; i++)
        {
            if (_diceFaces[i].position.y  > maxDist)
            {
                topFace = i;
                maxDist = _diceFaces[i].position.y;
            }
        }

        Debug.Log($"Dice result {topFace + 1}");

        OnDiceResult?.Invoke(_diceFaceIndex, topFace + 1);

        return topFace + 1;
    }

    public void RollDice(float throwForce, float rollForce, int diceIndex)
    {
        _diceFaceIndex = diceIndex;

        var randomVariance = Random.Range(-1f, 1f);
        _rigidbody.AddForce(transform.forward * (throwForce + randomVariance), ForceMode.Impulse);

        var randX = Random.Range(0f, 1f);
        var randY = Random.Range(0f, 1f);
        var randZ = Random.Range(0f, 1f);

        var torque = new Vector3(randX, randY, randZ);

        _rigidbody.AddTorque(torque * (rollForce + randomVariance), ForceMode.Impulse);

        DelayResult();
    }

    private async void DelayResult()
    {
        await Task.Delay(1000);
        _delayFinished = true;
    }

    public Rigidbody Rigidbody => _rigidbody;
}
