using UnityEngine;

public class UImanager : MonoBehaviour
{
    [SerializeField] GameObject PlayerGUI;
    [SerializeField] GameObject DeathScreen;
    [SerializeField] GameObject Player;
    void Start()
    {
        PlayerGUI.SetActive(true);
        DeathScreen.SetActive(false);

        var health = Player.GetComponent<Health>();
        health.Death += Health_death;
    }

    private void Health_death(string obj)
    {
        PlayerGUI.SetActive(false);
        DeathScreen.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
