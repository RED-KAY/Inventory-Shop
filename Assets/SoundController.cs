using System;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundController : GenericMonoSingleton<SoundController>
{
    [SerializeField] AudioClip m_BuyingSFX;
    [SerializeField] AudioClip m_SellingSFX;

    AudioSource m_Source;

    private void Start()
    {
        m_Source = GetComponent<AudioSource>();

        EventService.Instance.m_OnItemBought.AddListener(PlayBuyingSFX);
        EventService.Instance.m_OnItemSold.AddListener(PlaySellingSFX);
    }

    private void PlayBuyingSFX(string arg1, int arg2)
    {
        m_Source.clip = m_BuyingSFX;
        m_Source.Play();
    }

    void PlaySellingSFX(string arg1, int arg2)
    {
        m_Source.clip = m_SellingSFX;
        m_Source.Play();
    }

    private void OnDisable()
    {
        EventService.Instance.m_OnItemBought.RemoveListener(PlayBuyingSFX);
        EventService.Instance.m_OnItemSold.RemoveListener(PlaySellingSFX);
    }
}
