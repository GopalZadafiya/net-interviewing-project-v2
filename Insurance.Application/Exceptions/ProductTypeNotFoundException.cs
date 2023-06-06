using System;

namespace Insurance.Application.Exceptions
{
    public class ProductTypeNotFoundException: Exception
    {
        public ProductTypeNotFoundException() { }

        public ProductTypeNotFoundException(string message) : base(message) { }
    }
}
