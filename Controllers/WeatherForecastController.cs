using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using TestBlazorGridEnumJson.Services;

namespace TestBlazorGridEnumJson.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly WeatherForecastService _service;

    public WeatherForecastController(WeatherForecastService service)
    {
        _service = service;
    }

    [HttpGet("Grid")]
    public async Task<LoadResult> GetWeatherForecasts(DataSourceLoadOptions loadOptions)
    {
        var data1 = await _service.GetForecastAsync(DateOnly.FromDateTime(DateTime.Now.Date));

        var data = data1.Select(x => new WeatherForecast
        {
            Date = x.Date,
            TemperatureC = x.TemperatureC,
            Summary = x.Summary,
            DriveType = x.DriveType
        });

        return DataSourceLoader.Load(data, loadOptions);
    }
}
