/**
 * Created by sw on 2015/6/9.
 */
var mongoose = require('mongoose');
var models = require('../routes/Schema');
var User = models.User;

function UserObj()
{

};
module.exports = UserObj;


UserObj.prototype.get = function get(queryStr,queryfield,callback)
{
    User.findOne(queryStr,{},function(err,doc)
    {
        if(err)
        {
            callback(err);
        }
        else
        {
            if(!doc)
            {
                callback(null,null);
            }
            else
            {
                callback(null,doc[queryfield]);
            }
        }
    })
}

//辨别用户是否存在
UserObj.prototype.identify=function identify(username,password,callback)
{
    console.log("identify");
    var inputInfo=
    {
       username:username,
       password:password

    };

  /*  User.count(inputInfo, function (err, doc)
    {
        if(err)
        {
            return callback(err);
        }
        else
        {
             return callback(null,doc);
        }
    });*/

    User.findOne(inputInfo,{},function(err,doc)
    {
        if (!doc)
            err = "没有找到对应的用户";
        if(err)
        {
            return callback(null);
        }
        else
        {
            console.log("doc is "+doc);
            console.log("doc.username"+doc.username);
            console.log("doc.type " +doc.type);
            return callback(null,doc);
        }
    });
};


