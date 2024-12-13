using System;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent (typeof(AudioSource))]
public class SoundController : GenericMonoSingleton<SoundController>
{
    [SerializeField] AudioClip _BuyingSFX;
    [SerializeField] AudioClip _SellingSFX;

    AudioSource _Source;

    private void Start()
    {
        _Source = GetComponent<AudioSource> ();

        EventService.Instance._OnItemBought.AddListener(PlayBuyingSFX);
        EventService.Instance._OnItemSold.AddListener(PlaySellingSFX);
    }

    private void PlayBuyingSFX(string arg1, int arg2)
    {
        _Source.clip = _BuyingSFX;
        _Source.Play();
    }

    void PlaySellingSFX(string arg1, int arg2)
    {
        _Source.clip = _SellingSFX;
        _Source.Play();
    }

    private void OnDisable()
    {
        EventService.Instance._OnItemBought.RemoveListener(PlayBuyingSFX);
        EventService.Instance._OnItemSold.RemoveListener(PlaySellingSFX);
    }
}
