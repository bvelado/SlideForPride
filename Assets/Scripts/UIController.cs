using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    static UIController _instance;
    public static UIController Instance
    {
        get
        {
            return _instance;
        }
    }

    public Image cooldownDash;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateDashCooldown(float value)
    {
        cooldownDash.fillAmount = value;
    }
}
