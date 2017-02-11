using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Cameraオブジェクトに張り付けて使って下さい.
/// ……別にカメラ取得するなら、何に張り付けてもいいんですけどね.（ﾎﾞｿｯ
/// </summary>
public class CookieClicker : MonoBehaviour {

    public GameObject addTextObj;
    public float clickrate = 0.5f;

    int click = 0;
    Text countText, messageText;
    GameObject cookie;
    GameObject canvas;

	void Start () {
        click = 0;//デバッグ用.
        //click = PlayerPrefs.GetInt("ClickCount");
        countText = GameObject.Find("CountText").GetComponent<Text>();
        cookie = GameObject.Find("Cookie");
        canvas = GameObject.Find("Canvas");
        messageText = GameObject.Find("MessageText").GetComponent<Text>();
        countText.text = "Count:"+click;
	}
	
	void Update () {
        if (Input.GetMouseButtonUp(0)) {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if (hit) {
                //Debug.Log("Hit!" + hit.transform.gameObject.name);
                if (hit.transform.tag == "Cookie") {//tagがクッキーのオブジェクトをクリックしたとき.
                    Click();
                }
            }
        }

        //クッキーの数でメッセージが変化(定番ですね).
        if (click > 50) {
            messageText.text = "あなたのクッキーは異世界でも評判がいい。";
        }
        else if (click > 40) {
            messageText.text = "この世界であなたのクッキーを知らない生物はいない。";
        }
        else if (click > 30) {
            messageText.text = "あなたのクッキーは今や空前の大ブームとなっている。";
        }
        else if (click > 20) {
            messageText.text = "あなたのクッキーは町の人が並んで買うほど人気がある。";
        }
        else if (click > 10) {
            messageText.text = "あなたのクッキーを家族が食べてくれた。";
        }
        else if (click <= 10) {
            messageText.text = "あなたのクッキーからは生ごみの味がする。";
        }
	}

    /// <summary>
    /// クリック処理.
    /// </summary>
    void Click() {
        click++;
        GameObject obj = Instantiate(addTextObj);
        obj.transform.position = Input.mousePosition;//objの座標をマウス位置にする.
        obj.transform.SetParent(canvas.transform);//親をCanvasに指定する.
        obj.AddComponent<ObjectDestroy>();//ObjectDestroyコンポーネントをobjに加える.
        countText.text = "Count:" + click;//クリックUI更新.
        StartCoroutine("ClickAnim");//"ClickAnim"のコルーチンを呼ぶ.
    }

    /// <summary>
    /// クッキーが微妙に大きくなるような演出をするメソッド.
    /// </summary>
    /// <returns></returns>
    IEnumerator ClickAnim() {
        float time = 0;//アニメーションの時間を記録.
        float scale = 1;//クッキーサイズ.
        while (time < clickrate/2) {//アニメーションの半分の時間を拡大に使う.
            time += Time.fixedDeltaTime;//時間をフレームの増分足す.
            scale = 1 + time/clickrate/8;
            cookie.transform.localScale = new Vector2(scale,scale);//クッキーのサイズを変える.
            yield return new WaitForSeconds(Time.deltaTime);//フレームの終わりまで待つ.
        }
        //上の逆バージョン（今度は縮小処理）.
        while (time < clickrate/2) {
            time += Time.fixedDeltaTime;
            scale -= time/clickrate/8;
            cookie.transform.localScale = new Vector2(scale,scale);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        cookie.transform.localScale = new Vector2(1,1);//スケールに誤差があるかもしれないので、ここで(1,1)に戻す.
    }

    /// <summary>
    /// アプリケーション終了時に呼ばれる.
    /// </summary>
    private void OnApplicationQuit() {
        PlayerPrefs.SetInt("ClickCount", click);
    }
}

/// <summary>
/// クッキーをクリックしたときの『+1』を
/// 時間が経つと消してくれるクラス.
/// </summary>
public class ObjectDestroy : MonoBehaviour{

    float destroyTime = 0.5f;
    float timer;

    private void Update() {
        timer += Time.deltaTime;
        if (destroyTime <= timer) {
            Destroy(gameObject);
        }
    }
}
