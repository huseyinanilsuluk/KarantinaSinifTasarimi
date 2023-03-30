using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GirisKodlar : MonoBehaviour
{
   public void Giris_OgretmenGirisSayfasi()
    {
        SceneManager.LoadScene("OgretmenGirisSayfasi");
    }
    public void Giris_OgrenciGirisSayfasi()
    {
        SceneManager.LoadScene("OgrenciGirisSayfasi");
    }
}
