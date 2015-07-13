/**
 * Created by sw on 2015/6/9.
 */
var mongoose = require('mongoose');
var models = require('../routes/Schema');
var userModel = models.User;

function UserObj()
{

};
module.exports = UserObj;


UserObj.prototype.get = function get(queryStr,queryfield,callback)
{
    userModel.findOne(queryStr,{},function(err,doc)
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


UserObj.prototype.save = function save(username,password,callback)
{
    var userEntity = new userModel(
    {
        username:username,
        password:password
    });
    var userid = userEntity._id;
    userEntity.save(function(error)
    {
        if(error)
        {
            callback(error);
        }
        else
        {
            callback(null,userid)
        }
    })
}






