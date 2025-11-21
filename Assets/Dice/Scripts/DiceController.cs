using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class DiceController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private List<DiceValueHolder> _diceFaces;
    [SerializeField] private float _collisionForce = 10f;

    public List<AudioClip> _diceRollSFXs;
    public float _diceRollSFXVolume;

    private int _diceID;
    private int _diceFaceIndex = -1;
    private bool _hasStoppedRolling;
    private bool _delayFinished;

    private Action<int, DiceValueSO> _onRollResultCallback;

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

        int randomIndex = Random.Range(0, _diceRollSFXs.Count);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(_diceRollSFXs[randomIndex], _diceRollSFXVolume);
            _diceFaceIndex = Mathf.Min(_diceRollSFXs.Count - 1, _diceFaceIndex + 1);
        }
    }

    [ContextMenu("Get Dice Result")]
    private int GetNumberOnTopFace()
    {
        var topFace = 0;
        var maxDist = float.MinValue;

        for (int i = 0; i < _diceFaces.Count; i++)
        {
            if (_diceFaces[i].transform.position.y  > maxDist)
            {
                topFace = i;
                maxDist = _diceFaces[i].transform.position.y;
            }
        }

        _onRollResultCallback?.Invoke(_diceID, _diceFaces[topFace].DiceValue);

        return topFace + 1;
    }

    public void RollDice(float throwForce, float rollForce, int diceIndex, Action<int, DiceValueSO> onRollFinished)
    {
        _diceID = diceIndex;
        _onRollResultCallback = onRollFinished;

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
