# SensorMonitoringSystem2023
Continued project with adding new modules, new functionalities, new API Methods, new sql entities and fixing some visual bugs.

Servis-İstemci çalışma koşulları:
Kayıtlı sensörler verileri otomatik olarak veritabanına yazıcak(5-10 saniyede bir)
yada sensör verileri yetkisi olan kişiler tarafından excel'den eklenebilecek.
Her şirkete bağlı bir system admin olcak ve kullanıcı türleri olup yetkileri aşağıdaki gibi olucak:
System Admin(Her sensörü, her userı, her veriyi görebilecek ekleyebilecek)
Company Owner(Sensör hepsini görebilcek, approve edebilcek, sensör ve verisini ekleyebilecek)
Technical User(Sensör hepsini görebilcek, sensör ve verisini ekleyebilecek)
Sensor Type Admin(Belirli sensör görebilcek,sensör ekleyemeyecek,sensör silemeyecek, o sensöre veri ekleyebilcek)
Standart User(Sensör hepsini görebilcek,sensör ekleyemeyecek,sensör silemeyecek,veri ekleyemeyecek), 
HR User ise (approve edebilcek)
1 Yıl öncesine ait sensör verileri ve aktif edilmemiş userlar günlük olarak silinecek.
Kayıtlı sensörün değeri kritik sınırları aştığında yetkili kişiler mail yolu ile bilgilendirilecek.


2017 versiyonuna uygulanan değişikliklerin tümü aşağıdaki gibidir:

Servis Web Config Connection String Sunucu adı giriş username şifre güncellendi.
Logo Düzenlendi(Telif Hakkı, güzel olmadığı ve şirket logosu bulunmaması sebebiyle zamanındaki iş ortağı adı ve şirket adı kullanıldı)
Name alanı <!-- Regex tek isim veya iki isim arada boşluklu ve Türkçe karakter alacak şekilde güncellendi.-->
Soyadı alanı <!-- Türkçe karakter alacak şekilde güncellendi.-->
Address <!-- Türkçe karakter alacak şekilde güncellendi.-->
Register Sayfası Bilgiler Alt alta gelecek şekilde güncellendi.
Register Sayfası Unicode Character görüntüleme düzeltildi.(web client çağırımlarına header charset UTF-8 eklendi)
DB Ülkeler,Şehirler,İlçeler eklendi ve register sayfasında adres belirlemede kullanıldı.
Profanity Kelimeler DB Eklendi ve adres tanımında argo girişi engellendi.
INSTEAD OF INSERT UPDATE trigger ve User tablosu Password Hash+Salt DB'ye eklendi
Login SP DB'ye eklendi ve serviste, clientta kullanıldı.
Ülkeler,Şehirler,İlçeler,DirtyWords ve UserLogin Get Operation Contract Olarak API'ye eklendi
Post Methodların Result Int Değeri döndürülmesi sağlandı(0 & 1 = savechanges())
Sensör Ekleme ve Sensör datası ekleme methodları API'ye eklendi.
Page içerisine loading div css içerisine loading eklendi form onsubmit methoduna js function eklendi wait screen oluşturuldu.
Mail'e ait Adres,şifre,port,ssl,host servis webconfig içerisine eklendi.
MailTemplateHtml oluşturuldu.
User ve Userdetail tabloları güncellendi(telefon detail'e alındı ülke şehir ilçe detail eklendi, user is approved kolonu eklendi DB).
User ve Userdetail tablolarına göre serviste contractlarda güncelleme yapıldı istemcideki sınıflar güncellendi.
API UserApprove Op Contract eklendi.
Password postback'te silinmemesi sağlandı.
Register User Detail string.IsNullOrEmpty(RegisteredUserDetail.City) ? "No Recorded City" : RegisteredUserDetail.City,
Profanity Filter Register Page eklendi.
INSTEAD OF INSERT Trigger içerisine SET NO COUNT ON değeri eklendi trigger değerleri değiştirdiği için 1 row affected 2 sefer dönüş yapılıyordu bu sebeple dönüş değerini alamıyordum. Ayrıca Trigger içerisine -- FOR ENTITY FRAMEWORK CONCURRENCY EXCEPTION HANDLING
için scope_identity() olarak userid değerini döndürecek select eklendi ef concurrency exception için
UPDATE ve INSERT Triggerları ayrıştırıldı.
SaveChanges methodları serviste try catch alındı ve tüm post methodların int 0 ya da 1 döndürmesi sağlandı post başarılı oldumu diye serviste check ediliyor.
User ve UserDetail delete op contract oluşturuldu UserID DetailID.
Serviste gömülü sistem logosunu register mailinde göndermek için body için alternateview oluşturuldu ve iletildi.
Mail gönderimde 2Factor güvenlik olduğu için mail ayarlarından app password oluşturuldu.
Password oncopy,oncut,onpaste,ondelete false eklendi.
RegisterPage label text 4 saniye görüntüleme ardında countdown ve sayfa açıldığında ya da postback olduğunda countdown sayması sağlandı.
WebClient variable kullanımı sadeleştirildi.
Register ve Forgot password page password regex düzeltildi(Password requires at least one upper case English letter, at least one lower case English letter, at least one digit, at least one special character).
INSTEAD OF UPDATE USERS tablosu triggeri düzeltildi(sadece update password bakıyordu.)
Welcome,Register,Forgot password,Activation sayfaları buton timerları ve input kontrolleri eklendi.
Login'de aktivasyon ve approved kontrolü eklendi.
Login API'de username password base64 string'den okuması sağlandı clienttan'da base64 string olarak iletilmesi sağlandı.
Login API'de SP'Den dönen degeri okuması için güncelleme yapıldı.
Login SP'de return yerine select statement dönüşü yapıldı.
Profile Page Giriş yapan şirkete kayıt olan userların görüntülenmesi için downlist ler eklendi, timerler eklendi. Change password sayfasına yönlendiren buton eklendi.
User Tipi tablosu ve user tipleri eklendi.
Servisten aktif edilmiş approve edilmemiş userlar dönen op contract yazıldı.
label içerik approve/user silmeye göre değiştirildi ve Checkboxlist approve aynı zamanda approve edilmiş kişileri silebilmesi için güncellendi.(approve silme işlemi rol bazlı)
Approve, Refuse, Delete Sensor, Analyze, Change Code butonlarına doğrulama(alert confirm) eklendi.
Serviste delete user içerisinde deleteuserdetail çağırıldı
Password uzunluğu min8-max15, register page sensör add page labellar güncellendi. Country ve city select değeri liste dolduktan sonra seçildiğinde runtime hatası veriyordu düzeltildi.
Sensör ekleme sayfası oluşturuldu.
Sensor ve verisi ekleme op contract oluşturuldu serviste sp ler kullanıldı.
Monitoring page add sensör file extension kontrolü eklendi. Önyüzde tek butondan 3 farklı durum sağlandı(file selection, file validation before upload and upload file)
<httpRuntime targetFramework="4.8.1" maxRequestLength="4096"/> eklendi.
Client içerisine Nugetten DocumentFormat.OpenXml paketi indirildi.
system.webserver altına          <requestLimits maxAllowedContentLength="1073741824" /> eklendi ve fileupload için kontrol eklendi.
file kontrolleri(excel, excel cell format, empty file etc.) ve upload tamamlandı.
servis tarihler arası data alma order by asc eklendi.
data silme tarih aralığı ve id ye göre servise eklendi.
excel'e alma ve excel'den ekleme kontrollerle tamamlandı.
Chart milisecond göstermek için google datatable içerisinde date casting yapıldı.(JS milisecond erase ediyor.)
Chart resizing yapıldı.
Upload ve deleteden sonra chart'ın tekrar oluşması sağlandı
MonitorPage meta tag kullanıldı 120 saniyede 1 page reflesh olup güncel data doluyor.
Company,User,UserDetail,Sensor,SensorData audit tabloları eklendi ve triggerlar oluşturuldu.
aktif edilmemiş ve approve edilmemiş userlar, 1 yıl önceye ait veriler her gün job ile siliniyor.
upload,display ve downloadda günün tarihinden büyük tarih varsa hata verilmesi sağlandı.
Kritik uyarı için mail template oluşturuldu.
SensorAudit tablosundan yeni eklenen veriye bağlı olarak değerin kritik değerleri geçmesi durumunda otomatik mail atması için
Proje içerisine windows servis oluşturucu eklendi ve servis oluşturulup windows'a eklendi(C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe D:\P\SensorMonitoringSystem2023\SensorMonitoringSystem-master\SensorMonitoringSystemServiceCallerService\bin\Debug\SensorMonitoringSystemServiceCallerService.exe)
Otomatik mail atması için sensör tipi ve user tipine bağlı olarak servis methodu oluşturuldu.
Db-tablo-sp-function-trigger scriptleri ve constant defined tablo veri scriptleri oluşturuldu.
1 yıl öncesine ait sensör verilerini ve aktif edilmemiş userları günlük olarak silmek için job scripti eklendi.
Servis request bulunan her sayfa için timerlar eklendi.


