var mongoose = require('mongoose');
var models = require('../routes/Schema');
var otherModel = models.Others;

module.exports = OtherObj;

function OtherObj()
{

};



//查询meals指定字段
OtherObj.prototype.get = function get(query,field,callback)
{
    otherModel.find(query,field,function(error,doc)
    {
        if(error)
        {
            return callback(error);
        }
        else
        {
            return callback(null,doc);
        }
    });
}



