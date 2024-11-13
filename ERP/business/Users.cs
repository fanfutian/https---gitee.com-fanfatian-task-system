using System.Security.Cryptography.X509Certificates;
using System.Text;
using MySql.Data.MySqlClient;

namespace ERP;

public class BC_Users
{

    public static List<BC_UserEntity> GetUserList()
    {
        List<BC_UserEntity> userEntityList = new();
        StringBuilder sqlB = new();
        sqlB.AppendLine("SELECT");
        sqlB.AppendLine(" users.UserID");
        sqlB.AppendLine(" , users.UserName");
        sqlB.AppendLine(" , users.NickName");
        sqlB.AppendLine("FROM users");
        sqlB.AppendLine(";");
        MySqlConnection conn = BC_MySqlUtils.GetMysqlConnection();
        MySqlDataReader reader = BC_MySqlUtils.ExecuteSQLGetRS(sqlB.ToString(), conn);
        BC_UserEntity userEntity = new();
        while (reader.Read())
        {
            userEntity.UserID = Convert.ToInt32(reader["UserID"]);
            userEntity.UserName = reader["UserName"].ToString();
            userEntity.NickName = reader["NickName"].ToString();
            userEntityList.Add(userEntity);
        }
        BC_MySqlUtils.CloseResource(conn, reader);
        return userEntityList;
    }

    public struct BC_UserEntity
    {
        // `UserID`, `UserName`, `NickName`
        public int UserID;
        public string UserName;
        public string NickName;
    }

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


    public static object Register(string username, string password, string nickName, string email, string phone){
        
        bool isRegister = false;
        string message = "注册失败！";
        
        StringBuilder sqlB = new StringBuilder();
        sqlB.AppendLine("SELECT ");
        sqlB.AppendLine("COUNT(UserID) AS Count ");
        sqlB.AppendLine("FROM users ");
        sqlB.AppendLine($"WHERE UserName = '{username}'");
        sqlB.AppendLine(";");
        int count = Convert.ToInt32(BC_MySqlUtils.ExecuteSQLGetScalar(sqlB.ToString()));
        
        Console.WriteLine($"count:{count}");
        if (count > 0){
            message += $"用户名{username}已被使用！";
        }else{
            sqlB.Length = 0;
            sqlB.AppendLine("INSERT INTO `db_task`.`users` ");
            sqlB.AppendLine("( ");
            sqlB.AppendLine(" `UserName` ");
            sqlB.AppendLine(" , `NickName` ");
            sqlB.AppendLine(" , `RoleID` ");
            sqlB.AppendLine(" , `Email` ");
            sqlB.AppendLine(" , `Phone` ");
            sqlB.AppendLine(" , `EncryptedPassword` ");
            sqlB.AppendLine(" , `Status` ");
            sqlB.AppendLine(" , `UpdatedBy` ");
            sqlB.AppendLine(" , `TimeUpdate` ");
            sqlB.AppendLine(" , `CreatedBy` ");
            sqlB.AppendLine(" , `TimeCreated` ");
            sqlB.AppendLine(") ");
            sqlB.AppendLine("VALUES ");
            sqlB.AppendLine("( ");
            sqlB.AppendLine($" '{username}'");
            sqlB.AppendLine($", '{nickName}'");
            sqlB.AppendLine(" , 1 ");
            sqlB.AppendLine($" , '{email}'");
            sqlB.AppendLine($" , '{phone}'");
            sqlB.AppendLine($" , '{password}'");
            sqlB.AppendLine(" , 1 ");
            sqlB.AppendLine(" , 1 ");
            sqlB.AppendLine(" , NOW() ");
            sqlB.AppendLine(" , 1 ");
            sqlB.AppendLine(" , NOW() ");
            sqlB.AppendLine(" ); ");
            int result = BC_MySqlUtils.ExecuteSQL(sqlB.ToString());

            if (result > 0){
                isRegister = true;
                message = "注册成功!";
            }
            else{
                message += "请联系管理员！";
            }
        }
        return new{
            RegisterStatus = isRegister,
            Message = message
        };
    }
}