using System.Collections;
using UnityEngine;

public class WebcamTest : MonoBehaviour
{
    private WebCamTexture webcam;
    private int mevcutWebcam; // Cihazda birden çok webcam varsa aralarında geçiş yapmak için kullanacağımız bir kod
    private WebCamDevice[] webcamCihazlari; // Aygıttaki tüm webcam'leri depolayan bir değişken
    private Quaternion anaRotasyon; // Kameranın ekrana yamuk yumuk açılarla yansımasını engellemek için bir değişken

    IEnumerator Start()
    {
        // Webcam'e erişmek için kullanıcıdan izin istememiz gerekiyor mu kontrol et
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            // Kullanıcının izni gerekiyor, izin iste ve bekle
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

            // Kullanıcı izin vermezse script'i objeden kaldır
            if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
                Destroy(this);
        }

        webcamCihazlari = WebCamTexture.devices;

        // Eğer cihazda en az 1 tane webcam varsa, 0. indexteki webcam cihazını "webcam" değişkenine ata; eğer webcam yoksa hata mesajı ver
        if (webcamCihazlari.Length != 0)
        {
            webcam = new WebCamTexture(webcamCihazlari[0].name);
            mevcutWebcam = 0;
        }
        else
        {
            Debug.LogError("Hata! Cihazda webcam bulunamadı.");

            // Hata mesajının ardından bu scripti objeden kaldır
            Destroy(this);
        }

        // Objeye texture olarak webcam görüntüsünü ata
        GetComponent<Renderer>().material.mainTexture = webcam;

        // Webcam'den veri almaya başla (webcam'i çalıştır)
        webcam.Play();

        anaRotasyon = transform.rotation;
    }

    void Update()
    {
        // Her kameranın kendine has bir çekim açısı olabilir. Bu kod, objeyi kameranın çekim açısına göre otomatik olarak döndürüyor (benim de tam anlamadığım bir şekilde)
        transform.rotation = anaRotasyon * Quaternion.AngleAxis(webcam.videoRotationAngle, Vector3.forward);
    }

    void OnGUI()
    {
        // Webcam'i duraklatmak/devam ettirmek ya da durdurmak için ekranın sol üstüne butonlar ekle
        if (webcam.isPlaying)
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "DURAKLAT"))
                webcam.Pause();

            if (GUI.Button(new Rect(10, 70, 100, 50), "DURDUR"))
                webcam.Stop();
        }
        else
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "OYNAT"))
                webcam.Play();
        }

        // Görüntüyü ters çevirmek için butonlar ekle
        if (GUI.Button(new Rect(10, 150, 100, 50), "DİKEY ÇEVİR"))
        {
            Vector3 scale = transform.localScale;
            scale.y = -scale.y;
            transform.localScale = scale;
        }

        if (GUI.Button(new Rect(10, 210, 100, 50), "YATAY ÇEVİR"))
        {
            Vector3 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }

        // Eğer varsa başka kameralara geçmek için buton ekle
        if (webcamCihazlari.Length > 1)
        {
            if (GUI.Button(new Rect(120, 10, 100, 50), "KAMERA DEĞİŞ"))
                WebcamDegistir((mevcutWebcam + 1) % webcamCihazlari.Length);
        }

        // Eğer cihazda ön kamera varsa ilk ön kameraya geçiş yapmayı sağlar
        if (GUI.Button(new Rect(230, 10, 120, 50), "ÖN KAMERA"))
        {
            for (int i = 0; i < webcamCihazlari.Length; i++)
            {
                // Eğer bu webcam öne bakıyorsa mevcut kamerayı buna ayarla
                if (webcamCihazlari[i].isFrontFacing)
                {
                    WebcamDegistir(i);
                    return;
                }
            }
        }

        // Eğer cihazda arka kamera varsa ilk arka kameraya geçiş yapmayı sağlar
        if (GUI.Button(new Rect(360, 10, 120, 50), "ARKA KAMERA"))
        {
            for (var i = 0; i < webcamCihazlari.Length; i++)
            {
                // Eğer bu webcam arkaya bakıyorsa mevcut kamerayı buna ayarla
                if (!webcamCihazlari[i].isFrontFacing)
                {
                    WebcamDegistir(i);
                    return;
                }
            }
        }
    }

    private void WebcamDegistir(int yeniWebcam)
    {
        // Mevcut webcam'den görüntü almayı bırak
        webcam.Stop();

        // Artık kullanmayacağımız webcam görüntüsünü yok ederek hafızayı boşalt
        Destroy(webcam);

        // Yeni webcam'e geçiş yap
        mevcutWebcam = yeniWebcam;
        webcam = new WebCamTexture(webcamCihazlari[mevcutWebcam].name);
        GetComponent<Renderer>().material.mainTexture = webcam;

        // Yeni webcam'i çalıştır
        webcam.Play();
    }
}