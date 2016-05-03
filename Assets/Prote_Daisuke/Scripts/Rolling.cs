using UnityEngine;
using System.Collections;

public class Rolling : MonoBehaviour {
	// Label
	private float xac = 0.0f;
    public GameObject MainCamera;
    private GUIStyle labelStyle;
    private PhysicMaterial physicMaterial;
    private Rigidbody rb;
    public Transform particle;
    public int HP = 4;

    // initial
    void Awake(){
		// frame rate
		Application.targetFrameRate = 60;
    }

	// Use this for initialization
	void Start () {
        this.labelStyle = new GUIStyle();
        this.labelStyle.fontSize = Screen.height / 22;
        this.labelStyle.normal.textColor = Color.red;
        rb = GetComponent<Rigidbody>();
       
  	}
    void FixedUpdate()
    {
        xac = 0.1f * Input.acceleration.x;
        transform.position += new Vector3(xac*1.5f, 0, 0);
        Vector2 pos = transform.position;
        
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        pos.x = Mathf.Clamp(pos.x, min.x + 0.5f, max.x);
        //スマホが傾けられた方向に球を回転
        CheckView();
        BallRotation();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && rb.velocity.y >2)
        {
            
            HP -= 2;
            Debug.Log("耐久度は"+HP);
            Debug.Log(rb.velocity.y);
        }
        if(HP == 0)
        {
            Instantiate(particle,this.transform.position,Quaternion.identity);
            Destroy(gameObject);
            // Application.LoadLevel(0);
           
        }
    }

    void OnGUI() //デバッグの為傾きの値を取得し文字に描画
    {
        float x = Screen.width / 10;
        float y = 0;
        float w = Screen.width * 8 / 10;
        float h = Screen.height / 20;

        string text = string.Empty;
        text = string.Format("x={0}", System.Math.Round(xac,2));
        GUI.Label(new Rect(x, y, w, h), text, this.labelStyle);
    }

    void BallRotation()
    {
        if (xac > 0)
        {
            transform.Rotate(0, 0, -xac * 150);
        }
        else if (xac < 0)
        {

            transform.Rotate(0, 0, -xac * 150);
        }
        else
        {
            transform.Rotate(0, 0, 0);
        }
    }

    void CheckView()
    {
        if (transform.position.x > MainCamera.transform.position.x - 4)
        {
            Vector3 cameraPos = MainCamera.transform.position;
            //カメラの中心位置の設定
            cameraPos.x = transform.position.x + 1;
            MainCamera.transform.position = cameraPos;
        }
        if(transform.position.y > MainCamera.transform.position.y - 2)
        {
            Vector3 cameraPos = MainCamera.transform.position;

            cameraPos.y = transform.position.y;
            MainCamera.transform.position = cameraPos;
        }
    }
}
