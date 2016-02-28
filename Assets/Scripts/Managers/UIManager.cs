using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    // Singleton
    #region Singleton
    static UIManager _manager;
    public static UIManager Get
    {
        get
        {
            if (_manager == null)
                _manager = GameObject.FindObjectOfType<UIManager>();
            return _manager;
        }
    }
    #endregion

    public GameObject playerGameobject;

    public GameObject scoreText;
    private Text _scoreText;

    public GameObject dashImage;
    private Image _dashImage;

    private int score = 0;

    // Use this for initialization
    void Start () {
        // Register events
        PlayerController.OnChargeDashAction += StartDashUI;
        PlayerController.OnReleaseDashAction += EndDashUI;
        PlayerCollisionsController.OnSheepDestroyAction += ChangeScore;

        _dashImage = dashImage.GetComponent<Image>();
        _scoreText = scoreText.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (playerGameobject != null)
        {
            float dashChargeTime = playerGameobject.GetComponent<PlayerController>().dashChargeTime;
            float maxDashChargeAmount = playerGameobject.GetComponent<PlayerController>().maxDashChargeAmount;

            if (dashChargeTime > 0)
            {
                float chargePercent = dashChargeTime / maxDashChargeAmount;
                _dashImage.fillAmount = chargePercent;
            }
        }
	}

    private void ChangeScore()
    {
        score++;
        _scoreText.text = score.ToString();
    }

    private void StartDashUI()
    {

    }

    private void EndDashUI()
    {
        _dashImage.fillAmount = 0;
    }
}
