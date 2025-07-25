using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomDropSystem : GenericMonoSingleton<RandomDropSystem>
{
    public List<ItemEntry> m_AllItems { get { return GameController.Instance.ItemsList; } }

    [Header("Rarity Weights")]
    public float m_VeryCommonWeight = 50f;
    public float m_CommonWeight = 30f;
    public float m_RareWeight = 15f;
    public float m_EpicWeight = 4f;
    public float m_LegendaryWeight = 1f;

    private Dictionary<Rarity, float> m_RarityWeights;
    private Dictionary<Rarity, List<ItemEntry>> m_ItemsByRarity;

    [SerializeField] TextMeshProUGUI[] m_ItemNames;
    [SerializeField] TextMeshProUGUI[] m_Rarities;
    [SerializeField] TextMeshProUGUI[] m_Statues;

    [SerializeField] GameObject m_Popup;
    [SerializeField] int m_DropCount = 5;

    List<ItemEntry> m_DroppedItems;

    public void Initiliaze()
    {
        m_DroppedItems = new List<ItemEntry>(m_DropCount);
        m_RarityWeights = new Dictionary<Rarity, float>
        {
            { Rarity.VeryCommon, m_VeryCommonWeight },
            { Rarity.Common, m_CommonWeight },
            { Rarity.Rare, m_RareWeight },
            { Rarity.Epic, m_EpicWeight },
            { Rarity.Legendary, m_LegendaryWeight }
        };

        m_ItemsByRarity = new Dictionary<Rarity, List<ItemEntry>>();
        foreach (Rarity r in System.Enum.GetValues(typeof(Rarity)))
        {
            m_ItemsByRarity[r] = new List<ItemEntry>();
        }

        foreach (var item in m_AllItems)
        {
            m_ItemsByRarity[item.m_Rarity].Add(item);
        }
    }

    public List<ItemEntry> GetRandomDrop(int count)
    {
        List<ItemEntry> result = new List<ItemEntry>();
        for (int i = 0; i < count; i++)
        {
            result.Add(GetRandomItem());
        }
        return result;
    }

    private ItemEntry GetRandomItem()
    {
        float totalWeight = 0f;
        foreach (var kvp in m_RarityWeights)
        {
            if (m_ItemsByRarity[kvp.Key].Count > 0)
            {
                totalWeight += kvp.Value;
            }
        }

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;
        Rarity chosenRarity = Rarity.VeryCommon; 

        foreach (var kvp in m_RarityWeights)
        {
            if (m_ItemsByRarity[kvp.Key].Count == 0) continue; 

            cumulative += kvp.Value;
            if (roll < cumulative)
            {
                chosenRarity = kvp.Key;
                break;
            }
        }

        List<ItemEntry> chosenRarityItems = m_ItemsByRarity[chosenRarity];
        int randomIndex = Random.Range(0, chosenRarityItems.Count);
        return chosenRarityItems[randomIndex];
    }

    public void OnRandomDropClicked()
    {
        List<ItemEntry> drop = GetRandomDrop(m_DropCount);
        m_DroppedItems = drop;
        foreach (var droppedItem in drop)
        {
            Debug.Log("Dropped: " + droppedItem.m_Name + " (" + droppedItem.m_Rarity + ")");
        }

        m_Popup.SetActive(true);

        Dictionary<string, int> itemsToAdd = new Dictionary<string, int>();
        
        for (int i = 0; i < m_DroppedItems.Count; i++)
        {
            string k = m_DroppedItems[i].m_Id;
            int v = 1;
            if (itemsToAdd.ContainsKey(k))
            {
                itemsToAdd[k] += v;
            }
            else
            {
                itemsToAdd.Add(k, v);
            }
        }

        ItemsAddInfo[] info = new ItemsAddInfo[itemsToAdd.Count];
        int index = 0;
        foreach (var item in itemsToAdd)
        {
            info[index].m_Id = item.Key;
            info[index].m_Quantity = item.Value;
            index++;
        }

        ItemsAddInfoResult[] itemsAddInfoResults = EventService.Instance.m_TryAddItems?.Invoke(info);
        int flag = 0;
        for (int i = 0; i < itemsAddInfoResults.Length; i++)
        {
            ItemsAddInfoResult result = itemsAddInfoResults[i];
            for (int j = 0; j < result.m_Quantity; j++) {
                m_ItemNames[flag].text = GameController.Instance.AllItems[result.m_Id].m_Name;
                m_Rarities[flag].text = GameController.Instance.AllItems[result.m_Id].m_Rarity.ToString();
                m_Statues[flag].text = result.m_ItemsAdded > 0 ? "added to Inventory" : "cannot be added";
                result.m_ItemsAdded--;
                flag++;
            }

        }
    }

    public void Okay()
    {
        m_Popup.SetActive(false);
    }
}

