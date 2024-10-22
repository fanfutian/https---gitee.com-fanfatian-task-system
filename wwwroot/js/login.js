    document.querySelector("#submitBtn").addEventListener("click",function(){
    //alert("你点击了提交按钮。");
    //1.获取用户名和密码
    const username = document.querySelector("input[name='username']").value;
    const password = document.querySelector("input[name='password']").value;
    
    //2.非空校验
    if (username === ""){
        alert("用户名不能为空！");
        return;
    }
   
    if (password === ""){
        alert("密码不能为空！");
        return;
    }
    
    //3.请求登录API
    //3.1准备参数 url parameters
    const url = "/API"
    const parameters = {
        APICommand:"login",
        UserName: username,
        Password: password
    }
    //3.2调用getAPICommand
    getAPICommandData(url,parameters).then((data)=>{
        const loginStatus = data.Data.LoginStatus;

        if (loginStatus) {
            alert(`${data.Data.Message} 3s 后跳转到 Task 列表界⾯！`);
            setTimeout(() => {
            location.href = "/Home?DashboardID=2";
            }, 3000);
        }
        else{
            alert(data.Data.Message);
        }

    })

})