using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceThrower : MonoBehaviour
{
    [SerializeField] private DiceController _dicePrefab;
    [SerializeField] private int _diceAmount = 2;
    [SerializeField] private float _throwForce = 5f;
    [SerializeField] private float _roolForce = 10f;
    [SerializeField] private int _daleyInMilliseconds = 200;
    [SerializeField] private float _throwerAngleRange = 5f;
    [SerializeField] private float _timeScale = 2f;

    public AudioClip _shakingDicesSFX;
    public float _volume = 0.6f;

    private List<GameObject> _diceInstances = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            RollDices();
        }
    }

    private async void RollDices()
    {
        _diceInstances.ForEach(d => Destroy(d));
        _diceInstances.Clear();

        var angle = 0f;
        var oldRotation = transform.rotation;

        Time.timeScale = _timeScale;

        SoundManager.Instance.PlaySFX(_shakingDicesSFX, _volume);

        for (int i = 0; i < _diceAmount; i++)
        {
            angle = Random.Range(-_throwerAngleRange, _throwerAngleRange);
            
            transform.rotation = oldRotation;
            transform.Rotate(Vector3.up, angle);

            var dice = Instantiate(_dicePrefab, transform.position, transform.rotation);
            _diceInstances.Add(dice.gameObject);

            dice.RollDice(_throwForce, _roolForce, i);

            await Task.Delay(_daleyInMilliseconds);

            await Task.Yield();
        }


        Time.timeScale = 1f;
    }
}
