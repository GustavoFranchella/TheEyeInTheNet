using System;
using System.Net.Http;
using System.Threading.Tasks;
using CAMMESA_API;
using Newtonsoft.Json;


try
{
    // Create a HttpClient object
    HttpClient client = new HttpClient();

    // Set the base address of the API
    client.BaseAddress = new Uri("https://api.cammesa.com/demanda-svc/");

    // Set the accept header to JSON
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

    // Define the id_region parameter
    int id_region = 1002; // Example value for Total del SADI

    // Build the request URL with the parameter
    string requestUrl = $"generacion/ObtieneGeneracioEnergiaPorRegion?id_region={id_region}";

    // Send the GET request and get the response
    HttpResponseMessage response = await client.GetAsync(requestUrl);

    // Check if the response is successful
    if (response.IsSuccessStatusCode)
    {
     // Read the response content as a string
        string content = await response.Content.ReadAsStringAsync();
    /* Beginning of original code
    // Print the content to the console
        Console.WriteLine(content);
    // End of original code */
    // Beginning of new code
        // Deserialize the JSON into an array of objects
        var dataArray = JsonConvert.DeserializeObject<Data[]>(content);

        // Get the last element (assuming the array is not empty)
        var lastElement = dataArray![dataArray.Length - 1];
        var carbonIntensity = lastElement.termico / (lastElement.sumTotal - lastElement.importacion);
        // Parse the input string
        DateTime date = DateTime.ParseExact(lastElement.fecha, "yyyy-MM-ddTHH:mm:ss.fffzzz", null);
        // Format the DateTime object
        string formattedDate = date.ToString("yyyy-MM-dd HH:mm:ss");

        
        Console.WriteLine ("Fecha y hora: " + formattedDate + " | Intensidad de carbono últimos 5 minutos: " + Math.Round(carbonIntensity, 3) + " (No incluye importación)");
    // End of new code
    }
    else
    {
        // Print the status code and reason phrase to the console
        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
    }

    // Dispose the client object
    client.Dispose();
}
catch (Exception ex)
{
    Console.WriteLine($"Exception occurred: {ex.Message}");
}

// Beginning of new definition
namespace CAMMESA_API
{
// Define a class to match the structure of the JSON objects
    class Data
    {
    public required string fecha { get; set; }
    public double sumTotal { get; set; }
    public double hidraulico { get; set; }
    public double termico { get; set; }
    public double nuclear { get; set; }
    public double renovable { get; set; }
    public double importacion { get; set; }

    }
    // End of new definition
}

