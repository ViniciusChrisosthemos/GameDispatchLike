using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceThrower : MonoBehaviour
{
    [SerializeField] private DiceController _dicePrefab;
    [SerializeField] private float _throwForce = 5f;
    [SerializeField] private float _rollForce = 2f;
    [SerializeField] private int _daleyInMilliseconds = 200;
    [SerializeField] private float _throwerAngleRange = 5f;
    [SerializeField] private float _timeScale = 2f;

    [Header("SFX")]
    [SerializeField] private AudioClip _shakingDicesSFX;
    [SerializeField] private float _volume = 0.6f;

    private List<GameObject> _diceInstances = new List<GameObject>();

    public async void RollDices(int diceAmount, Transform parent, Action<int, DiceValueSO> diceCallback)
    {
        _diceInstances.ForEach(d => Destroy(d));
        _diceInstances.Clear();

        var angle = 0f;
        var oldRotation = transform.rotation;

        Time.timeScale = _timeScale;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(_shakingDicesSFX, _volume);
        }

        for (int i = 0; i < diceAmount; i++)
        {
            angle = Random.Range(-_throwerAngleRange, _throwerAngleRange);
            
            transform.rotation = oldRotation;
            transform.Rotate(Vector3.up, angle);

            var dice = Instantiate(_dicePrefab, transform.position, transform.rotation);
            _diceInstances.Add(dice.gameObject);

            dice.RollDice(_throwForce, _rollForce, i, diceCallback);

            await Task.Delay(_daleyInMilliseconds);

            await Task.Yield();
        }


        Time.timeScale = 1f;
    }
}
