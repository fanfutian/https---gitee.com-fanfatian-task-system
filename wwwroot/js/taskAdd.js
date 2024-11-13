loadPage();
function loadPage() {
    getSelectData();
    addClickEventForAddTaskBen();
}
function getSelectData() {
    // 1 准备参数
    const url = "/API"
    const parameters = {
        APICommand: "GetAddTaskSelectListData"
    }
    // 2 调⽤ getAPICommandData() ⽅法
    getAPICommandData(url, parameters).then((data) => {
        // 1. 获取 list 数据
        const userList = data.Data.UserList;
        const taskTypeList = data.Data.TaskTypeList;
        const taskStatusList = data.Data.TaskStatusList;
        // 2. 填充 userList
        let userListhtml = "";
        if (userList.length != 0) {
            for (let i = 0; i < userList.length; i++) {
                userListhtml += `<option value="${userList[i].UserID}">${userList[i].NickName === "" ? userList[i].UserName : userList[i].NickName}</option > `
            }
        } else {
            userListhtml = `< option value = "" > 没有数据</option > `;
        }
        document.querySelector("#assignedToUserIDSelect").innerHTML = userListhtml;
        // 3. 填充 taskTypeList
        let taskTypeListhtml = "";
        if (taskTypeList.length != 0) {
            for (let i = 0; i < taskTypeList.length; i++) {
                taskTypeListhtml += `<option value = "${taskTypeList[i].TaskTypeID} ">${taskTypeList[i].TaskType}</option>`
            }
        }
        else {
            taskTypeListhtml = `<option value="">没有数据</option>`;
        }
        document.querySelector("#taskTypeIDSelect").innerHTML = taskTypeListhtml;
        // 4. 填充 taskStatusList
        let taskStatusListhtml = "";
        if (taskStatusList.length != 0) {
            for (let i = 0; i < taskStatusList.length; i++) {
                taskStatusListhtml += `<option value="${taskStatusList[i].TaskStatusID} ">${taskStatusList[i].TaskStatus}</option>`
            }
        }
        else {
            taskStatusListhtml = `<option value="">没有数据</option>`;
        }
        document.querySelector("#taskStatusIDSelect").innerHTML = taskStatusListhtml;
    });
}


function addClickEventForAddTaskBen() {
    document.querySelector("#addTaskBtn").addEventListener("click",
        () => {
            // 1. 获取⻚⾯输⼊参数
            const task = document.querySelector("[name='Task']").value;
            const assignedToUserID = document.querySelector("#assignedToUserIDSelect").value;
            const taskTypeID = document.querySelector("#taskTypeIDSelect").value;
            const taskStatusID = document.querySelector("#taskStatusIDSelect").value;
            const projectID = document.querySelector("[name='projectID']").value;
            // 2. ⾮空校验
            if (task === "") {
                alert("task 不能为空!");
                return;
            }
            if (assignedToUserID === "") {
                alert("指派⼈不能为空!");
                return;
            }
            if (taskTypeID === "") {
                alert("task 类型不能为空!");
                return;
            }
            if (taskStatusID === "") {
                alert("task 状态不能为空!");
                return;
            }
            if (projectID === "") {
                alert("项⽬ ID 不能为空!");
                return;
            }
            // 3. 调⽤ AddTask API Command
            addTask(task, assignedToUserID, taskTypeID, taskStatusID, projectID);
        });
}


function addTask(task, assignedToUserID, taskTypeID, taskStatusID,projectID) {
    // 1 准备参数
    const url = "/API"
    const parameters = {
        APICommand: "addTask",
        Task: task,
        AssignedToUserID: assignedToUserID,
        TaskTypeID: taskTypeID,
        TaskStatusID: taskStatusID,
        ProjectID: projectID
    }
    // 2 调⽤ getAPICommandData() ⽅法
    getAPICommandData(url, parameters).then((data) => {
        if (data.Data.AddTaskStatus) {
            alert(`${data.Data.Message} 1s 后跳转 Task 列表界⾯...`);
            setTimeout(() => {
                location.href = "/Home?DashboardID=2";
            }, 1000);
        }
        else {
            alert(`${data.Data.Message}`);
        }
    });
}