

var mongoose = require('mongoose');
//�����Զ���İ�
var models = require('./Schema');
var UserObj=require('../models/User');

var userObj=new UserObj();

exports.Login = function(req,res)
{
    var ret =
    {
        StateCode:1000,
        LogMessage:"",
        UserID:"-1"
    }
    //console.log('login');
    var post ='';
    req.on('data',function(chunk)
    {
        post+=chunk;
    });
    req.on('end',function() {
        console.log('收到的数据是' + post);
        var postObj= JSON.parse(post);
        var queryString=new Array();
        //获取用户名和密码
        for(var val in postObj)
        {
            queryString.push(postObj[val]);
        }
        userObj.get({username:queryString[0],password:queryString[1]},"_id",function(error,userid)
        {
            if(error)
            {
                ret.StateCode = 1000;
                ret.LogMessage = error;
            }
            else
            {

                if(!userid)
                {
                    //不存在
                    ret.StateCode = 1001;
                    ret.LogMessage = "username or password not found";

                }
                else
                {
                    //存在
                    ret.StateCode =1002;
                    ret.LogMessage = "Login success";
                    ret.UserID = userid;
                }
            }
            res.end(JSON.stringify(ret));
        })
    })
}



