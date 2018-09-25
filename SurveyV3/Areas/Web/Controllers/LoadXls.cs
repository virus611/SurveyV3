using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;

namespace SurveyV3.Areas.Web.Controllers
{
    public class LoadXls
    {
        public static DataTable loadXls(HttpPostedFileBase fostFile)
        {
            System.IO.Stream streamfile = fostFile.InputStream;
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(streamfile);
            using (NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(0))
            {
                DataTable table = new DataTable();
                IRow headerRow = sheet.GetRow(0);//第一行为标题行  
                int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells  
                int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1  
                //handling header.  
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }
                for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();
                    if (row != null)
                    {
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                dataRow[j] = LoadXls.GetCellValue(row.GetCell(j));
                        }
                    }
                    table.Rows.Add(dataRow); 
                }
                return table;
            }
        }


        /// <summary>  
        /// 根据Excel列类型获取列的值  
        /// </summary>  
        /// <param name="cell">Excel列</param>  
        /// <returns></returns>  
        private static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.BLANK:
                    return string.Empty;
                case CellType.BOOLEAN:
                    return cell.BooleanCellValue.ToString();
                case CellType.ERROR:
                    return cell.ErrorCellValue.ToString();
                case CellType.NUMERIC:
                case CellType.Unknown:
                default:
                    return cell.ToString();
                case CellType.STRING:
                    return cell.StringCellValue;
                case CellType.FORMULA:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }

        }
    }
}