using B1Core.Business;
using B1Core.Business.UI;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;

namespace B1Core.Initial
{
    public static class TableCreate
    {

        public static bool TableExists(string TableName)
        {
            SAPbobsCOM.UserTablesMD oTables;
            oTables = (SAPbobsCOM.UserTablesMD)Main.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
            bool oFlag = false;
            oFlag = oTables.GetByKey(TableName);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oTables);
            oTables = null;
            GCClear();
            return oFlag;
        }

        public static bool CreateTable(string TableName, string TableDesc, SAPbobsCOM.BoUTBTableType TableType)
        {
            if (TableExists(TableName))
            {
                return true;
            }

            int RetVal = 0;
            int ErrCode = 0;
            string ErrMsg = "";

            try
            {
                Application.SBO_Application.StatusBar.SetText("Tablo Oluşturuluyor " + TableName + " ..........Lütfen Bekleyiniz.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                SAPbobsCOM.UserTablesMD v_UserTableMD;
                v_UserTableMD = (SAPbobsCOM.UserTablesMD)Main.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);

                v_UserTableMD.TableName = TableName;
                v_UserTableMD.TableDescription = TableDesc;
                v_UserTableMD.TableType = TableType;


                RetVal = v_UserTableMD.Add();
                if (RetVal != 0)
                {
                    Main.oCompany.GetLastError(out ErrCode, out ErrMsg);
                    Application.SBO_Application.StatusBar.SetText(" Hata Alınan Tablo : " + TableName + " Hata Kodu: " + ErrCode + " Hata Tanımı: " + ErrMsg, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(v_UserTableMD);
                    v_UserTableMD = null;
                    GCClear();
                    return false;
                }
                else
                {
                    Application.SBO_Application.StatusBar.SetText("Tablo : " + TableName + " Başarıyla Eklendi!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(v_UserTableMD);
                    v_UserTableMD = null;
                    GCClear();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(" Hata Alınan Tablo : " + TableName + " Hata : " + ex.ToString(), SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return false;
            }
        }

        public static void CreateTable(object nAR_EDOCNUM, string v, BoUTBTableType bott_NoObjectAutoIncrement)
        {
            throw new NotImplementedException();
        }

        private static bool ColumnExists(string TableName, string FieldID)
        {
            try
            {
                SAPbobsCOM.Recordset rs = (SAPbobsCOM.Recordset)Main.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                bool oFlag = true;
                //rs.DoQuery("Select 1 from CUFD Where CUFD.\"TableID\"=\'" + TableName.Trim() + "\' and CUFD.\"AliasID\"=\'" + FieldID.Trim() + "\'");
                //string sqlClause = "select count(*) as count from LGSFT_GET_CUFD Where \"TableID\"='" + TableName.Trim() + "' and \"AliasID\"='" + FieldID.Trim() + "'";
                string sqlClause = "select count(*) as count from CUFD Where \"TableID\"='" + TableName.Trim() + "' and \"AliasID\"='" + FieldID.Trim() + "'";
                rs.DoQuery(sqlClause);
                int count = Convert.ToInt32(rs.Fields.Item("count").Value);
                if (count == 0)
                    oFlag = false;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(rs);
                rs = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.WaitForFullGCComplete();
                return oFlag;
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message);
            }
            return true;
        }

        public static bool CreateUserFields(string TableName, string FieldName, string FieldDescription, SAPbobsCOM.BoFieldTypes type, long size = 0, SAPbobsCOM.BoFldSubTypes subType = SAPbobsCOM.BoFldSubTypes.st_None, string LinkedTable = "", string DefaultValue = "", Dictionary<string, string> ValidValues = null)
        {
            if (ColumnExists(TableName, FieldName))
            {
                return true;
            }

            int RetVal = 0;
            int ErrCode = 0;
            string ErrMsg = "";

            try
            {
                Application.SBO_Application.StatusBar.SetText("Alan: " + FieldName + " oluşturulurken bekleyiniz.!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                //if (TableName.StartsWith("@") == true)
                //{
                SAPbobsCOM.UserFieldsMD v_UserField = default(SAPbobsCOM.UserFieldsMD);
                v_UserField = (SAPbobsCOM.UserFieldsMD)Main.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                v_UserField.TableName = TableName;
                v_UserField.Name = FieldName;
                v_UserField.Description = FieldDescription;
                v_UserField.Type = type;


                if (ValidValues != null)
                {
                    foreach (var item in ValidValues)
                    {
                        v_UserField.ValidValues.Value = item.Key;
                        v_UserField.ValidValues.Description = item.Value;

                        v_UserField.ValidValues.Add();
                    }
                }

                if (type != SAPbobsCOM.BoFieldTypes.db_Date)
                {
                    if (size != 0)
                    {
                        if (type == SAPbobsCOM.BoFieldTypes.db_Numeric)
                        {
                            //v_UserField.EditSize = 11;
                        }
                        else
                        {
                            v_UserField.Size = (int)size;
                            v_UserField.EditSize = (int)size;
                        }
                    }
                }
                if (subType != SAPbobsCOM.BoFldSubTypes.st_None)
                {
                    v_UserField.SubType = subType;
                }
                if (!string.IsNullOrEmpty(LinkedTable))
                    v_UserField.LinkedTable = LinkedTable;
                if (!string.IsNullOrEmpty(DefaultValue))
                    v_UserField.DefaultValue = DefaultValue;

                RetVal = v_UserField.Add();
                if (RetVal != 0)
                {
                    Main.oCompany.GetLastError(out ErrCode, out ErrMsg);
                    Application.SBO_Application.StatusBar.SetText($"Hata:{ErrCode} - {ErrMsg} , Alan:{FieldName} Tablo:{TableName} ", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(v_UserField);
                    v_UserField = null;
                    GCClear();
                    return false;
                }
                else
                {
                    Application.SBO_Application.StatusBar.SetText("Tablo: " + TableName + " Alan: " + FieldName + " Eklendi", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(v_UserField);
                    v_UserField = null;
                    GCClear();
                    return true;
                }
                //}
                //else if (TableName.StartsWith("@") == false)
                //{
                //    SAPbobsCOM.UserFieldsMD v_UserField = (SAPbobsCOM.UserFieldsMD)Main.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                //    v_UserField.TableName = TableName;
                //    v_UserField.Name = FieldName;
                //    v_UserField.Description = FieldDescription;
                //    v_UserField.Type = type;

                //    if (ValidValues != null)
                //    {
                //        foreach (var item in ValidValues)
                //        {
                //            v_UserField.ValidValues.Value = item.Key;
                //            v_UserField.ValidValues.Description = item.Value;

                //            v_UserField.ValidValues.Add();
                //        }
                //    }

                //    if (type != SAPbobsCOM.BoFieldTypes.db_Date)
                //    {
                //        if (size != 0)
                //        {
                //            if (type == SAPbobsCOM.BoFieldTypes.db_Numeric)
                //            {
                //                //   v_UserField.EditSize = 11;
                //            }
                //            else
                //            {
                //                v_UserField.Size = (int)size;
                //                v_UserField.EditSize = (int)size;
                //            }
                //        }
                //    }
                //    if (subType != SAPbobsCOM.BoFldSubTypes.st_None)
                //    {
                //        v_UserField.SubType = subType;
                //    }
                //    if (!string.IsNullOrEmpty(LinkedTable))
                //    {
                //        v_UserField.LinkedTable = LinkedTable;
                //        v_UserField.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                //    }
                //    if (!string.IsNullOrEmpty(DefaultValue))
                //        v_UserField.DefaultValue = DefaultValue;

                //    RetVal = v_UserField.Add();
                //    if (RetVal != 0)
                //    {
                //        Main.oCompany.GetLastError(out ErrCode, out ErrMsg);
                //        Application.SBO_Application.StatusBar.SetText(" Hata Alınan Tablo : " + TableName + " Alan: " + FieldName + " Hata Kodu: " + ErrCode + " Hata Tanımı: " + ErrMsg, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                //        v_UserField = null;
                //        System.Runtime.InteropServices.Marshal.ReleaseComObject(v_UserField);
                //        GCClear();
                //        return false;
                //    }
                //    else
                //    {
                //        Application.SBO_Application.StatusBar.SetText("Tablo : " + TableName + " Alan : " + FieldName + " Başarıyla Eklendi!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                //        System.Runtime.InteropServices.Marshal.ReleaseComObject(v_UserField);
                //        v_UserField = null;
                //        GCClear();
                //        return true;
                //    }
                //}
                //else
                //    return false;
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(" Hata Alınan Tablo : " + TableName + " Alan: " + FieldName + " Hata Kodu: " + ErrCode + " Hata Tanımı: " + ErrMsg + " ex:"+ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return false;
            }
        }


        public static void GCClear()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
        }



        private static bool UDOExists(string code)
        {

            GC.Collect();
            SAPbobsCOM.UserObjectsMD v_udoMD = default(SAPbobsCOM.UserObjectsMD);
            v_udoMD = (SAPbobsCOM.UserObjectsMD)Main.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD);
            bool v_ReturnCode = false;
            v_ReturnCode = v_udoMD.GetByKey(code);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(v_udoMD);
            v_udoMD = null;
            GC.Collect();
            return v_ReturnCode;
        }
        public static void AddUDO(string UDOCode, string UDOName, string TableName, SAPbobsCOM.BoUDOObjType UDOType)
        {
            try
            {
                if (UDOExists(UDOCode))
                    return;
                SAPbobsCOM.UserObjectsMD oUserObjectMD = null;
                oUserObjectMD = ((SAPbobsCOM.UserObjectsMD)(Main.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD)));

                oUserObjectMD.CanCancel = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.CanClose = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.CanCreateDefaultForm = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanDelete = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.CanFind = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.CanYearTransfer = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.Code = UDOCode;
                oUserObjectMD.Name = UDOName;



                oUserObjectMD.ObjectType = UDOType;
                oUserObjectMD.ManageSeries = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.TableName = TableName;

                int RetVal = oUserObjectMD.Add();
                string ErrMsg = "";

                // check for errors in the process
                if (RetVal != 0)
                {
                    Main.oCompany.GetLastError(out RetVal, out ErrMsg);
                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ErrMsg);
                }
                else
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("UDO: " + oUserObjectMD.Name + " eklendi", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserObjectMD);
                oUserObjectMD = null;
                GCClear();
            }
            catch (Exception ex)
            {
                StatusBar.SetMessage(SAPbouiCOM.BoStatusBarMessageType.smt_Error, " UDO hata : " + ex.ToString());
            }
        }

    }
}