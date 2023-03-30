using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ogrenci_Giris : MonoBehaviour
{
    public InputField kullaniciAdi;
    public InputField sifre;
    public GameObject yanlis_Mesaji;

    public void giris_Button()
    {
        if (kullaniciAdi.text == "161180053" & sifre.text == "12345")
        {
            SceneManager.LoadScene("OgrenciSayfasi");
        }
        else
        {
            yanlis_Mesaji.SetActive(true);
        }
    }
    public void geri_Button()
    {
        SceneManager.LoadScene("GirisSayfasi");
    }
}
