using System;

namespace Publicis.ReportHub.Framework.DTO
{
    [Serializable]
    public class GeneratorValuationInput
    {
        public GeneratorValuationInput()
        {
        }

        public GeneratorValuationInput(string col)
        {
            string[] colarray = col.Split(",");

            if (colarray.Length < 12)
            {
                TradeParty1TransactionId = colarray[0];
                
                TradeParty1ValuationAmount = colarray[1];

                TradeParty1ValuationCurrency = colarray[2];
                
                TradeParty1ValuationDatetime = colarray[3];

                TradeParty1ValuationType = colarray[4];
                
                CCPMTMValue = colarray[5];

                CCPMTMCurrency = colarray[6];
                
                CCPValuationDateTime = colarray[7];

                CCPValuationTypeParty1 = colarray[8];

                ClearingStatus = colarray[9];

                TradeRegulatorNames = new string[] { colarray[10] };
            }
            else 
            { 
                throw new Exception("Not Well formatted column");
            }
        }

        public string TradeParty1TransactionId { get; set; }

        public string TradeParty1ValuationAmount { get; set; }

        public string TradeParty1ValuationCurrency { get; set; }

        public string TradeParty1ValuationDatetime { get; set; }

        public string TradeParty1ValuationType { get; set; }

        public string CCPMTMValue { get; set; }

        public string CCPMTMCurrency { get; set; }

        public string CCPValuationDateTime { get; set; }

        public string CCPValuationTypeParty1 { get; set; }

        public string ClearingStatus { get; set; }

        public string[] TradeRegulatorNames { get; set; }
    }
}