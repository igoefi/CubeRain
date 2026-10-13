using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ISpawner))]
public class SpawnerView : MonoBehaviour
{
    private const string Text = "Всего заспавнено: #\nВсего создано: #\nСейчас активно: #\n";
    private const char VarPositionChar = '#';
    
    [SerializeField] private TMP_Text _text;

    private ISpawner _spawner;

    private void Awake()
    {
        _spawner = GetComponent<ISpawner>();
        ChangeValues();
    }

    private void OnEnable()
    {
        _spawner.CountChanged += ChangeValues;
    }

    private void OnDisable()
    {
        _spawner.CountChanged -= ChangeValues;
    }

    private void ChangeValues()
    {
        var texts = Text.Split(VarPositionChar);
        _text.text = texts[0] + _spawner.CountSpawnObjects + texts[1] + _spawner.CountCreateObjects + texts[2] +
                     _spawner.CountActiveObjects;
    }
}
