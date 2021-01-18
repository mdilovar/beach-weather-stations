# beach-weather-stations
## How to run:
Check out the repo and open the solution in an IDE that supports .NET (Rider, Visual Studio, etc.)
Open the Test.cs file and run the tests there. Most IDEs will recognize that this is a test project and give you shortcuts to run the tests in the file.
## Possible improvements that I did not get into:
1. Refactoring common items from the test (e.g. the rest client, the base URL, the resource path, etc.)
2. Adding support for logging. More on this below if you are interested.
3. I am sure there are other potential improvements possible such as using a BDD framework like Serenity instead of NUnit, etc.
# An idea for adding logging support:
One way would be to add a proxy class around RestClient that logs all requests and responses. When we use this proxy class instead of the bare RestClient class, the tests would produce a log of all the requests and responses which can be very useful. For now, only some assertion logging is done. Here is a sample proxy class I have used in the past:
```
public class LoggingRestClient : RestClient
    {
        protected readonly ILogger _logger;

        public LoggingRestClient(string baseUrl, ILogger logger) : base(baseUrl)
        {
            _logger = logger;
        }

        public LoggingRestClient(Uri baseUrl, ILogger logger) : base(baseUrl)
        {
            _logger = logger;
        }

        public LoggingRestClient(ILogger logger) : base()
        {
            _logger = logger;
        }

        public override IRestResponse Execute(IRestRequest request, Method httpMethod)
        {
            LogRequest(request);
            var response = base.Execute(request, httpMethod);
            LogResponse(response);
            return response;
        }

        public override IRestResponse Execute(IRestRequest request)
        {
            LogRequest(request);
            var response =  base.Execute(request);
            LogResponse(response);
            return response;
        }

        protected virtual void LogRequest(IRestRequest request)
        {
            var requestToLog = new
            {
                resource = request.Resource,
                parameters = request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                method = request.Method.ToString(),
                uri = BuildUri(request),
            };

            _logger.Info($"Request: {JsonConvert.SerializeObject(requestToLog, Formatting.Indented)}");
        }

        protected virtual void LogResponse(IRestResponse response)
        {
            var responseToLog = new
            {
                statusCode = response.StatusCode,
                content = response.Content,
                headers = response.Headers,
                responseUri = response.ResponseUri,
                errorMessage = response.ErrorMessage,
            };

            _logger.Info($"Response: {JsonConvert.SerializeObject(responseToLog, Formatting.Indented)}");
        }

    }
```
