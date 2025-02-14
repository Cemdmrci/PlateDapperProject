using Dapper;
using PlateDapperProject.Context;
using PlateDapperProject.PlateDtos;
using System;

namespace PlateDapperProject.PlateRepositories
{
    public class PlateRepository : IPlateRepository
    {
        private readonly PlateContext _context;

        public PlateRepository(PlateContext context)
        {
            _context = context;
        }

        public async Task<List<ResultPlateDto>> GetAllPlateAsync()
        {
            string query = "Select * From PLATES";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultPlateDto>(query);
            return values.ToList();
        }

        public async Task<object> GetAnnualFuelTypeVehicleCountAsync()
        {
            string query = @"
            SELECT 
                YEAR(LICENCEDATE) AS Year, /* Araç ruhsat tarihinden yılı alıyor*/
                FUEL,
                COUNT(*) AS VehicleCount /*O yıl ve yakıt türüne sahip kaç araç olduğunu sayıyor*/
            FROM PLATES
            WHERE FUEL IN ('Benzin', 'Dizel')
            GROUP BY YEAR(LICENCEDATE), FUEL /*Yıla ve yakıt türüne göre gruplama yapıyoruz. Yani, her yıl için ""Benzin"" ve ""Dizel"" olarak ayrı ayrı toplam araç sayılarını buluyoruz.*/
            ORDER BY Year"; /*Sonuçları yıla göre sıralıyoruz(artan şekilde).*/
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();

        }

        public async Task<object> GetAnnualTopFuelTypeAsync()
        {
            string query = @"
    WITH FuelCounts AS (   /*Yıllara göre yakıt türü dağılımını çıkarır.*/
        SELECT 
            YEAR(LICENCEDATE) AS Year,
            FUEL,
            COUNT(*) AS FuelTypeCount /*O yıl içinde o yakıt türüne sahip kaç araç var, bunu sayıyoruz*/
        FROM PLATES
        GROUP BY YEAR(LICENCEDATE), FUEL
    ),
    RankedFuelCounts AS ( /*En Popüler Yakıt Türünü Bulma*/
        SELECT 
            Year,
            FUEL,
            FuelTypeCount,
            RANK() OVER (PARTITION BY Year ORDER BY FuelTypeCount DESC) AS Rank /*o yıl içinde en popüler yakıt türünü 1. sıraya koyuyoruz.*/
        FROM FuelCounts
    )
    SELECT 
        Year,
        FUEL AS TopFuelType,
        FuelTypeCount
    FROM RankedFuelCounts /*RANK() fonksiyonu ile en fazla kullanılan yakıt türünü sıralar.*/
    WHERE Rank = 1 /*En popüler yakıt türünü seçiyoruz*/
    ORDER BY Year;
    ";

            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();

        }

        public async Task<object> GetAnnualVehicleCountAsync()
        {
            string query = @"
     SELECT 
YEAR(LICENCEDATE) AS Year,
COUNT(*) AS VehicleCount
            FROM PLATES
            GROUP BY YEAR(LICENCEDATE)
            ORDER BY Year;

";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<object> GetCategoryWiseVehicleCountsAsync()
        {
            string query = @"
            SELECT
     'Fuel' AS Category, FUEL AS SubCategory, COUNT(*) AS VehicleCount
            FROM PLATES
            GROUP BY FUEL

            UNION ALL

            SELECT 
                'ShiftType' AS Category, SHIFTTYPE AS SubCategory, COUNT(*) AS VehicleCount
            FROM PLATES
            GROUP BY SHIFTTYPE

            UNION ALL

            SELECT 
                'CaseType' AS Category, CASETYPE AS SubCategory, COUNT(*) AS VehicleCount
            FROM PLATES
            GROUP BY CASETYPE;
        ";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<object> GetColorDistributionAsync()
        {
            string query = @"
                SELECT
                COLOR AS Color, 
                COUNT(*) AS VehicleCount, 
                CAST(COUNT(*) * 100.0 / SUM(COUNT(*)) OVER () AS DECIMAL(5, 2)) AS Percentage
            FROM PLATES
            GROUP BY COLOR
            ORDER BY Percentage DESC;
";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<List<object>> GetFuelDistributionAsync()
        {
            string query = "SELECT DISTINCT Fuel, COUNT(*) AS VehicleCount FROM PLATES GROUP BY Fuel";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<object> GetFuelDistributionByYearAsync()
        {
            string query = @"
            SELECT Year_, Fuel, COUNT(*) AS VehicleCount
            FROM PLATES
            GROUP BY Year_, Fuel
            ORDER BY Year_, Fuel";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<object> GetMonthlyVehicleRegistrationsByAllBrandsAsync()
        {
            string query = @"
SELECT Brand, MONTH(LICENCEDATE) AS Month, COUNT(*) AS VehicleCount
            FROM PLATES
            WHERE LICENCEDATE IS NOT NULL
            GROUP BY Brand, MONTH(LICENCEDATE)
            ORDER BY Brand, MONTH(LICENCEDATE)";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<object> GetNewVehicleRegistrationsByMonthAsync()
        {
            string query = @"
            SELECT MONTH(LICENCEDATE) AS Month, COUNT(*) AS VehicleCount
            FROM PLATES
            WHERE LICENCEDATE IS NOT NULL
            GROUP BY MONTH(LICENCEDATE)
            ORDER BY MONTH(LICENCEDATE)";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<List<dynamic>> GetTop5BrandsAsync()
        {
            string query = @"
    SELECT TOP 5 
        BRAND, 
        COUNT(*) AS VehicleCount
    FROM PLATES
    GROUP BY BRAND
    ORDER BY VehicleCount DESC";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<List<dynamic>> GetTop5CaseTypesAsync()
        {
            string query = @"
    SELECT TOP 5 
        CASETYPE AS CaseType, 
        COUNT(*) AS VehicleCount
    FROM PLATES
    GROUP BY CASETYPE
    ORDER BY VehicleCount DESC";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<List<dynamic>> GetTop5ColorsAsync()
        {
            string query = @"
    SELECT TOP 5 
        COLOR AS Color, 
        COUNT(*) AS VehicleCount
    FROM PLATES
    GROUP BY COLOR
    ORDER BY VehicleCount DESC";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<List<dynamic>> GetTop5EngineCapacitiesAsync()
        {
            string query = @"
    SELECT TOP 5 
        MOTORVOLUME AS EngineCapacity, 
        COUNT(*) AS VehicleCount
    FROM PLATES
    GROUP BY MOTORVOLUME
    ORDER BY VehicleCount DESC";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<List<dynamic>> GetTop5FuelTypesAsync()
        {
            string query = @"
    SELECT TOP 5 
        FUEL AS FuelType, 
        COUNT(*) AS VehicleCount
    FROM PLATES
    GROUP BY FUEL
    ORDER BY VehicleCount DESC";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<List<object>> GetVehicleCountByBrandAsync()
        {
            string query = "SELECT Brand, COUNT(*) as VehicleCount FROM PLATES GROUP BY Brand";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }
    }
}
