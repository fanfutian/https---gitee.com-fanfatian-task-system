
loadPage();

function loadPage() {
    getTaskList("", "", "");
}

function getTaskList(taskID, taskTile, assignedToUser) {
    // 1 准备参数
    const url = "/API"
    const parameters = {
        APICommand: "taskList",
        TaskID: taskID,
        TaskTitle: taskTile,
        AssignedToUser: assignedToUser
    }
    // 2 调⽤ getAPICommandData() ⽅法
    getAPICommandData(url, parameters).then((data) => {
        // 把返回的 TaskList 循环添加⾄ tbody 中的 html 代码
        let tbodyHtml = "";
        const taskList = data.Data.TaskList;
        if (taskList.length !== 0) {
            for (let i = 0; i < taskList.length; i++) {
                tbodyHtml +=
                    "<tr>"
                    + ` <td>${taskList[i].TaskID}</td>`
                    + ` <td>${taskList[i].Task}</td>`
                    + ` <td>${taskList[i].AssignedToUserID}</td>`
                    + ` <td>${taskList[i].AssignedToUser}</td>`
                    + ` <td>${taskList[i].TaskTypeID}</td>`
                    + ` <td>${taskList[i].TaskStatusID}</td>`
                    + ` <td>${taskList[i].ProjectID}</td>`
                    + ` <td>${taskList[i].UpdatedBy}</td>`
                    + ` <td>${taskList[i].TimeUpdate}</td>`
                    + ` <td>${taskList[i].CreatedBy}</td>`
                    + ` <td>${taskList[i].TimeCreated}</td>`
                    + "<tr>"
            }
        }
        else {
            document.querySelector("#taskTableContainer").innerHTML = "未查询到 Task 数据！";
            return;
        }
        // 将 tbodyHtml 代码加载回⻚⾯
        document.querySelector("#taskListBody").innerHTML = tbodyHtml;
    })
}


document.querySelector("#queryBtn").addEventListener("click", () => {
    const taskID = document.querySelector("[name='taskID']").value;
    const taskTile = document.querySelector("[name='task']").value;
    const assignedToUser = document.querySelector("[name='assignedToUser']").value;
    getTaskList(taskID, taskTile, assignedToUser);
});