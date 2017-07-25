//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Web;
//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;
//using NPOI.SS.Util;
//using NPOI.XSSF.UserModel;
//using System.Text;

//namespace LoticLight.Web.App_Start
//{
//    public class ExcelHerp : IDisposable
//    {
//        #region 从datatable中将数据导出到excel
//        /// <summary>
//        /// 导出excel到MemoryStream中
//        /// </summary>
//        /// <param name="dtSource">源Datatable数据</param>
//        /// <param name="strHeaderText">表头文本</param>
//        /// <param name="sty">是否包含表头</param>
//        /// <returns></returns>
//        public static MemoryStream ExportDT(DataTable dtSource, string strHeaderText, bool sty)
//        {
//            HSSFWorkbook workbook = new HSSFWorkbook();
//            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;

//            #region 右击文件 属性信息

//            //{
//            //    DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
//            //    dsi.Company = "http://www.yongfa365.com/";
//            //    workbook.DocumentSummaryInformation = dsi;

//            //    SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
//            //    si.Author = "柳永法"; //填加xls文件作者信息
//            //    si.ApplicationName = "NPOI测试程序"; //填加xls文件创建程序信息
//            //    si.LastAuthor = "柳永法2"; //填加xls文件最后保存者信息
//            //    si.Comments = "说明信息"; //填加xls文件作者信息
//            //    si.Title = "NPOI测试"; //填加xls文件标题信息
//            //    si.Subject = "NPOI测试Demo"; //填加文件主题信息
//            //    si.CreateDateTime = DateTime.Now;
//            //    workbook.SummaryInformation = si;
//            //}

//            #endregion

//            HSSFCellStyle dateStyle = workbook.CreateCellStyle() as HSSFCellStyle;
//            HSSFDataFormat format = workbook.CreateDataFormat() as HSSFDataFormat;
//            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

//            HSSFCellStyle datetimeStyle = workbook.CreateCellStyle() as HSSFCellStyle;
//            datetimeStyle.DataFormat = format.GetFormat("yyyy-mm-dd hh:mm:ss");
//            //取得列宽
//            int[] arrColWidth = new int[dtSource.Columns.Count];
//            foreach (DataColumn item in dtSource.Columns)
//            {
//                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
//            }
//            for (int i = 0; i < dtSource.Rows.Count; i++)
//            {
//                for (int j = 0; j < dtSource.Columns.Count; j++)
//                {
//                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
//                    if (intTemp > arrColWidth[j])
//                    {
//                        arrColWidth[j] = intTemp;
//                    }
//                }
//            }
//            int rowIndex = 0;

//            foreach (DataRow row in dtSource.Rows)
//            {
//                #region 新建表，填充表头，填充列头，样式

//                if (rowIndex == 65535 || rowIndex == 0)
//                {
//                    if (rowIndex != 0)
//                    {
//                        sheet = workbook.CreateSheet() as HSSFSheet;
//                    }

//                    #region 表头及样式
//                    if (sty == true)
//                    {
//                        HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
//                        headerRow.HeightInPoints = 25;
//                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

//                        HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
//                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
//                        HSSFFont font = workbook.CreateFont() as HSSFFont;
//                        font.FontHeightInPoints = 20;
//                        font.Boldweight = 700;
//                        headStyle.SetFont(font);

//                        headerRow.GetCell(0).CellStyle = headStyle;

//                        sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
//                        //headerRow.Dispose();
//                    }

//                    #endregion


//                    #region 列头及样式
//                    var i = 0;
//                    if (sty == true)
//                        i = 1;
//                    {

//                        HSSFRow headerRow = sheet.CreateRow(i) as HSSFRow;


//                        HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
//                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
//                        HSSFFont font = workbook.CreateFont() as HSSFFont;
//                        font.FontHeightInPoints = 10;
//                        font.Boldweight = 700;
//                        headStyle.SetFont(font);


//                        foreach (DataColumn column in dtSource.Columns)
//                        {
//                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
//                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

//                            //设置列宽
//                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);

//                        }
//                    }

//                    #endregion

//                    rowIndex = i + 1;
//                }

//                #endregion

//                #region 填充内容

//                HSSFRow dataRow = sheet.CreateRow(rowIndex) as HSSFRow;
//                foreach (DataColumn column in dtSource.Columns)
//                {
//                    HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;

//                    string drValue = row[column].ToString();

//                    switch (column.DataType.ToString())
//                    {
//                        case "System.String": //字符串类型
//                            newCell.SetCellValue(drValue);
//                            break;
//                        case "System.DateTime": //日期类型
//                            DateTime dateV;
//                            DateTime.TryParse(drValue, out dateV);
//                            newCell.SetCellValue(dateV);
//                            if (dateV.Hour == 0)
//                                newCell.CellStyle = dateStyle;//格式化显示，只有日期
//                            else
//                                newCell.CellStyle = datetimeStyle;//格式化显示，日期时间
//                            break;
//                        case "System.Boolean": //布尔型
//                            bool boolV = false;
//                            bool.TryParse(drValue, out boolV);
//                            newCell.SetCellValue(boolV);
//                            break;
//                        case "System.Int16": //整型
//                        case "System.Int32":
//                        case "System.Int64":
//                        case "System.Byte":
//                            int intV = 0;
//                            int.TryParse(drValue, out intV);
//                            newCell.SetCellValue(intV);
//                            break;
//                        case "System.Decimal": //浮点型
//                        case "System.Double":
//                            double doubV = 0;
//                            double.TryParse(drValue, out doubV);
//                            newCell.SetCellValue(doubV);
//                            break;
//                        case "System.DBNull": //空值处理
//                            newCell.SetCellValue("");
//                            break;
//                        default:
//                            newCell.SetCellValue("");
//                            break;
//                    }

//                }
//                #endregion

//                rowIndex++;
//            }
//            //using (MemoryStream ms = new MemoryStream())
//            //{
//            MemoryStream ms = new MemoryStream();
//            workbook.Write(ms);
//            ms.Flush();
//            ms.Position = 0;

//            //sheet;
//            //workbook.Dispose();

//            return ms;
//            //}
//        }

//        /// <summary>
//        /// DataTable导出到Excel文件
//        /// </summary>
//        /// <param name="dtSource">源DataTable</param>
//        /// <param name="strHeaderText">表头文本</param>
//        /// <param name="strFileName">保存位置</param>
//        public static void ExportDTtoExcel(DataTable dtSource, string strHeaderText, string strFileName, bool sty)
//        {
//            using (MemoryStream ms = ExportDT(dtSource, strHeaderText, sty))
//            {
//                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
//                {
//                    byte[] data = ms.ToArray();
//                    fs.Write(data, 0, data.Length);
//                    fs.Flush();
//                }
//            }
//        }
//        #endregion


//        #region 从datatable中将数据导出到excel

//        private string fileName = null; //文件名
//        private IWorkbook workbook = null;
//        private FileStream fs = null;
//        private bool disposed;

//        public ExcelHerp(string fileName)
//        {
//            this.fileName = fileName;
//            disposed = false;
//        }

//        /// <summary>
//        /// 将excel中的数据导入到DataTable中
//        /// </summary>
//        /// <param name="sheetName">excel工作薄sheet的名称</param>
//        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
//        /// <returns>返回的DataTable</returns>
//        public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
//        {
//            ISheet sheet = null;
//            DataTable data = new DataTable();
//            int startRow = 0;
//            try
//            {
//                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
//                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
//                    workbook = new XSSFWorkbook(fs);
//                else if (fileName.IndexOf(".xls") > 0) // 2003版本
//                    workbook = new HSSFWorkbook(fs);

//                if (sheetName != null)
//                {
//                    sheet = workbook.GetSheet(sheetName);
//                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
//                    {
//                        sheet = workbook.GetSheetAt(0);
//                    }
//                }
//                else
//                {
//                    sheet = workbook.GetSheetAt(0);
//                }
//                if (sheet != null)
//                {
//                    IRow firstRow = sheet.GetRow(0);
//                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

//                    if (isFirstRowColumn)
//                    {
//                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
//                        {
//                            ICell cell = firstRow.GetCell(i);
//                            if (cell != null)
//                            {
//                                string cellValue = cell.StringCellValue;
//                                if (cellValue != null)
//                                {
//                                    DataColumn column = new DataColumn(cellValue);
//                                    data.Columns.Add(column);
//                                }
//                            }
//                        }
//                        startRow = sheet.FirstRowNum + 1;
//                    }
//                    else
//                    {
//                        startRow = sheet.FirstRowNum;
//                    }

//                    //最后一列的标号
//                    int rowCount = sheet.LastRowNum;
//                    for (int i = startRow; i <= rowCount; ++i)
//                    {
//                        IRow row = sheet.GetRow(i);
//                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

//                        DataRow dataRow = data.NewRow();
//                        for (int j = row.FirstCellNum; j < cellCount; ++j)
//                        {
//                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
//                                dataRow[j] = row.GetCell(j).ToString();
//                        }
//                        data.Rows.Add(dataRow);
//                    }
//                }

//                return data;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Exception: " + ex.Message);
//                return null;
//            }
//        }


//        /// <summary>
//        /// 将excel中的数据导入到DataTable中
//        /// </summary>
//        /// <param name="sheetName">excel工作薄sheet的名称</param>
//        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
//        /// <returns>返回的DataTable</returns>
//        public DataTable ExcelToDataTable1(string sheetName, bool isFirstRowColumn)
//        {
//            ISheet sheet = null;
//            DataTable data = new DataTable();
//            int startRow = 0;
//            try
//            {
//                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
//                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
//                    workbook = new XSSFWorkbook(fs);
//                else if (fileName.IndexOf(".xls") > 0) // 2003版本
//                    workbook = new HSSFWorkbook(fs);

//                if (sheetName != null)
//                {
//                    sheet = workbook.GetSheet(sheetName);
//                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
//                    {
//                        sheet = workbook.GetSheetAt(0);
//                    }
//                }
//                else
//                {
//                    sheet = workbook.GetSheetAt(0);
//                }
//                if (sheet != null)
//                {
//                    IRow firstRow = sheet.GetRow(0);
//                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

//                    if (isFirstRowColumn)
//                    {
//                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
//                        {
//                            ICell cell = firstRow.GetCell(i);
//                            if (cell != null)
//                            {
//                                if (cell.StringCellValue == "")
//                                {
//                                    string cellValue = sheet.GetRow(0).GetCell(i-1).StringCellValue + sheet.GetRow(1).GetCell(i).StringCellValue;
//                                    if (cellValue != null)
//                                    {
//                                        DataColumn column = new DataColumn(cellValue);
//                                        data.Columns.Add(column);
//                                    }
//                                }
//                                else
//                                {
//                                    string cellValue = cell.StringCellValue;
//                                    if (cellValue != null)
//                                    {
//                                        DataColumn column = new DataColumn(cellValue);
//                                        data.Columns.Add(column);
//                                    }
//                                }

//                            }
//                        }
//                        startRow = sheet.FirstRowNum + 2;
//                    }
//                    else
//                    {
//                        startRow = sheet.FirstRowNum;
//                    }

//                    //最后一列的标号
//                    int rowCount = sheet.LastRowNum;
//                    for (int i = startRow; i <= rowCount; ++i)
//                    {
//                        IRow row = sheet.GetRow(i);
//                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

//                        DataRow dataRow = data.NewRow();
//                        for (int j = row.FirstCellNum; j < cellCount; ++j)
//                        {
//                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
//                                dataRow[j] = row.GetCell(j).ToString();
//                        }
//                        data.Rows.Add(dataRow);
//                    }
//                }

//                return data;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Exception: " + ex.Message);
//                return null;
//            }
//        }
//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!this.disposed)
//            {
//                if (disposing)
//                {
//                    if (fs != null)
//                        fs.Close();
//                }

//                fs = null;
//                disposed = true;
//            }
//        }
//        #endregion
//    }
//}