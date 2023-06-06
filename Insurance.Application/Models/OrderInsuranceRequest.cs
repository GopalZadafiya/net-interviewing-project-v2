using System.Collections.Generic;

namespace Insurance.Application.Dto
{
    public class OrderInsuranceRequest
    {
        public IList<ProductInsuranceResponse> ProductInsurances { get; set; } 
            = new List<ProductInsuranceResponse>();
    }
}
