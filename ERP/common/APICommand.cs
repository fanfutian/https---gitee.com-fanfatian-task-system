using System.Text;
using Newtonsoft.Json.Linq;

namespace ERP
{
    public class BC_APICommand
    {
        private static JObject PostData;
        // 实现方法
        public static async Task ProcessAPIResult(HttpContext content, IApplicationBuilder builder)
        {
            string returnStr = "";
            // 获取POST参数
            string parameters = "{}";
            using (StreamReader sr = new StreamReader(content.Request.Body, Encoding.UTF8))
            {
                //string content = sr.ReadToEnd();  //.Net Core 3.0 默认不再支持
                parameters = sr.ReadToEndAsync().Result;
            }
            PostData = JObject.Parse(parameters);
            // 获取APICommand
            string APICommand = GetParameterByName("APICommand");
            switch (APICommand)
            {
                case "demo":
                    {
                        returnStr = BC_APIResult.GetAPIResult("", (int)BC_APIResultStatus.UN_KNOW, "示例请求");
                        break;
                    }
                // 登录 login
                case "login":
                    {
                        // 1. 获取 username 和 password 参数w
                        string userName = GetParameterByName("UserName");
                        string password = GetParameterByName("Password");
                        // 2. 调用登录方法 BC_Users.login 传入 username 和 password 并拿到返回结果
                        object loginResult = BC_Users.Login(userName, password);
                        // 3. 用BC_APIResult.GetAPIResult 格式化返回结果 并作为API的最终返回结果
                        returnStr = BC_APIResult.GetAPIResult(loginResult,(int)BC_APIResultStatus.SUCCESS,"Login API");
                        break;
                    }


                default:
                    {
                        returnStr = BC_APIResult.GetAPIResult("", (int)BC_APIResultStatus.FAIL, "请检查APICommand名称是否正确!");
                        break;
                    }
            }
            await content.Response.WriteAsync(returnStr);
        }
        // 根据名称获取string类型参数
        private static string GetParameterByName(string ParameterName)
        {
            return PostData[ParameterName].ToString();
        }
    }
}