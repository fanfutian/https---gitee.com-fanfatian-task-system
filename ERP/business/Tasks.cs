using System.Text;
using MySql.Data.MySqlClient;
using static ERP.BC_Users;

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
        public string TaskType;
        public string TaskStatus;
        public int TaskStatusID;
        public int ProjectID;
        public string UpdatedBy;
        public DateTime TimeUpdate;
        public string CreatedBy;
        public DateTime TimeCreated;

    }


    public struct BC_TaskTypeEntity
    {
        // `TaskTypeID`, `TaskType`
        public int TaskTypeID;
        public string TaskType;
    }

    public struct BC_TaskStatusEntity
    {
        // `TaskStatusID`, `TaskStatus`
        public int TaskStatusID;
        public string TaskStatus;
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
        sqlB.AppendLine(" , task_types.TaskType ");
        sqlB.AppendLine(" , tasks.TaskStatusID ");
        sqlB.AppendLine(" , task_status.TaskStatus ");
        sqlB.AppendLine(" , tasks.ProjectID ");
        sqlB.AppendLine(" , tasks.UpdatedBy ");
        sqlB.AppendLine(" , tasks.TimeUpdate ");
        sqlB.AppendLine(" , tasks.CreatedBy ");
        sqlB.AppendLine(" , tasks.TimeCreated ");
        sqlB.AppendLine("FROM tasks ");
        sqlB.AppendLine("LEFT JOIN users ON tasks.AssignedToUserID = users.UserID ");
        sqlB.AppendLine("LEFT JOIN task_types ON tasks.TaskTypeID = task_types.TaskTypeID ");
        sqlB.AppendLine("LEFT JOIN task_status ON tasks.TaskStatusID = task_status.TaskStatusID ");
        

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
                TaskType = reader["TaskType"].ToString(),
                TaskStatusID = Convert.ToInt32(reader["TaskStatusID"]),
                TaskStatus = reader["TaskStatus"].ToString(),
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



    public static object GetAddTaskSelectListData()
    {
        // 1. 获取 users 表数据
        List<BC_UserEntity> userList = BC_Users.GetUserList();
        // 2. 获取 task_types 表数据
        List<BC_TaskTypeEntity> taskTypeList = BC_TaskTypes.GetTaskTypeList();
        // 3. 获取 task_status 表数据
        List<BC_TaskStatusEntity> taskStatusList = BC_TaskStatus.GetTaskStatuList();
        return new
        {
            UserList = userList,
            TaskTypeList = taskTypeList,
            TaskStatusList = taskStatusList
        };
    }

    public class BC_TaskStatus
    {
        public static List<BC_TaskStatusEntity> GetTaskStatuList()
        {
            List<BC_TaskStatusEntity> taskStatusEntityList = new();
            StringBuilder sqlB = new();
            sqlB.AppendLine("SELECT");
            sqlB.AppendLine(" task_status.TaskStatusID");
            sqlB.AppendLine(" , task_status.TaskStatus");
            sqlB.AppendLine("FROM task_status");
            sqlB.AppendLine(";");
            MySqlConnection conn = BC_MySqlUtils.GetMysqlConnection();
            MySqlDataReader reader = BC_MySqlUtils.ExecuteSQLGetRS(sqlB
           .ToString(), conn);
            BC_TaskStatusEntity taskStatusEntity = new();
            while (reader.Read())
            {
                taskStatusEntity.TaskStatusID = Convert.ToInt32(reader[
               "TaskStatusID"]);
                taskStatusEntity.TaskStatus = reader["TaskStatus"].ToString();
                taskStatusEntityList.Add(taskStatusEntity);
            }
            BC_MySqlUtils.CloseResource(conn, reader);
            return taskStatusEntityList;
        }
    }

    public class BC_TaskTypes
    {
        public static List<BC_TaskTypeEntity> GetTaskTypeList()
        {
            List<BC_TaskTypeEntity> taskTypeEntityList = new();
            StringBuilder sqlB = new();
            sqlB.AppendLine("SELECT");
            sqlB.AppendLine(" task_types.TaskTypeID");
            sqlB.AppendLine(" , task_types.TaskType");
            sqlB.AppendLine("FROM task_types");
            sqlB.AppendLine(";");
            MySqlConnection conn = BC_MySqlUtils.GetMysqlConnection();
            MySqlDataReader reader = BC_MySqlUtils.ExecuteSQLGetRS(sqlB
           .ToString(), conn);
            BC_TaskTypeEntity taskTypeEntity = new();
            while (reader.Read())
            {
                taskTypeEntity.TaskTypeID = Convert.ToInt32(reader["TaskTypeID"]);
               
                taskTypeEntity.TaskType = reader["TaskType"].ToString();
                taskTypeEntityList.Add(taskTypeEntity);
            }
            BC_MySqlUtils.CloseResource(conn, reader);
            return taskTypeEntityList;
        }
    }

    public static object AddTask(TaskEntity taskEntity)
    {
        bool addTaskStatus = false;
        string message = "新增失败！请联系管理员。";
        StringBuilder sqlB = new();
        sqlB.AppendLine("INSERT INTO `tasks` ");
        sqlB.AppendLine("( ");
        sqlB.AppendLine(" `Task` ");
        sqlB.AppendLine(" , `AssignedToUserID` ");
        sqlB.AppendLine(" , `TaskTypeID` ");
        sqlB.AppendLine(" , `TaskStatusID` ");
        sqlB.AppendLine(" , `ProjectID` ");
        sqlB.AppendLine(" , `UpdatedBy` ");
        sqlB.AppendLine(" , `TimeUpdate` ");
        sqlB.AppendLine(" , `CreatedBy` ");
        sqlB.AppendLine(" , `TimeCreated` ");
        sqlB.AppendLine(") VALUES ( ");
        sqlB.AppendLine($" '{taskEntity.Task}' ");
        sqlB.AppendLine($" , '{taskEntity.AssignedToUserID}' ");
        sqlB.AppendLine($" , '{taskEntity.TaskTypeID}' ");
        sqlB.AppendLine($" , '{taskEntity.TaskStatusID}' ");
        sqlB.AppendLine($" , '{taskEntity.ProjectID}' ");
        sqlB.AppendLine($" , '1' ");
        sqlB.AppendLine(" , NOW() ");
        sqlB.AppendLine(" , '1' ");
        sqlB.AppendLine(" , NOW() ");
        sqlB.AppendLine(");");
        int addCount = BC_MySqlUtils.ExecuteSQL(sqlB.ToString());
        if (addCount == 1)
        {
            addTaskStatus = true;
            message = "新增 Task 成功！";
        }
        return new
        {
            AddTaskStatus = addTaskStatus,
            Message = message
        };
    }
}