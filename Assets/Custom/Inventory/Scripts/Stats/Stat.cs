using System;
using UnityEngine;

[Serializable]
public class StatModifier
{
    [SerializeField]
    private Stat m_ImpactedStat = null;
    [SerializeField]
    private float m_ModifierValue = 0.0f;
    [SerializeField]
    private bool m_equipItem = false;
    [SerializeField]
   // private ScriptableObject[] m_ingredients;
   

    public Stat ImpactedStat => m_ImpactedStat;
    public float ModifierValue => m_ModifierValue;
    public bool equipItem => m_equipItem;
    //public ScriptableObject[] ingredients => m_ingredients;

   
}

[CreateAssetMenu(menuName = "ScriptableObjects/Stats/Stat")]
public class Stat : ScriptableObject
{
    [SerializeField]
    private float m_BaseValue;

    public float BaseValue => m_BaseValue;

   /* [SerializeField]
    private bool equip;

    public bool Equip => equip;

    [SerializeField]
    private ScriptableObject[] ingredient;*/
}
