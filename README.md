# SmartEcoLife 🤖💰

Yapay zeka destekli akıllı finans yönetim uygulaması. SmartEcoLife, kullanıcıların finansal durumlarını takip etmelerine, hedefler belirlemelerine ve yapay zeka asistanından kişiselleştirilmiş finansal tavsiyeler almalarına olanak tanır.

## 🎯 Özellikler

### Finansal Yönetim
- **Finansal Kayıt Takibi**: Gelir ve gider kayıtlarınızı kategorilere göre organize edin
- **Hedef Belirleme**: Finansal hedeflerinizi belirleyin ve ilerlemenizi takip edin
- **Finansal Hesaplayıcılar**: Bileşik faiz ve kredi hesaplama araçları
- **Dashboard**: Finansal durumunuza dair özet görünüm

### Yapay Zeka Entegrasyonları
- **Sel AI Chatbot**: Finansal danışman yapay zeka asistanı ile sohbet edin
- **Akıllı Öneriler**: Kişiselleştirilmiş finansal tavsiyeler alın
- **Bağlamsal Analiz**: Finansal kayıtlarınız ve hedeflerinize göre özelleştirilmiş analiz

### Kullanıcı Yönetimi
- Güvenli kayıt ve giriş sistemi
- Profil yönetimi
- Şifre değiştirme
- Hesap silme

## 🏗️ Mimari

Bu proje **Vertical Slice Architecture** prensiplerine göre tasarlanmıştır. Her özellik (slice), kendi klasörü altında tam bir işlevsellik sunar:

```
Features/
├── Users/           # Kullanıcı yönetimi slice'ı
├── FinancialRecords/# Finansal kayıt yönetimi slice'ı
├── Goals/           # Hedef yönetimi slice'ı
├── SelAI/           # Yapay zeka entegrasyonu slice'ı
├── FinancialCalculator/ # Finansal hesaplama araçları slice'ı
├── Categories/      # Kategori yönetimi slice'ı
└── Dashboards/      # Dashboard slice'ı
```

Her slice kendi içinde:
- Entity modellerini
- Servis katmanını
- UI componentlerini (Razor sayfaları)
- İlgili DTO'ları

içerir. Bu mimari sayesinde:
- ✅ Kod organizasyonu ve bakım kolaylığı
- ✅ Özellikler arası düşük bağımlılık
- ✅ Yeni özellik ekleme kolaylığı
- ✅ Test edilebilirlik

## 🛠️ Teknolojiler

### Backend & Framework
- **.NET 10.0**: Son nesil .NET platformu
- **Blazor Server**: Interaktif web uygulaması framework'ü
- **Entity Framework Core 9.0**: ORM ve veritabanı yönetimi
- **PostgreSQL**: İlişkisel veritabanı
- **ASP.NET Core Identity**: Kimlik doğrulama ve yetkilendirme

### Yapay Zeka
- **Microsoft Semantic Kernel 1.66.0**: AI uygulamaları için framework
- **OpenAI API** (OpenRouter üzerinden): Büyük dil modelleri entegrasyonu

### Diğer Kütüphaneler
- **AutoMapper 15.1.0**: Nesne eşleme
- **Blazor.Bootstrap 3.4.0**: UI bileşenleri
- **Memory Cache**: Performans optimizasyonu

## 🤖 AI Entegrasyonları

### Sel AI Chatbot
Kullanıcıların finansal durumlarını analiz eden ve sorularını yanıtlayan akıllı chatbot. Özellikleri:
- Finansal kayıtları ve hedefleri analiz eder
- Bağlamsal sohbet geçmişi tutar
- Kişiselleştirilmiş finansal tavsiyeler sunar
- Memory cache ile performans optimizasyonu

### Akıllı Öneri Sistemi
Kullanıcının finansal durumuna göre otomatik öneriler üreten sistem:
- Son finansal kayıtları analiz eder
- Aktif hedefleri değerlendirir
- Motivasyonel ve kısa tavsiyeler üretir
- 1 saatlik cache ile performans sağlar

### AI Kernel Yapılandırması
Proje, iki ayrı Semantic Kernel kullanır:
- **Recommendation Kernel**: Öneri üretimi için optimize edilmiş
- **Chat Kernel**: Sohbet etkileşimleri için optimize edilmiş

Bu ayrım sayesinde her use case için farklı modeller ve parametreler kullanılabilir.

## 📦 Kurulum

### Gereksinimler
- .NET 10.0 SDK
- PostgreSQL 12+
- Visual Studio 2022 veya VS Code

### Adımlar

1. **Repository'yi klonlayın**
```bash
git clone <repository-url>
cd SmartEcoLife
```

2. **Veritabanını yapılandırın**
   - PostgreSQL'de yeni bir veritabanı oluşturun
   - `appsettings.json` dosyasındaki connection string'i güncelleyin

3. **Migration'ları çalıştırın**
```bash
dotnet ef database update
```

4. **AI API anahtarlarını yapılandırın**
   - `appsettings.json` dosyasında `AI:Recommendation:ApiKey` ve `AI:Chat:ApiKey` değerlerini güncelleyin
   - OpenRouter API anahtarınızı ekleyin veya farklı bir provider kullanın

5. **Uygulamayı çalıştırın**
```bash
dotnet run
```

## ⚙️ Yapılandırma

### appsettings.json

```json
{
  "AI": {
    "Recommendation": {
      "Provider": "https://openrouter.ai/api/v1",
      "Model": "model_name",
      "ApiKey": "your-api-key"
    },
    "Chat": {
      "Provider": "https://openrouter.ai/api/v1",
      "Model": "model_name",
      "ApiKey": "your-api-key"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=SmartEcoLifeDb;Username=postgres;Password=your-password"
  }
}
```

## 📁 Proje Yapısı

```
SmartEcoLife/
├── Data/                    # DbContext ve veritabanı yapılandırması
├── Features/                # Vertical slice'lar
│   ├── Categories/         # Kategori yönetimi
│   ├── Dashboards/         # Dashboard görünümleri
│   ├── FinancialCalculator/# Finansal hesaplama araçları
│   ├── FinancialRecords/   # Finansal kayıt yönetimi
│   ├── Goals/              # Hedef yönetimi
│   ├── SelAI/              # Yapay zeka entegrasyonu
│   └── Users/              # Kullanıcı yönetimi
├── Shared/                  # Paylaşılan bileşenler
│   ├── Dtos/               # Data Transfer Objects
│   ├── Layout/             # Layout bileşenleri
│   ├── MappingProfiles/    # AutoMapper profilleri
│   └── ErrorPages/         # Hata sayfaları
├── Migrations/              # EF Core migration'ları
├── wwwroot/                 # Statik dosyalar
└── Program.cs               # Uygulama giriş noktası
```

## 🚀 Kullanım

1. **Hesap Oluşturma**: Yeni bir kullanıcı hesabı oluşturun
2. **Finansal Kayıt Ekleme**: Gelir ve gider kayıtlarınızı ekleyin
3. **Hedef Belirleme**: Finansal hedeflerinizi tanımlayın
4. **AI Asistanı ile Sohbet**: Sel AI chatbot ile finansal durumunuz hakkında sorular sorun
5. **Önerileri İnceleme**: Dashboard'da AI'dan gelen kişiselleştirilmiş önerileri görün


<img width="1905" height="847" alt="Ekran görüntüsü 2025-11-10 021923" src="https://github.com/user-attachments/assets/8c96e014-3db1-4567-8573-c332afd20fa1" />
<img width="1911" height="853" alt="Ekran görüntüsü 2025-11-10 021749" src="https://github.com/user-attachments/assets/cde62b90-f95b-4194-99b5-bbde1f9b5631" />
<img width="1897" height="858" alt="Ekran görüntüsü 2025-11-10 022055" src="https://github.com/user-attachments/assets/03be19a7-fa38-4473-901a-b6ed4771f63e" />
<img width="1907" height="847" alt="Ekran görüntüsü 2025-11-10 021939" src="https://github.com/user-attachments/assets/82c8e7a4-26b8-4971-a9df-cf2f5d8aac95" />




