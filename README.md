# Address Book API Challenge

A minimal Address Book API built using ASP.NET Core 8 to demonstrate backend engineering practices such as authentication wiring, validation, error handling, logging, OpenAPI documentation, and testing.

The API allows authenticated users to manage their own UK addresses.

# Features

- Create a new address

- Update an existing address

- Delete an address

- List all addresses for the authenticated user

- Retrieve a specific address

- JWT Bearer authentication wiring

- Input validation ,Global error handling & Structured logging

- Swagger/OpenAPI documentation

- Unit testing with xUnit


# Project Structure

AddressBookChallenge
│
├── Controllers
│   ├── AddressesController.cs
│   └── DevelopmentController.cs
│
├── Authentication
│   ├── CurrentUserService.cs
│   └── DevelopmentTokenService.cs
│
├── Infrastructure
│   ├── AddressValidator.cs
│   ├── ExceptionHandlingMiddleware.cs
│   ├── NotFoundException.cs
│   └── ValidationException.cs
│
├── Models
│   ├── UkAddress.cs
│   ├── AddressRecord.cs
│   ├── CreateAddressRequest.cs
│   ├── UpdateAddressRequest.cs
│   ├── AddressResponse.cs
│   ├── JwtOptions.cs
│   └── ErrorResponse.cs
│
├── Repositories
│   ├── IAddressRepository.cs
│   └── InMemoryAddressRepository.cs
│
├── Services
│   ├── IAddressService.cs
│   └── AddressService.cs
│
├── Tests
│   ├── AddressServiceTests.cs
│   └── Tests.csproj
│
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── AddressBookChallenge.csproj

# API Endpoints

    GET     /                     → Health check endpoint
    GET     /dev/token/{userId}   → Generate development JWT token  
    POST    /api/addresses        → Create a new address
    GET     /api/addresses        → List all addresses for the authenticated user
    GET     /api/addresses/{id}   → Retrieve a specific address
    PUT     /api/addresses/{id}   → Update an existing address
    DELETE  /api/addresses/{id}   → Delete an address

# API Execution Flow

1. Start the application
   - dotnet restore
   - dotnet build
   - dotnet run

2. Open Swagger UI:
   https://localhost:<port>/swagger

3. Verify the API is running
   GET /

4. Generate a development token
   GET /dev/token/user1

5. Copy the returned JWT token

6. Click "Authorize" in Swagger

7. Paste the token as:
   Bearer <token>

8. Create an address
   POST /api/addresses

9. Retrieve all addresses for the authenticated user
   GET /api/addresses

10. Retrieve a specific address using its generated id
    GET /api/addresses/{id}

11. Update an existing address
    PUT /api/addresses/{id}

12. Delete an address
    DELETE /api/addresses/{id}

13. Test user isolation
    - Generate another token:
      GET /dev/token/user2
    - Authorize with the new token
    - Call:
      GET /api/addresses
    - Verify that user2 cannot see user1’s addresses


