using PlateDapperProject.PlateDtos;

namespace PlateDapperProject.PlateRepositories
{
    public interface IPlateRepository
    {
        Task<List<ResultPlateDto>> GetAllPlateAsync();
        Task<List<object>> GetFuelDistributionAsync();//Yakıt türlerine göre (benzin, dizel, elektrik, hibrit, vb.) dağılımı hesaplar.
        Task<List<object>> GetVehicleCountByBrandAsync();//Markalara göre araç sayısını hesaplar.
        Task<object> GetFuelDistributionByYearAsync();//Yıllara göre yakıt türü dağılımını hesaplar.
        Task<object> GetNewVehicleRegistrationsByMonthAsync();//Aylara göre yeni araç kayıt sayısını hesaplar.
        Task<object> GetMonthlyVehicleRegistrationsByAllBrandsAsync();//Tüm markaların aylık araç kayıt sayılarını hesaplar.
        Task<object> GetAnnualFuelTypeVehicleCountAsync();//Yıllara göre yakıt türüne göre araç sayısını hesaplar.
        Task<object> GetAnnualVehicleCountAsync();//Yıllara göre toplam araç kayıt sayısını hesaplar.
        Task<object> GetAnnualTopFuelTypeAsync();//Yıllara göre en popüler yakıt türünü hesaplar.
        Task<object> GetCategoryWiseVehicleCountsAsync();//Kategorilere göre (örneğin, binek, ticari, motosiklet) araç sayısını hesaplar.
        Task<object> GetColorDistributionAsync();//Araç renklerine göre dağılımı hesaplar.
        Task<List<dynamic>> GetTop5BrandsAsync();//En çok kaydedilen ilk 5 markayı getirir.
        Task<List<dynamic>> GetTop5FuelTypesAsync();//En çok kullanılan ilk 5 yakıt türünü getirir.
        Task<List<dynamic>> GetTop5ColorsAsync();//En çok kullanılan ilk 5 rengi getirir.
        Task<List<dynamic>> GetTop5EngineCapacitiesAsync();//En çok kullanılan ilk 5 motor hacmini getirir.
        Task<List<dynamic>> GetTop5CaseTypesAsync();//En çok kullanılan ilk 5 kasa türünü getirir.
    }
}
