using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public float FillAmount;
    public Image bar;
    [SerializeField] GameObject player;
    [SerializeField] float MaxValue = 100;
    [SerializeField] float MinValue = 0;

    void Start()
    {
        Health health = player.GetComponent<Health>();
        health.HealthChanged += Health_healthChanged;
    }

    private void Health_healthChanged(string object_name, float new_health)
    {
        FillAmount = new_health;
    }

    // Update is called once per frame
    void Update()
    {
        var fill = Mathf.Clamp(FillAmount / (MaxValue - MinValue), 0, 1);
        bar.transform.localScale = new Vector3(fill, 1, 1);
    }
}
