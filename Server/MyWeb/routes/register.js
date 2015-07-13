

var mongoose = require('mongoose');
//�����Զ���İ�
var models = require('./Schema');
var UserObj=require('../models/User');

var userObj=new UserObj();

exports.Register = function(req,res)
{

    var ret =
    {
        StateCode:1000,
        LogMessage:"",
        UserID:"-1"
    }
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
        var username = queryString[0];
        var password = queryString[1];
        userObj.get({username:username},"_id",function(error,userid)
        {
            if(error)
            {
                ret.LogMessage = error;
            }
            else
            {
                if(userid)
                {
                    //用户名存在
                    ret.StateCode = 1003;
                    ret.LogMessage = "username existed";

                }
                else
                {
                    userObj.save(username,password,function(error,userid)
                    {
                        if(error)
                        {
                            ret.LogMessage = error;
                        }
                        else
                        {
                            ret.StateCode =1004;
                            ret.LogMessage = "RegisterSuccess";
                            ret.UserID = userid;
                        }
                    })
                }
                res.end(JSON.stringify(ret));
            }

        })
    })
}
