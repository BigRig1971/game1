using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class StatBar : MonoBehaviour
{
    private static StatBar _instance;

    public static StatBar Instance { get { return _instance; } }


    private void Awake()
    {
      /*  if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else*/
        {
            _instance = this;
        }
    }
    public float value;
    public float maxValue;
	[SerializeField] private Image image;

	public void UpdateStat(float amount)
	{
		value += amount;
		image.fillAmount = value / maxValue;
	}
}
