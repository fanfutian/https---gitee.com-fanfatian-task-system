

document.querySelector("#registerBtn").addEventListener("click", function () {
    const username = document.querySelector("[name='userName']").value;
    const nickname = document.querySelector("[name='nickName']").value;
    const password = document.querySelector("[name='password']").value;
    const password1 = document.querySelector("[name='passwordAgain']").value;
    const email = document.querySelector("[name='email']").value;
    const phone = document.querySelector("[name='phone']").value;
    if(username===""){
        alert("用户名不能为空！");
        return;
    }
    if(nickname===""){
        alert("昵称不能为空！");
        return;
    }
    if(email===""){
        alert("邮箱不能为空！");
        return;
    }
    if(phone===""){
        alert("手机号不能为空！");
        return;
    }
    if(password===""){
        alert("密码不能为空！");
        return;
    }
    if(password1===""){
        alert("确认密码不能为空！");
        return;
    }
    if(password!==password1){
        alert("两次密码不一致！");
        return;
    }

    const url = "/API";
    const parameters = {
        APICommand: "register",
        UserName: username,
        NickName: nickname,
        Password: password,
        Email: email,
        Phone: phone
    };
    // 调用 API
    getAPICommandData(url, parameters).then((data) => {
        console.log(data);
        if(data.Data.RegisterStatus){
            alert(`${data.Data.Message}1s后跳转到登录界⾯!`)
            setTimeout(() => {
                location.href = "/Home?DashboardID=0";
            }, 1000);
        }else{
            alert(data.Data.Message);
        }
    });
});

