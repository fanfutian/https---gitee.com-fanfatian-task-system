using System.Text;
using MySql.Data.MySqlClient;

namespace ERP;

public class BC_Tasks
{
    public struct TaskEntity
    {
        public int TaskID;
        public string Task;
        // AssignedToUserID 数据库字段
        public int AssignedToUserID;
        // AssignedToUser 前端展示字段
        public string AssignedToUser;
        public int TaskTypeID;
        public int TaskStatusID;
        public int ProjectID;
        public string UpdatedBy;
        public DateTime TimeUpdate;
        public string CreatedBy;
        public DateTime TimeCreated;

    }

    public static object TaskList(string taskID, string task, string assignedToUser)
    {
        // 返回值
        List<TaskEntity> taskList = new();
        // 1. 编写 查询 sql，执⾏ -> reader
        StringBuilder sqlB = new();
        sqlB.AppendLine("");
        sqlB.AppendLine("SELECT ");
        sqlB.AppendLine(" tasks.TaskID ");
        sqlB.AppendLine(" , tasks.Task ");
        sqlB.AppendLine(" , tasks.AssignedToUserID ");
        sqlB.AppendLine(" , IFNULL(users.nickName, users.username) AS AssignedToUser ");
       
        sqlB.AppendLine(" , tasks.TaskTypeID ");
        sqlB.AppendLine(" , tasks.TaskStatusID ");
        sqlB.AppendLine(" , tasks.ProjectID ");
        sqlB.AppendLine(" , tasks.UpdatedBy ");
        sqlB.AppendLine(" , tasks.TimeUpdate ");
        sqlB.AppendLine(" , tasks.CreatedBy ");
        sqlB.AppendLine(" , tasks.TimeCreated ");
        sqlB.AppendLine("FROM tasks ");
        sqlB.AppendLine("LEFT JOIN users ON tasks.AssignedToUserID = users.UserID ");

        sqlB.AppendLine("WHERE 1 ");
        if (!string.IsNullOrEmpty(taskID))
        {
            sqlB.AppendLine($"AND tasks.TaskID = '{taskID}' ");
        }
        if (!string.IsNullOrEmpty(task))
        {
            sqlB.AppendLine($"AND tasks.Task LIKE '%{task}%' ");
        }
        if (!string.IsNullOrEmpty(assignedToUser))
        {
            sqlB.AppendLine($"AND IFNULL(users.nickName, users.username) LIKE '%{assignedToUser}%' ");
        }

        sqlB.AppendLine(";");
        MySqlConnection connection = BC_MySqlUtils.GetMysqlConnection();
        using MySqlDataReader reader = BC_MySqlUtils.ExecuteSQLGetRS(sqlB.ToString(), connection);


        // 2. 遍历 reader，添加⾄返回值中
        while (reader.Read())
        {
            // 2.1 new taskEntity
            TaskEntity taskEntity = new()
            {
                TaskID = Convert.ToInt32(reader["TaskID"]),
                Task = reader["Task"].ToString(),
                AssignedToUserID = Convert.ToInt32(reader["AssignedToUserID"]),
           
            AssignedToUser = reader["AssignedToUser"].ToString(),
            TaskTypeID = Convert.ToInt32(reader["TaskTypeID"]),
            TaskStatusID = Convert.ToInt32(reader["TaskStatusID"]),
            ProjectID = Convert.ToInt32(reader["ProjectID"]),
            UpdatedBy = reader["UpdatedBy"].ToString(),
            TimeUpdate = Convert.ToDateTime(reader["TimeUpdate"]),
            CreatedBy = reader["CreatedBy"].ToString(),
                TimeCreated = Convert.ToDateTime(reader["TimeCreated"]),
            };
            // 2.2 添加⾄返回值 List 中
            taskList.Add(taskEntity);
        }
        // 3. 关闭 mysql 资源
        BC_MySqlUtils.CloseResource(connection, reader);
        // 4. 返回 TaskList
        return new
        {
            TaskList = taskList
        };
    }
}