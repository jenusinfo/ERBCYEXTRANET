using CMS.Helpers;
using Eurobank.Models.Api;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

using System.Threading.Tasks;

namespace Eurobank.Helpers.External.File
{
	public class ExternalFileService
	{
        //public static readonly string _documentSaveApiUrl = ValidationHelper.GetString(AppSettings.Instance.Get<string>("MailConfig:Sender:Email"), "");
        public static DocumentUploadResponse SendFile(IFormFile file, string applicationNumber, string url)
		{
            DocumentUploadResponse retVal = new DocumentUploadResponse();

            string jsonResult = string.Empty;

			if(file != null)
			{
                //using(var client = new HttpClient())
                //{
                //    using(var content = new MultipartFormDataContent())
                //    {
                //        //byte[] Bytes = new byte[file.Length + 1];
                //        //file..Read(Bytes, 0, Bytes.Length);

                //        //using(var ms = new MemoryStream())
                //        //{
                //        //    file.CopyTo(ms);
                //        //    var fileBytes = ms.ToArray();
                //        //    string s = Convert.ToBase64String(fileBytes);
                //        //    // act on the Base64 data
                //        //    var fileContent = new ByteArrayContent(fileBytes);
                //        //    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
                //        //    content.Add(fileContent, "file");
                //        //    var requestUri = "https://localhost:44343/api/File/upload";
                //        //    var result = client.PostAsync(requestUri, content).Result;
                //        //    if(result.StatusCode == System.Net.HttpStatusCode.Created)
                //        //    {
                //        //        List<string> m = result.Content.ReadAsAsync<List<string>>().Result;
                //        //        //ViewBag.Success = m.FirstOrDefault();

                //        //    }
                //        //    else
                //        //    {
                //        //        //ViewBag.Failed = "Failed !" + result.Content.ToString();
                //        //    }
                //        //}

                        

                //    }
                //}

                using(var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(url);

                        byte[] data;
                        using(var br = new BinaryReader(file.OpenReadStream()))
                            data = br.ReadBytes((int)file.OpenReadStream().Length);

                        ByteArrayContent bytes = new ByteArrayContent(data);


                        MultipartFormDataContent multiContent = new MultipartFormDataContent();

                        multiContent.Add(bytes, "file", file.FileName);

                        var result = client.PostAsync("api/File/upload?applicationNumber=" + applicationNumber, multiContent).Result;
      //                  using(var client2 = new HttpClient())
						//{
      //                      client2.BaseAddress = new Uri("http://C1001CMSM1:8090/");
      //                      var result2 = client2.PostAsync("upload-file" + applicationNumber, multiContent).Result;
      //                  }
                            

                        if(result.StatusCode == System.Net.HttpStatusCode.OK)
						{
                            jsonResult = result.Content.ReadAsStringAsync().Result;
                            if(jsonResult != null)
							{
                                retVal = JsonConvert.DeserializeObject<DocumentUploadResponse>(jsonResult);
        //                        if(retVal != null && !retVal.IsSuccess)
								//{
        //                            eventLogService.LogException("ExternalFileService", "SendFile", documentUploadResult.ErrorMessage);
        //                        }
                            }
                        }
						else
						{
							//ViewBag.Failed = "Failed !" + result.Content.ToString();
						}

						//return StatusCode((int)result.StatusCode); //201 Created the request has been fulfilled, resulting in the creation of a new resource.

					}
					catch(Exception ex)
                    {
                        //return StatusCode(500); // 500 is generic server error
                    }
                }
            }

			return retVal;
		}

        public static byte[] DowloadFile(string fileGuid, string applicationNumber, string fileName, string url)
		{
            bool retVal = true;

            if(!string.IsNullOrEmpty(fileGuid) && !string.IsNullOrEmpty(applicationNumber) && !string.IsNullOrEmpty(fileName))
			{
                using(HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);

                    var response = client.GetAsync(string.Format("api/File/download/{0}/{1}/{2}", fileGuid, applicationNumber, fileName) ).Result;
                    if(response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsByteArrayAsync().Result;//Here is the problem
                        return result;
                    }
                }
            }

            return null;
		}

        public static string GetMIMEType(string fileName)
        {
            var provider =
                new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            string contentType;
            if(!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

    }
}
