# Insurance Api

## Task 1
 - The Issue was in if condition. Because of outer if/else logic to add addtional 500 was not executed at all.

## Task 2
  **Refactoring**
  - Added support for swagger
  -  Renamed `HomeController` to `InsuranceController`
  -  Seperated BusinessRule into following based on responsibilities:
     - `InsuranceService` (calulates insurance amount based on product type, price etc.)
     - `ProductService` (retrives data from external product api)
  - Updated code to use diffrent models over request and reponse
  - Added validation for insurance calculation
  - Added global exception handling and logging
  - Added `ApiClient` to communicate over external api.
  - Created static `InsuranceCalculator` helper which returns amount of insurance based on product type, price etc.
  - Removed unused references
  - Upgraded Newtonsoft.Json package

**Assumption/Decision Made & Reason**
 - Following Clean Architecture, added 3 new projects
    - `Insurance.Application` --> Independanct of database, external services. It contains all business logic and can be tested well  
    - `Insurance.Domain` --> Where all application related entities resides
    - `Insurance.Infrastructure` --> Where communication to external services/frameworks resides
 - Added ProductService in Insurance.Infrastructure --> It's different api to which we don't have control over. we just communicates to retrieve product data. Therefore its ideal to put it in Infrastructure. It doesn't affect even insurance calculation even if in future something changes over product api. 
 - Using EFCore Inmemory db -  I find it easier to configure & use. That helps to focus on core functionality implementation.  

## Task 3
- Created an endpoint which calculates and return insurance for an order by passing product ids
- Created a service method to calculate insurance which reuses the existing `GetInsuranceAsync(Product)` method
- Added unit tests

## Task 4
- Added a step in `GetInsuranceAsync(Order)` to calculate addtional insurance when product belongs to certain product type(Digital Cameras)
- Adjusted unit tests

## Task 5
- Added endpoints to retrieve and add surcharge rates for product types 
- Added unit tests