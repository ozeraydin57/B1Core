using B1Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1Core.Business
{
    public static class UDO
    {

        public static Response<string> AddUDOData(string udo, string field, string value)
        {
            var resp = new Response<string>();

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;
            try
            {
                oCompanyService = Main.oCompany.GetCompanyService();
                // Get GeneralService (oCmpSrv is the CompanyService)
                oGeneralService = oCompanyService.GetGeneralService(udo);
                // Create data for new row in main UDO
                oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));
                oGeneralData.SetProperty(field, value);

                oGeneralParams = oGeneralService.Add(oGeneralData);
                string docEntry = System.Convert.ToString(oGeneralParams.GetProperty("DocEntry"));

                resp.Success = true;
                resp.Data = docEntry;
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
            }

            return resp;
        }



        public static Response<string> UpdateUDOData(string udo, string docEntry, string field, string value)
        {
            var resp = new Response<string>();

            try
            {
                SAPbobsCOM.GeneralService oGeneralService;
                SAPbobsCOM.GeneralData oGeneralData;
                SAPbobsCOM.GeneralDataParams oGeneralParams;

                SAPbobsCOM.CompanyService sCmp;
                sCmp = Main.oCompany.GetCompanyService();

                // Get a handle to the SM_MOR UDO
                oGeneralService = sCmp.GetGeneralService(udo);

                // Get UDO record
                oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("DocEntry", docEntry);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);

                // Update UDO record
                oGeneralData.SetProperty(field, value);
                oGeneralService.Update(oGeneralData);

                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
            }

            return resp;
        }
    }
}
