# TaskSystem

#### 介绍
计算机实用技术课程关于简单web应用的实践



#### 使用说明

1. 下载成功后将数据库改为自己的数据库
    文件位置：ERP/common/MySqlUtils.cs
![输入图片说明](https://foruda.gitee.com/images/1730129037370864877/87cac087_14048343.png "屏幕截图")
2. 更改文件：ERP/business/Users.cs下的注册数据的数据库名
![输入图片说明](https://foruda.gitee.com/images/1730129343270541462/1f3d8a98_14048343.png "屏幕截图")
3. 创建需要的表结构，并添加数据，修改数据库名为自己的数据库名称,我这里是`db_task`
```
--  创建tasks表结构
CREATE TABLE IF NOT EXISTS `db_task`.`tasks` (
    `TaskID` int NOT NULL AUTO_INCREMENT,
    `Task` varchar(64) NOT NULL,
    `AssignedToUserID` int NOT NULL,
    `TaskTypeID` int NOT NULL,
    `TaskStatusID` int NOT NULL,
    `ProjectID` int NOT NULL,
    `UpdatedBy` varchar(45) DEFAULT NULL,
    `TimeUpdate` datetime NOT NULL,
    `CreatedBy` varchar(45) DEFAULT NULL,
    `TimeCreated` datetime NOT NULL,
    PRIMARY KEY (`TaskID`)
) ENGINE = InnoDB AUTO_INCREMENT = 3 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci


INSERT INTO `db_task`.`tasks`(`TaskID`,`Task`,`AssignedToUserID`,`TaskTypeID`,`TaskStatusID`,`ProjectID`,`UpdatedBy`,`TimeUpdate`,`CreatedBy`,`TimeCreated`) VALUES(1,'登录功能',3,1,1,1,'1','2024-01-01 00:00:00','1','2024-01-01 00:00:00');
INSERT INTO `db_task`.`tasks`(`TaskID`,`Task`,`AssignedToUserID`,`TaskTypeID`,`TaskStatusID`,`ProjectID`,`UpdatedBy`,`TimeUpdate`,`TimeCreated`,`CreatedBy`) VALUES(2,'注册功能',1,1,1,1,'1','2024-01-02 00:02:23','2024-01-02 00:02:23','1');




-- 创建Task_types表结构
CREATE TABLE IF NOT EXISTS `db_task`.`task_types` (
    `TaskTypeID` INT NOT NULL AUTO_INCREMENT,
    `TaskType` VARCHAR(32) NOT NULL,
    `UpdatedBy` VARCHAR(45) NULL,
    `TimeUpdate` DATETIME NOT NULL,
    `CreatedBy` VARCHAR(45) NULL,
    `TimeCreated` DATETIME NOT NULL,
    PRIMARY KEY (`TaskTypeID`)
);

INSERT INTO `db_task`.`task_types` (`TaskType`,`UpdatedBy`,`TimeUpdate`,`CreatedBy`,`TimeCreated`)
            VALUES ('需求确认','1','2024-01-01','1','2024-01-01');

INSERT INTO `db_task`.`task_types` (`TaskType`,`UpdatedBy`,`TimeUpdate`,`CreatedBy`,`TimeCreated`)
            VALUES ('功能','1','2024-01-01','1','2024-01-01');

INSERT INTO `db_task`.`task_types` (`TaskType`,`UpdatedBy`,`TimeUpdate`,`CreatedBy`,`TimeCreated`)
            VALUES ('缺陷','1','2024-01-01','1','2024-01-01');

INSERT INTO `db_task`.`task_types` (`TaskType`,`UpdatedBy`,`TimeUpdate`,`CreatedBy`,`TimeCreated`)
            VALUES ('⽂档','1','2024-01-01','1','2024-01-01');

INSERT INTO `db_task`.`task_types` (`TaskType`,`UpdatedBy`,`TimeUpdate`,`CreatedBy`,`TimeCreated`)
            VALUES ('测试','1','2024-01-01','1','2024-01-01');




-- 创建task_status表结构
CREATE TABLE `db_task`.`task_status` (
    `TaskStatusID` INT NOT NULL AUTO_INCREMENT,
    `TaskStatus` VARCHAR(32) NOT NULL,
    `UpdatedBy` VARCHAR(45) NULL,
    `TimeUpdate` DATETIME NOT NULL,
    `CreatedBy` VARCHAR(45) NULL,
    `TimeCreated` DATETIME NOT NULL,
    PRIMARY KEY (`TaskStatusID`)
);

INSERT INTO`db_task`.`task_status` (`TaskStatus`,`UpdatedBy`,`TimeUpdate`,`CreatedBy`,`TimeCreated`)
            VALUES ('指派','1','2024-01-01','1','2024-01-01');

INSERT INTO `db_task`.`task_status` (`TaskStatus`,`UpdatedBy`,`TimeUpdate`,`CreatedBy`,`TimeCreated`)
            VALUES ('开发中','1','2024-01-01','1','2024-01-01');

INSERT INTO `db_task`.`task_status` (`TaskStatus`,`UpdatedBy`,`TimeUpdate`,`CreatedBy`,`TimeCreated`)
            VALUES ('测试中','1','2024-01-01','1','2024-01-01');

INSERT INTO `db_task`.`task_status` (`TaskStatus`,`UpdatedBy`,`TimeUpdate`,`CreatedBy`,`TimeCreated`)
            VALUES ('等待升级','1','2024-01-01','1','2024-01-01');

INSERT INTO `db_task`.`task_status` (`TaskStatus`,`UpdatedBy`,`TimeUpdate`,`CreatedBy`,`TimeCreated`)
            VALUES ('功能确认','1','2024-01-01','1','2024-01-01');

INSERT INTO `db_task`.`task_status` (`TaskStatus`,`UpdatedBy`,`TimeUpdate`,`CreatedBy`,`TimeCreated`)
            VALUES ('已完成','1','2024-01-01','1','2024-01-01');

```



