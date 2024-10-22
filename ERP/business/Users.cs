using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ERP;

public class BC_Users
{
    public static  object Login(string userName ,string password)
    {
        bool isLogin = false;
        string message = "登录失败！用户名或密码错误。";
        // 编写sql ，用传入的username 和 password 在 users 表中查询，返回查询到的记录数量
        StringBuilder sqlB = new StringBuilder();
        sqlB.AppendLine("SELECT ");
        sqlB.AppendLine("COUNT(UserID) AS Count ");
        sqlB.AppendLine("FROM users ");
        sqlB.AppendLine($"WHERE UserName = '{userName}'");
        sqlB.AppendLine($"AND EncryptedPassword = '{password}'");
        sqlB.AppendLine(";");

        // 执行 sql -> count
        int count = Convert.ToInt32(BC_MySqlUtils.ExecuteSQLGetScalar(sqlB.ToString()));
        // 判断 count
        // count == 1 登录成功
        if (count == 1)
        {
            isLogin = true;
            message = "登录成功！";
        }
        // count 不为1 登录失败
        // else
        // {
        //     isLogin = false;
        //     message = "登录失败！用户名或密码错误。";
        // }
        // 返回登录结果

        return new {
            LoginStatus = isLogin,
            Message = message
        };
    }
}