# Lykke Template for Internal API
This template should be used for internal API development

## Getting Started

### Installation
To install this template run the following command

```ps
dotnet new -i Lykke.Template::*
```

If the template is changed, you can run above command again to update local installation.

### Usage
To create a new solution:

* navigate to the folder where solution folder should be created

* run the following command:
    ```
    dotnet new LykkeTemplate -n Lykke.Assets -s Assets
    ```
    `-n` is the parameter for the name of the new _folder_, _solution_ and the root _namespace_.

    `-s` is the short name for some functions used in the generated code (ex: `serivces.AddAssetsRepository()`).

    > to list all available command parameters run:
    > ```
    > dotnet new LykkeTemplate -h
    > ```

This will create a folder with a solution named `Lykke.Assets` with 4 projects.

## Project Structure

### 1. Abstractions Project
This project should contain only functionality definition abstracted from implementation details.
Usually contains `interface` files.

> example `ISamplesRepository`:
> ```cs
> namespace Lykke.Template.Abstractions
> {
>     public interface ISample
>     {
>         string Id { get; }
>         string Name { get; }
>         string Description { get; }
>     }
> 
>     public class Sample : ISample
>     {
>         //interface properties and static mapping methods
>     }
> 
>     public interface ISamplesRepository
>     {
>         Task InsertAsync(ISample model);
>         Task UpdateAsync(ISample model);
>         Task<ISample> GetAsync(string id);
>         Task<IEnumerable<ISample>> GetAsync();
>     }
> }
> ```

### 2. Azure Project
This project should contain _Azure_ implementations for definitions from _Abstractions Project_.

> example `SamplesRepository`:
> ```cs
> namespace Lykke.Template.Azure
> {
>     public class SampleEntity : TableEntity, ISample
>     {
>         //interface properties and static mapping methods
>     }
> 
>     public class SamplesRepository : ISamplesRepository
>     {
>         public Task InsertAsync(ISample model)
>         {
>             var entity = SampleEntity.Map(model);
>             return _tableStorage.InsertAsync(entity);
>         }
> 
>         //implementations of other methods
>     }
> }
> ```

**Project Dependencies**:
* _Abstractions Project_

**NuGet dependencies**:
* _Lykke.AzureStorage_: for Azure storage data manupulations (persisting and retriving)

### 3. WebApi Project
This project should contain controllers to expose functionality as a Rest service.

> ```cs
> example `SamplesController`:
> namespace Lykke.Template.WebApi.Controllers
> {
>     [Authorize]
>     [Route("api/[controller]")]
>     public class SamplesController : Controller
>     {
>         [HttpPost]
>         public Task<IActionResult> InsertAsync([FromBody] Sample model)
>         {
>             return _samplesRepository
>                 .InsertAsync(model)
>                 .ToActionResult();
>         }
>         
>         //implementations of other methods
>     }
> }

**Project Dependencies**:
* _Abstractions Project_
* _Azure Project_

**NuGet dependencies**:
* _Lykke.ApiAuth.Azure_: Api Authentication/Authorization implementation on Azure
* _Lykke.ApiAuth.Mvc_: MVC Authentication MiddleWare and other extensions.
* _Lykke.Extensions_: For reading settings from URL
    > Usage:
    > ```cs
    > var builder = new ConfigurationBuilder()
    >     .AddFromConfiguredUrl("TEMPLATE_API_SETTINGS_URL");
    > Configuration = builder.Build();
    >
    > var apiAzureConfig = Configuration
    >     .GetSection("LykkeApiAuth")
    >     .Get<ApiAuthAzureConfig>();
    > ```
    > `TEMPLATE_API_SETTINGS_URL` sould be configured to the settings url.
* _Lykke.Http_: Common classes to be serialized between `api` and `api client`

### 4. WebClient Project
This project should contain proxy classes, implementing interfaces from _Abstractions Project_ by sending inputs to _WebApi_ rest service, and returning results.

> example `SamplesRepositoryClient`:
> ```cs
> namespace Lykke.Template.WebClient
> {
>     public class SamplesRepositoryClient : > ISamplesRepository
>     {
>         private readonly RestClient _restClient;
>         private const string Endpoint = "api/samples";
> 
>         public Task InsertAsync(ISample model)
>         {
>             return _restClient.PostAsync($"{Endpoint}", model);
>         }
> 
>         //implementations of other methods
>     }
> }
> ```

## Additional Notes
### Exception ahndling:
Exceptions are being simplified and serialized and returned as rest result.
> 400 BadRequest
> 
> ```json
> {
>   "message": "Conflict",
>   "innerException": null
> }
> ```

When rest client gets `400 - BadRequest` result, it tries to deserialize it as an exception and if is succeeds throws new exception, so we can use `try / catch` on the client side, and will get the error thrown in API.

> For exapmle if we try to insert an item with an already existing Id, we will get Exception with "Conflict" message at client side.
> 
> ```cs
> var services = new ServiceCollection()
>     .AddTemplateWebClient(/* */)
>     .BuildServiceProvider();
> 
> var samplesService = services.GetService<ISamplesRepository>();
> 
> try
> {
>     await samplesService.InsertAsync(item);
>     Console.WriteLine("Added sample");
> }
> catch (Exception e)
> {
>     Console.WriteLine(e);
> }
> ```