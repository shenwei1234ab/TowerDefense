var mongoose = require('mongoose');
var models = require('../routes/Schema');
var Operator = models.Operator;

function OperatorObj(name,password)
{
    this.name=name;
    this.password=password;
};
module.exports = Operator;

//辨别用户
Operator.prototype.identify=function identify(callback)
{
    var inputInfo=
    {
        name:this.name,
        password:this.password
    };

    Operator.count(inputInfo, function (err, doc)
    {
        if(err)
        {
            return callback(err);
        }
        else
        {
            return callback(null,doc);
        }
    });
};/**
 * Created by sw on 2015/6/10.
 */
