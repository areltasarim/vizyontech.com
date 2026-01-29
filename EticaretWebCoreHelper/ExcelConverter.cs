using CsvHelper;
using CsvHelper.Configuration;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreHelper
{
    public static class ExcelConverter
    {
        public static void ConvertToExcel(string csvFilePath, string xlsFilePath)
        {
            //BOZUK EXCEL DOSYASINI YENİDEN KAYETME
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("Sheet1");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                Encoding = Encoding.GetEncoding("ISO-8859-1")
            };

            using (var reader = new StreamReader(csvFilePath, Encoding.GetEncoding("ISO-8859-1"), true))
            using (var csv = new CsvReader(reader, config))
            {
                var rowNumber = 0;

                while (csv.Read())
                {
                    var row = sheet.CreateRow(rowNumber);

                    for (var i = 0; i < csv.Parser.Count; i++)
                    {
                        var cell = row.CreateCell(i);
                        cell.SetCellValue(csv.GetField(i));
                    }

                    rowNumber++;
                }
            }

            using (var fileStream = new FileStream(xlsFilePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }
            //BOZUK EXCEL DOSYASINI YENİDEN KAYETME

            //XLS DOSYASINI XLSX DOSYASINA ÇEVİRME
            string xlsxFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadExcel", "sariEtiketlerYuklenecek.xlsx");
            using (FileStream file = new FileStream(xlsFilePath, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook newWorkbook = new XSSFWorkbook();
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    HSSFSheet xlsSheet = workbook.GetSheetAt(i) as HSSFSheet;
                    ISheet newSheet = newWorkbook.CreateSheet(xlsSheet.SheetName);

                    // .xls sayfasındaki her satırı .xlsx sayfasına kopyala
                    for (int rowIndex = 0; rowIndex <= xlsSheet.LastRowNum; rowIndex++)
                    {
                        HSSFRow xlsRow = xlsSheet.GetRow(rowIndex) as HSSFRow;
                        IRow newRow = newSheet.CreateRow(rowIndex);

                        // .xls satırındaki her hücreyi .xlsx satırına kopyala
                        for (int cellIndex = 0; cellIndex < xlsRow.LastCellNum; cellIndex++)
                        {
                            HSSFCell xlsCell = xlsRow.GetCell(cellIndex) as HSSFCell;
                            ICell newCell = newRow.CreateCell(cellIndex);

                            // Hücrenin değerini kopyala
                            newCell.SetCellValue(xlsCell.StringCellValue);
                        }
                    }
                }
                // .xlsx dosyasını kaydet
                using (FileStream newFile = new FileStream(xlsxFilePath, FileMode.Create))
                {
                    newWorkbook.Write(newFile);
                }
            }
            //XLS DOSYASINI XLSX DOSYASINA ÇEVİRME

        }

    }
}
