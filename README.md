# Proje Kurulum Adımları
Bu doküman, uygulamayı çalıştırmak için gerekli kurulum ve başlatma adımlarını içerir.

## Gerekli Bağımlılıklar

- Docker
- RabbitMQ (Docker üzerinden çalıştırılacak)
- Dapr CLI
- .NET SDK

## 1. RabbitMQ Çalıştırılması
- RabbitMQ'yu Docker ile çalıştırmak için aşağıdaki komutu terminalde çalıştırın:

docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management

- RabbitMQ'nun doğru çalıştığından emin olun. Yönetim paneline erişmek için http://localhost:15672 adresini kullanabilirsiniz.
- Varsayılan olarak kullanıcı adı: guest ve parola: guest

## 2. ApplicaitonMS Dapr Üzrinden Çalıştırılması

dapr run --app-id applicationsidecar --app-port 3700 --dapr-http-port 3710 -- dotnet run

--app-id: Dapr için benzersiz uygulama kimliği.
--app-port: Uygulamanın dinleyeceği port.
--dapr-http-port: Dapr'ın HTTP üzerinden dinleyeceği port.
--dotnet run: Uygulamanın başlatılması.

## 3. PersonMS Dapr Üzrinden Çalıştırılması

dapr run --app-id personsidecar --app-port 3701 --dapr-http-port 3711 -- dotnet run

--app-id: Dapr için benzersiz uygulama kimliği.
--app-port: Uygulamanın dinleyeceği port.
--dapr-http-port: Dapr'ın HTTP üzerinden dinleyeceği port.
--dotnet run: Uygulamanın başlatılması.

Bu dosyayı projenize ekleyerek kolayca kurulum ve çalıştırma yönergelerini yapabilirsiniz.
