using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using System.Diagnostics;
//using UnityEngine.Networking;
using UnityEngine.Networking;

public class Ogretmen_Giris : MonoBehaviour
{
    public InputField kullaniciAdi;
    public InputField sifre;
    public GameObject yanlis_Mesaji;

    void Start()
    {
       
    }

    public void giris_Button()
    {
       
        StartCoroutine(girisYap());
    }
    public void geri_Button()
    {
        SceneManager.LoadScene("GirisSayfasi");
    }
    IEnumerator girisYap()
    {
        WWWForm form = new WWWForm();
        form.AddField("unity", "girisYapma");
        form.AddField("kullaniciAdi", kullaniciAdi.text);
        form.AddField("sifre", sifre.text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Unity_DB/ogretmen.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Sorgu sonucu: " + www.downloadHandler.text);
                if (www.downloadHandler.text.Contains("Giris Basarili")){
                    SceneManager.LoadScene("OgretmenSayfasi");
                }
                else
                {
                    yanlis_Mesaji.SetActive(true);
                }
            }
        }
    }

}
