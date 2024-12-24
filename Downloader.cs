using System.Windows.Controls;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;

namespace База_данных_фирмы
{
    internal static class Downloader
    {
        private static string filePath = "Important/";

        public static void ExportToDocx(DataGrid dataGrid)
        {
            if (dataGrid == null || string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Не получилось.");
            }
            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "DataGrid" };
                    sheets.Append(sheet);

                    Worksheet worksheet = new Worksheet();
                    SheetData sheetData = new SheetData();
                    worksheet.Append(sheetData);
                    Row headerRow = new Row();
                    foreach (DataGridColumn column in dataGrid.Columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(column.Header.ToString());
                        headerRow.Append(cell);
                    }
                    sheetData.Append(headerRow);
                    foreach (DataRowView rowView in dataGrid.Items)
                    {
                        Row dataRow = new Row();
                        foreach (DataGridColumn column in dataGrid.Columns)
                        {
                            Cell cell = new Cell();
                            cell.DataType = CellValues.String;
                            object value = null;
                            try
                            {
                                value = rowView[column.Header.ToString()];
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка при доступе к столбцу {column.Header}: {ex.Message}");
                                value = "";
                            }
                            cell.CellValue = new CellValue(value?.ToString() ?? "");
                            dataRow.Append(cell);
                        }
                        sheetData.Append(dataRow);
                    }
                    worksheetPart.Worksheet = worksheet;
                    workbookPart.Workbook.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка конвертации .docx: " + ex.Message);
            }
        }

    }
}
