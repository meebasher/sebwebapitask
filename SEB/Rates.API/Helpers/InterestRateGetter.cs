using Rates.API.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Rates.API.Helpers
{
    public class InterestRateGetter
    {
        /// <summary>
        /// Gets base rate code value asynchronously
        /// </summary>
        /// <param name="baseRateCode"></param>
        /// <returns>Base rate code value</returns>
        public static async Task<decimal> GetBaseRateValueAsync(string baseRateCode)
        {
            string uri = "http://www.lb.lt/webservices/VilibidVilibor/VilibidVilibor.asmx/getLatestVilibRate?RateType=" + baseRateCode;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await ParseBaseRateCodeAsync(reader.ReadToEndAsync().Result);
            }
        }

        /// <summary>
        /// Parses xml data that has a node with a base rate code value
        /// </summary>
        /// <param name="baseRateCodeResponse">Response from base rate code get requesr</param>
        /// <returns></returns>
        private static decimal ParseBaseRateCode(string baseRateCodeResponse)
        {
            decimal baseRateValue = default;
            bool isParsed = default;
            if (baseRateCodeResponse != null && baseRateCodeResponse != default) 
            {
                var doc = XDocument.Parse(baseRateCodeResponse);
                var value = doc.Root.DescendantNodes().OfType<XNode>().FirstOrDefault().ToString().Trim();
                isParsed = decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out baseRateValue);
            }
            if (baseRateValue==default)
            {
                throw new ArgumentNullException(nameof(baseRateValue));
            }
            else
            {
                return baseRateValue;
            }
        }
        /// <summary>
        ///  Parses xml data that has a node with a base rate code value asynchronously
        /// </summary>
        /// <param name="baseRateCodeResponse"></param>
        /// <returns></returns>
        private static async Task<decimal> ParseBaseRateCodeAsync(string baseRateCodeResponse)
        {
            var result = await Task.Run(() => ParseBaseRateCode(baseRateCodeResponse));
            return result;
        }

        /// <summary>
        /// Calculates Interet rate
        /// </summary>
        /// <param name="baseRateCode"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
        public static decimal CalculateInterestRate(string baseRateCode, decimal margin)
        {
            return GetBaseRateValueAsync(baseRateCode).Result + margin;
        }

    }
}
