## Resmi Tatil Uygulaması

Ders: Görsel Programlama
Proje Türü: C# Konsol Uygulaması
Amaç: API üzerinden Türkiye’ye ait resmi tatil listesini almak ve kullanıcıya menü üzerinden çeşitli sorgulama işlemleri sunmak.
Hazırlayan: Beyza Çevik

## 1. Projenin Amacı

Bu projenin amacı, belirtilen yıllara ait Türkiye’deki resmi tatil bilgilerini bir API üzerinden çekip, bu verileri bir konsol uygulamasında işlemek ve kullanıcıya çeşitli filtreleme seçenekleri sunmaktır. Uygulama, API’den alınan JSON verisini Holiday sınıfına dönüştürerek hafızada saklar ve kullanıcı menüsünden yapılan isteklere göre gerekli işlemleri gerçekleştirir.

## 2. Kullanılan Teknolojiler

Bu proje tamamen C# Konsol Uygulaması olarak geliştirilmiştir.
Kullanılan temel yapılar:

## C# Programlama Dili

HttpClient ile API üzerinden veri çekme

JSON verisini nesneye dönüştürmek için DataContractJsonSerializer

.NET Framework Konsol Projesi

Döngüler, koşullu ifadeler, liste işlemleri

Herhangi bir harici paket veya NuGet eklentisi kullanılmamıştır.

## 3. API Kaynakları

Uygulama veri kaynağı olarak Nager.Date API’sini kullanır:

https://date.nager.at/api/v3/PublicHolidays/2023/TR
https://date.nager.at/api/v3/PublicHolidays/2024/TR
https://date.nager.at/api/v3/PublicHolidays/2025/TR


Her çağrı, ilgili yılın tatil bilgilerini JSON formatında döndürür.

## 4. Holiday Sınıfı

API’den gelen JSON, aşağıdaki modele dönüştürülmektedir:

class Holiday
{
    public string date { get; set; }
    public string localName { get; set; }
    public string name { get; set; }
    public string countryCode { get; set; }
    public bool fixed { get; set; }
    public bool global { get; set; }
}


Bu sınıf, tatil tarihini, yerel adını, İngilizce adını ve diğer temel bilgileri saklar.

## 5. Uygulamanın Çalışma Mantığı

Uygulama açıldığında belirtilen yıllara ait tatil verileri API’den çekilir.
Her yıl için veri bir defa alındıktan sonra hafızada saklanır ve aynı yıl tekrar istendiğinde API’ye yeni istek gönderilmez.

Kullanıcıya aşağıdaki menü gösterilir:

===== PublicHolidayTracker =====
1. Tatil listesini göster (yıl seçmeli)
2. Tarihe göre tatil ara (gg-aa formatı)
3. İsme göre tatil ara
4. Tüm tatilleri 3 yıl boyunca göster (2023–2025)
5. Çıkış


Kullanıcı menüden bir seçenek seçer ve yapılan seçime göre ilgili metot çalışır.

## 6. Menü Fonksiyonları
1) Tatil listesini göster (yıl seçmeli)

Kullanıcıdan yıl alınır (2023–2025).
Girilen yıla ait tüm tatiller listelenir.

2) Tarihe göre tatil arama

Kullanıcı gg-aa formatında tarih girer.
Üç yıl içindeki tüm tatiller taranarak o gün ile eşleşen tatil varsa gösterilir.

3) İsme göre arama

Kullanıcı bir anahtar kelime girer.
Yerel adında veya İngilizce adında bu kelime geçen tatiller listelenir.

4) Üç yılın tüm tatillerini göster

2023, 2024 ve 2025 yıllarının tamamı sırayla ekrana yazdırılır.

5) Çıkış

Uygulamadan çıkılır.

## 7. Projenin Çalıştırılması

Visual Studio açılır.

Proje Debug modunda başlatılır (Ctrl + F5).

Konsolda menü görüntülenir.

Kullanıcı istediği seçeneği girerek devam eder.

## 8. Sonuç

Bu proje, API tüketimi, JSON veri işleme, konsol tabanlı kullanıcı etkileşimi, metod kullanımı ve C# temel programlama tekniklerinin uygulanmasını amaçlayan örnek bir çalışmadır.

Kullanıcı dostu bir menü yapısı ile resmi tatil sorgulamaları kolayca yapılabilmektedir.
