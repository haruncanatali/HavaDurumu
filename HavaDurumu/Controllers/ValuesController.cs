using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HavaDurumu.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace HavaDurumu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IHostingEnvironment Environment;
        private IOptions<ApiAuth> myOptions;

        public ValuesController(IHostingEnvironment x,IOptions<ApiAuth> y)
        {
            this.Environment = x;
            this.myOptions = y;
        }

        [HttpGet]
        [Route("weather")]
        public IActionResult GetWeatherForecast(string cityName)
        {
            Normalization(ref cityName);

            if (String.IsNullOrEmpty(cityName))
                return Ok(new string[] { "null" });

            string token = "", url = "";

            GetValuesForAPI(ref token,ref url,cityName);

            try
            {
                var request = System.Net.WebRequest.Create(url);
                if (request != null)
                {
                    request.Method = "GET";
                    request.Headers.Add("authorization", token);
                    request.Headers.Add("content-type", "application/json");

                    using (Stream stream = request.GetResponse().GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var jResponse = reader.ReadToEnd();
                            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<Response>(jResponse);
                            if (response == null)
                                return Ok(new string[] { "null" });
                            response = StatusToTr(response);
                            response = ConfigureDegress(response);
                            return Ok(response);
                        }
                    }
                }
                else
                {
                    return Ok(new string[] { "null" });
                }
            }
            catch (Exception e)
            {
                return Ok(new string[] { "null" });
            }
        }

        [HttpGet]
        [Route("cities")]
        public IActionResult GetCities()
        {
            List<Il> ilList = new List<Il>();
            using (StreamReader reader = new StreamReader(this.Environment.WebRootPath+"\\il.json"))
            {
                var jsonBody = reader.ReadToEnd();
                ilList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Il>>(jsonBody);
            }
            return Ok(ilList);
        }

        private Response ConfigureDegress(Response model)
        {
            foreach (var item in model.Result)
            {
                item.Degree = SetDegree(item.Degree);
                item.Max = SetDegree(item.Max);
                item.Min = SetDegree(item.Min);
                item.Night = SetDegree(item.Night);
            }

            return model;
        }

        private double SetDegree(double val)
        {
            if (val<0)
            {
                return int.Parse(Math.Floor(val).ToString());
            }
            else if (val > 0)
            {
                return int.Parse(Math.Ceiling(val).ToString());
            }
            else
            {
                return 0;
            }
        }

        private Response StatusToTr(Response response)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Rain","Yağmurlu");
            dict.Add("Snow", "Karlı");
            dict.Add("Wind", "Rüzgarlı");
            dict.Add("Cloudy", "Bulutlu");
            dict.Add("Clouds", "Bulutlu");
            dict.Add("Windy","Rüzgarlı");
            dict.Add("Hot","Sıcak");
            dict.Add("Cold","Soğuk");
            dict.Add("Sun","Güneşli");
            dict.Add("Sunny","Güneşli");
            dict.Add("Typhoon", "Tayfun");
            dict.Add("Storm", "Fırtına");
            dict.Add("Hurricane ", "Kasırga");
            dict.Add("Tornado", "Kasırga");
            dict.Add("Foggy", "Sisli");
            dict.Add("Sleet", "Sulu Kar");
            dict.Add("Hail", "Dolu");
            dict.Add("Clear", "Açık");

            foreach (var item in response.Result)
            {
                foreach (var itemx in dict)
                {
                    if (itemx.Key == item.Status)
                    {
                        item.Status = itemx.Value;
                    }   
                }
            }

            return response;
        }

        private void GetValuesForAPI(ref string token,ref string url,string cityName)
        {
            token = myOptions.Value.Token;
            url = myOptions.Value.Url + cityName;
        }
        
        private void Normalization(ref string cityName)
        {
            if (String.IsNullOrEmpty(cityName))
                return;
            try
            {
                Dictionary<char, char> tr = new Dictionary<char, char>();
                tr.Add('ı','i');
                tr.Add('ü','u');
                tr.Add('ö','o');
                tr.Add('ç','c');
                tr.Add('ğ','g');
                tr.Add('ş','s');

                var temp = cityName.ToLower().ToCharArray();

                for (int i = 0; i < temp.Length; i++)
                {
                    foreach (var item in tr)
                    {
                        if (item.Key == temp[i])
                        {
                            temp[i] = item.Value;
                            break;
                        }
                    }
                }

                cityName = new string(temp);
            }
            catch (Exception e)
            {
                cityName = "";
            }
        }

    }
}
