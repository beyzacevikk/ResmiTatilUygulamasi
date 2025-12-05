using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

[DataContract]
class Holiday
{
    [DataMember(Name = "date")]
    public string date { get; set; }

    [DataMember(Name = "localName")]
    public string localName { get; set; }

    [DataMember(Name = "name")]
    public string name { get; set; }

    [DataMember(Name = "countryCode")]
    public string countryCode { get; set; }

    [DataMember(Name = "fixed")]
    public bool @fixed { get; set; }

    [DataMember(Name = "global")]
    public bool global { get; set; }
}

class Program
{
    // API istekleri için tek bir HttpClient
    static HttpClient istemci = new HttpClient();

    // Her yıl için tatilleri hafızada tutmak
    static Dictionary<int, List<Holiday>> tatilCache = new Dictionary<int, List<Holiday>>();

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            MenuyuYazdir();

            Console.Write("Seçiminiz: ");
            string secim = Console.ReadLine();
            Console.WriteLine();

            switch (secim)
            {
                case "1":
                    TatilListesiYilSecmeli();
                    break;
                case "2":
                    TariheGoreTatilAra();
                    break;
                case "3":
                    İsmeGoreTatilAra();
                    break;
                case "4":
                    UcYilinHepsiniGoster();
                    break;
                case "5":
                    Console.WriteLine("Programdan çıkılıyor...");
                    return;
                default:
                    Console.WriteLine("Geçersiz seçim.");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Menüye dönmek için bir tuşa basın...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    static void MenuyuYazdir()
    {
        Console.WriteLine("===== PublicHolidayTracker =====");
        Console.WriteLine("1. Tatil listesini göster (yıl seçmeli)");
        Console.WriteLine("2. Tarihe göre tatil ara (gg-aa formatı)");
        Console.WriteLine("3. İsme göre tatil ara");
        Console.WriteLine("4. Tüm tatilleri 3 yıl boyunca göster (2023–2025)");
        Console.WriteLine("5. Çıkış");
        Console.WriteLine();
    }

    // API'den belirtilen yıl için tatil listesini al ve cache'e koy
    static List<Holiday> YilinTatilleriniGetir(int yil)
    {
        if (tatilCache.ContainsKey(yil))
            return tatilCache[yil];

        string url = $"https://date.nager.at/api/v3/PublicHolidays/{yil}/TR";

        // HttpClient ile JSON'ı çek
        var cevap = istemci.GetAsync(url).Result;
        cevap.EnsureSuccessStatusCode();
        string json = cevap.Content.ReadAsStringAsync().Result;

        // DataContractJsonSerializer ile JSON -> List<Holiday>
        var serializer = new DataContractJsonSerializer(typeof(List<Holiday>));
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
        {
            var liste = (List<Holiday>)serializer.ReadObject(ms);
            tatilCache[yil] = liste;
            return liste;
        }
    }

    // 1) Tatil listesini göster (yıl seçmeli)
    static void TatilListesiYilSecmeli()
    {
        Console.Write("Yıl giriniz (2023–2025): ");
        string giris = Console.ReadLine();

        int yil;
        if (!int.TryParse(giris, out yil) || yil < 2023 || yil > 2025)
        {
            Console.WriteLine("Geçerli bir yıl giriniz.");
            return;
        }

        var liste = YilinTatilleriniGetir(yil);

        Console.WriteLine($"=== {yil} Resmi Tatilleri ===");
        foreach (var t in liste)
        {
            Console.WriteLine($"{t.date} | {t.localName} ({t.name})");
        }
    }

    // 2) Tarihe göre tatil ara (gg-aa)
    static void TariheGoreTatilAra()
    {
        Console.Write("Tarih (gg-aa): ");
        string aranan = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(aranan) || aranan.Length != 5 || aranan[2] != '-')
        {
            Console.WriteLine("Format hatalı. Örnek: 23-04");
            return;
        }

        int[] yillar = { 2023, 2024, 2025 };
        bool bulundu = false;

        foreach (int yil in yillar)
        {
            var liste = YilinTatilleriniGetir(yil);

            foreach (var t in liste)
            {
                DateTime dt;
                if (DateTime.TryParse(t.date, out dt))
                {
                    string gunAy = dt.ToString("dd-MM");
                    if (gunAy == aranan)
                    {
                        if (!bulundu)
                        {
                            Console.WriteLine($"{aranan} tarihine denk gelen tatiller:");
                            bulundu = true;
                        }

                        Console.WriteLine($"{dt:dd-MM-yyyy} | {t.localName} ({t.name}) | Yıl: {yil}");
                    }
                }
            }
        }

        if (!bulundu)
            Console.WriteLine("Bu tarihe denk gelen tatil bulunamadı.");
    }

    // 3) İsme göre tatil ara
    static void İsmeGoreTatilAra()
    {
        Console.Write("Aranacak kelime: ");
        string kelime = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(kelime))
        {
            Console.WriteLine("Boş arama yapılamaz.");
            return;
        }

        kelime = kelime.ToLower();
        int[] yillar = { 2023, 2024, 2025 };
        bool bulundu = false;

        foreach (int yil in yillar)
        {
            var liste = YilinTatilleriniGetir(yil);

            foreach (var t in liste)
            {
                string yerel = (t.localName ?? "").ToLower();
                string ing = (t.name ?? "").ToLower();

                if (yerel.Contains(kelime) || ing.Contains(kelime))
                {
                    if (!bulundu)
                    {
                        Console.WriteLine($"\"{kelime}\" içeren tatiller:");
                        bulundu = true;
                    }

                    Console.WriteLine($"{t.date} | {t.localName} ({t.name}) | Yıl: {yil}");
                }
            }
        }

        if (!bulundu)
            Console.WriteLine("Eşleşen tatil bulunamadı.");
    }

    // 4) 3 yılın hepsini göster
    static void UcYilinHepsiniGoster()
    {
        int[] yillar = { 2023, 2024, 2025 };

        foreach (int yil in yillar)
        {
            var liste = YilinTatilleriniGetir(yil);

            Console.WriteLine();
            Console.WriteLine($"=== {yil} Resmi Tatilleri ===");

            foreach (var t in liste)
            {
                Console.WriteLine($"{t.date} | {t.localName} ({t.name})");
            }
        }
    }
}
