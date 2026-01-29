using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;

namespace EticaretWebCoreHelper
{
    public static class ClosedXmlExtentions
    {
        public static DataTable ToDataTable(this IXLWorksheet Sayfa)
        {
            DataTable Sonuc = new DataTable();
            bool firstRow = true;
            foreach (IXLRow row in Sayfa.Rows())
            {
                if (firstRow)
                {
                    foreach (IXLCell cell in row.Cells())
                    {
                        Sonuc.Columns.Add(cell.Value.ToString());
                    }
                    firstRow = false;
                }
                else
                {
                    Sonuc.Rows.Add();
                    int i = 0;

                    foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                    {
                        Sonuc.Rows[Sonuc.Rows.Count - 1][i] = cell.Value.ToString();
                        i++;
                    }
                }
            }
            return Sonuc;
        }
        public static List<T> ToImportModel<T>(this IXLWorksheet Sayfa)
        {
            List<T> Sonuc = new List<T>();
            bool firstRow = true;
            foreach (IXLRow Satir in Sayfa.Rows())
            {
                if (firstRow)
                {
                    firstRow = false;
                }
                else
                {
                    Type KayitType = typeof(T);
                    object Kayit = Activator.CreateInstance(KayitType);
                    var PropertyListesi = KayitType.GetProperties();

                    var KontrolHucre = Satir.FirstCellUsed();
                    if (KontrolHucre!=null)
                    {
                        List<string> SatirVeriler = new List<string>();
                        int firstcolumn = KontrolHucre.Address.ColumnNumber;
                        int lastcolumn = PropertyListesi.Length;
                        if (Satir.Cells(firstcolumn, lastcolumn) != null)
                        {
                            foreach (IXLCell cell in Satir.Cells(firstcolumn, lastcolumn))
                            {
                                string Deger = (cell?.CachedValue != null ? cell.CachedValue.ToString() : "");
                                SatirVeriler.Add(Deger);
                            }

                            for (int PropertyIndex = 0; PropertyIndex < SatirVeriler.Count; PropertyIndex++)
                            {
                                PropertyListesi[PropertyIndex].SetValue(Kayit, SatirVeriler[PropertyIndex]);
                            }

                            Sonuc.Add((T)Kayit);
                        }
                    }
                }
            }
            return Sonuc;
        }
    }
}
