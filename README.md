# Pokedex
### A Pokdex based on the Restful API

## About the project
This repo exposes a pair of Restful endpoints to output any requested Pokemon basic information either by their original or funny translated description.
## Dependencies
- [.NET 5.0](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
- [ASP.NET Web APIs](https://dotnet.microsoft.com/en-us/apps/aspnet/apis)

## How to run

- Clone the repo (`https://github.com/Mostafa-Armandi/Pokedex.git`)
- Go to the `/Pokedex` path through console and run `dotnet run	`.
    - `Dockerfile` is also provided
- Application is exposed on the following URLs:
    - https://localhost:5001/
    - http://localhost:5000/
- The following endpoints can be queries with any desired pokemon `name`:
    - `/pokemon/{name}` e.g [ditto](https://localhost:5001/pokemon/ditto)
    - `/pokemon/translated/{name}` e.g [ditto](https://localhost:5001/pokemon/translated/ditto)
- The application is documented and OpenApi enabled through Swagger accesible from the following address:
    - [`/swagger/index.html`](https://localhost:5001/swagger/index.html)


## Production-ready suggestions

- Handle partial failure for the underlying third-party APIs applying:
    - Retries with exponential backoff
    - Circuit Breaker pattern
      
      [Polly](https://github.com/App-vNext/Polly) is a great library to implement the above patterns.
- Provide links to support HATEOAS-style navigation
- Support client-side caching
- Use API versioning to facilitate API schema and behavior change over the time