using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinDisplay : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text = null;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCoins(int cnt)
    {
        text.text = cnt.ToString();
    }
}
