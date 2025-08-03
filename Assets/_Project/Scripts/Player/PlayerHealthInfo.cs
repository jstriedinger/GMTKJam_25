using UnityEngine;
using TMPro;

public class PlayerHealthInfo : MonoBehaviour
{
    [SerializeField] private HealthPlayer health;
    [SerializeField] private TMP_Text healthText;

    // Update is called once per frame
    void Update()
    {
        healthText.text = string.Format("Health:{0}", health.totalHealth);
    }
}
