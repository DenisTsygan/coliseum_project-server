using System.Text;

public class DataService
{

    private readonly IElectricityConsumedDayRepository _ecDayRepository;

    private readonly IElectricityConsumedMounthRepository _ecMounthRepository;



    public DataService(IElectricityConsumedDayRepository electricityConsumedDayRepository,
        IElectricityConsumedMounthRepository electricityConsumedMounthRepository
        )
    {
        _ecDayRepository = electricityConsumedDayRepository;
        _ecMounthRepository = electricityConsumedMounthRepository;
    }

    public async Task<List<ElectricityConsumedMounthEntity>> GetECMByPeriodName(string periodName)
    {
        return await _ecMounthRepository.GetByPeriodDate(periodName);
    }

    public async Task<MemoryStream> GetFileECMByPeriodExcel(string periodName)
    {
        //TODO file D:\Dowland chrome\ElectricityData_12-2024 (2).xlsx
        using var workbook = new ClosedXML.Excel.XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Electricity Data");

        // Добавляем заголовки
        worksheet.Cell(1, 1).Value = "Id";
        worksheet.Cell(1, 2).Value = "Client Name";
        worksheet.Cell(1, 3).Value = "Period Date";
        worksheet.Cell(1, 4).Value = "Total Hours (Period)";
        worksheet.Cell(1, 5).Value = "Total Electricity Consumed (kW·h)";
        worksheet.Cell(1, 6).Value = "Day";
        worksheet.Cell(1, 7).Value = "Day Electricity Consumed (kW·h)";
        worksheet.Cell(1, 8).Value = "Hourly Data (kW·h)";

        // Получение данных из базы данных
        var data = await _ecMounthRepository.GetByPeriodDate(periodName);

        int row = 2;

        foreach (var month in data)
        {
            // Первая строка для месяца
            worksheet.Cell(row, 1).Value = month.Id.ToString();
            worksheet.Cell(row, 2).Value = month.Client?.Name ?? "Unknown Client";
            worksheet.Cell(row, 3).Value = month.PeriodDate;
            worksheet.Cell(row, 4).Value = month.Period;
            worksheet.Cell(row, 5).Value = month.AllElectricyConsumed;

            // Выводим данные по дням
            foreach (var day in month.ElectricyConsumedDays)
            {
                worksheet.Cell(row, 6).Value = day.Day;
                worksheet.Cell(row, 7).Value = day.AllElectricyConsumed;
                worksheet.Cell(row, 8).Value = string.Join(", ", day.ElectricyConsumedHours);

                row++; // Следующая строка
            }

            row++; // Пропуск строки между месяцами
        }

        // Генерируем файл Excel
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        return stream;
        //return await _ecMounthRepository.GetByPeriodDate(periodName);
    }

    public async Task<string> GetFileECMByPeriodCSV(string periodName)
    {
        //TODO file D:\Dowland chrome\ElectricityData_12-2024.csv 
        var data = await _ecMounthRepository.GetByPeriodDate(periodName);

        var csv = new StringBuilder();
        csv.AppendLine("Id,Name,Period,AllElectricyConsumed");

        foreach (var item in data)
        {
            csv.AppendLine($"{item.Id},{item.Client?.Name ?? "Unknown Client"},{item.Period},{item.AllElectricyConsumed}");
        }

        return csv.ToString();
    }

    public async Task InitData()
    {
        await _ecMounthRepository.Init();
        await _ecDayRepository.Init();
    }

    public async Task RenameClientById(Guid ecmId, string newName)
    {
        await _ecMounthRepository.RenameClientById(ecmId, newName);
    }

}