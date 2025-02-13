using Dapper;
using PlateDapperProject.Context;
using PlateDapperProject.PlateDtos;
using System;

namespace PlateDapperProject.PlateRepositories
{
    public class PlateRepository:IPlateRepository
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
        SELECT 
            YEAR(LICENCEDATE) AS Year,
            Brand,
            COUNT(*) AS VehicleCount
        FROM PLATES
        WHERE Brand IS NOT NULL
        GROUP BY YEAR(LICENCEDATE), Brand
        ORDER BY Year, VehicleCount DESC";
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
    ShiftType AS Category,
    COUNT(*) AS VehicleCount
FROM PLATES
WHERE ShiftType IS NOT NULL
GROUP BY ShiftType
ORDER BY VehicleCount DESC;
";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<object> GetColorDistributionAsync()
        {
            string query = @"
SELECT 
    Color AS Category,
    COUNT(*) AS VehicleCount
FROM PLATES
WHERE Color IS NOT NULL
GROUP BY Color
ORDER BY VehicleCount DESC;
";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<List<object>> GetFuelDistributionAsync()
        {
            string query = @"
SELECT 
    Fuel AS Category,
    COUNT(*) AS VehicleCount,
    (COUNT(*) * 100.0 / (SELECT COUNT(*) FROM PLATES)) AS Percentage
FROM PLATES
WHERE Fuel IS NOT NULL
GROUP BY Fuel
ORDER BY VehicleCount DESC;
";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<object> GetFuelDistributionByYearAsync()
        {
            string query = @"
        SELECT 
            YEAR(LicenceDate) AS Year,
            Fuel AS Category,
            COUNT(*) AS VehicleCount
        FROM PLATES
        WHERE Fuel IS NOT NULL
        GROUP BY YEAR(LicenceDate), Fuel
        ORDER BY Year, VehicleCount DESC
";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<object> GetMonthlyVehicleRegistrationsByAllBrandsAsync()
        {
            string query = @"
        SELECT 
            YEAR(LicenceDate) AS Year,
            MONTH(LicenceDate) AS Month,
            Brand,
            COUNT(*) AS VehicleCount
        FROM PLATES
        WHERE Brand IS NOT NULL
        GROUP BY YEAR(LicenceDate), MONTH(LicenceDate), Brand
        ORDER BY Year, Month, VehicleCount DESC";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }

        public async Task<object> GetNewVehicleRegistrationsByMonthAsync()
        {
            string query = @"
        SELECT 
            YEAR(LicenceDate) AS Year,
            MONTH(LicenceDate) AS Month,
            COUNT(*) AS VehicleCount
        FROM PLATES
        WHERE LicenceDate IS NOT NULL
        GROUP BY YEAR(LicenceDate), MONTH(LicenceDate)
        ORDER BY Year, Month";
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
            string query = @"
        SELECT 
            Brand,
            COUNT(*) AS VehicleCount
        FROM PLATES
        WHERE Brand IS NOT NULL
        GROUP BY Brand
        ORDER BY VehicleCount DESC";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<dynamic>(query);
            return values.ToList();
        }
    }
}
