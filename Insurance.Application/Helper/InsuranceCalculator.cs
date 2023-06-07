using Insurance.Domain.Enums;

namespace Insurance.Application.Helper
{
    /// <summary>
    /// Helper class to calculate insurance amount
    /// </summary>
    public static class InsuranceCalculator
    {
        private const float DELICATE_INSURANCE_AMOUNT = 500;

        /// <summary>
        /// Get insurance amount by price of product
        /// </summary>
        /// <param name="salesPrice"></param>
        /// <returns></returns>
        public static float GetBySalesPrice(float salesPrice)
        {
            float baseInsurance = 0;

            switch (salesPrice)
            {
                case float p when p < 500:
                    baseInsurance = 0;
                    break;

                case float p when p >= 500 && p < 2000:
                    baseInsurance = 1000;
                    break;

                case float p when p >= 2000:
                    baseInsurance = 2000;
                    break;
            }

            return baseInsurance;
        }

        /// <summary>
        /// Get additional insurance amount for specific products
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns></returns>
        public static float GetByProductType(int productTypeId)
        {
            float extraInsurance = 0;

            switch ((ProductTypes)productTypeId)
            {
                case ProductTypes.Laptops:
                case ProductTypes.SmartPhones:
                    extraInsurance = DELICATE_INSURANCE_AMOUNT;
                    break;
            }

            return extraInsurance;
        }
    }
}
