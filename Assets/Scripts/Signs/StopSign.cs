using UnityEngine;

public class StopSign : MonoBehaviour
{
    public SignManager sign;

    public GameObject stopCollider;
    public GameObject pots;

    private void Start()
    {
        sign.OnFlip += OnFlipped;
        sign.OnRuined += OnRuined;
        stopCollider.SetActive(true);
        pots.SetActive(false);
    }

    void OnRuined()
    {
        stopCollider.SetActive(false);
        pots.SetActive(false);
    }

    void OnFlipped(bool flipped)
    {
        stopCollider.SetActive(!flipped);
        pots.SetActive(flipped);
    }
}
